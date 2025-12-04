using ApiAtsKungFu.Application.DTOs.Auth;
using ApiAtsKungFu.Application.Interfaces;
using ApiAtsKungFu.Domain.Entities;
using ApiAtsKungFu.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiAtsKungFu.Application.Services
{
    /// <summary>
    /// Serviço de autenticação com JWT e RefreshToken
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenResponseDto> RegistrarAsync(RegisterUsuarioDto dto, string? ipAddress = null, string? userAgent = null)
        {
            Log.Information("Tentando registrar novo usuário: {Email}", dto.Email);

            // Verificar se email já existe
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                Log.Warning("Tentativa de registro com email já existente: {Email}", dto.Email);
                throw new InvalidOperationException("Email já cadastrado");
            }

            // Criar novo usuário
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = dto.Email,
                Email = dto.Email,
                NomeCompleto = dto.NomeCompleto,
                CPF = dto.CPF,
                PhoneNumber = dto.Telefone,
                DataNascimento = dto.DataNascimento,
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Senha);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                Log.Error("Erro ao criar usuário {Email}: {Errors}", dto.Email, errors);
                throw new InvalidOperationException($"Erro ao criar usuário: {errors}");
            }

            Log.Information("Usuário criado com sucesso: {UserId} - {Email}", user.Id, user.Email);

            // Gerar tokens
            return await GerarTokensAsync(user, false, ipAddress, userAgent);
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto dto, string? ipAddress = null, string? userAgent = null)
        {
            Log.Information("Tentativa de login para: {Email}", dto.Email);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                Log.Warning("Tentativa de login com email não encontrado: {Email}", dto.Email);
                throw new UnauthorizedAccessException("Email ou senha inválidos");
            }

            if (!user.Ativo)
            {
                Log.Warning("Tentativa de login com usuário inativo: {Email}", dto.Email);
                throw new UnauthorizedAccessException("Usuário inativo");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Senha, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                Log.Warning("Falha no login para {Email}: {Reason}", dto.Email,
                    result.IsLockedOut ? "Bloqueado" : result.IsNotAllowed ? "Não permitido" : "Senha inválida");
                throw new UnauthorizedAccessException("Email ou senha inválidos");
            }

            // Atualizar último login
            user.DataUltimoLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            Log.Information("Login bem-sucedido para: {UserId} - {Email}", user.Id, user.Email);

            // Gerar tokens
            return await GerarTokensAsync(user, dto.LembrarMe, ipAddress, userAgent);
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenDto dto, string? ipAddress = null, string? userAgent = null)
        {
            Log.Information("Tentativa de refresh token");

            // Validar access token expirado (sem validar expiração)
            var principal = ObterPrincipalDeTokenExpirado(dto.AccessToken);
            if (principal == null)
            {
                Log.Warning("Access token inválido no refresh");
                throw new UnauthorizedAccessException("Token inválido");
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                Log.Warning("UserId não encontrado no token");
                throw new UnauthorizedAccessException("Token inválido");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.Ativo)
            {
                Log.Warning("Usuário não encontrado ou inativo no refresh: {UserId}", userId);
                throw new UnauthorizedAccessException("Usuário inválido ou inativo");
            }

            // Buscar refresh token no banco
            var storedToken = await _context.Set<RefreshToken>()
                .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken && rt.UserId == user.Id);

            if (storedToken == null)
            {
                Log.Warning("Refresh token não encontrado: {UserId}", userId);
                throw new UnauthorizedAccessException("Refresh token inválido");
            }

            if (!storedToken.IsValido)
            {
                Log.Warning("Refresh token inválido: usado={IsUsed}, revogado={IsRevoked}, expirado={Expired}",
                    storedToken.IsUsed, storedToken.IsRevoked, storedToken.DataExpiracao < DateTime.UtcNow);
                throw new UnauthorizedAccessException("Refresh token expirado ou revogado");
            }

            // Marcar token como usado
            storedToken.IsUsed = true;
            storedToken.DataUso = DateTime.UtcNow;

            // Gerar novos tokens (token rotation)
            var response = await GerarTokensAsync(user, false, ipAddress, userAgent);

            // Vincular token antigo ao novo
            var newRefreshToken = await _context.Set<RefreshToken>()
                .FirstOrDefaultAsync(rt => rt.Token == response.RefreshToken);

            if (newRefreshToken != null)
            {
                storedToken.SubstituidoPorToken = newRefreshToken.Id;
            }

            await _context.SaveChangesAsync();

            Log.Information("Refresh token realizado com sucesso para: {UserId}", userId);

            return response;
        }

        public async Task RevogarTokenAsync(RevokeTokenDto dto, string? ipAddress = null)
        {
            Log.Information("Tentativa de revogação de token");

            var token = await _context.Set<RefreshToken>()
                .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken);

            if (token == null)
            {
                Log.Warning("Tentativa de revogar token não encontrado");
                throw new InvalidOperationException("Token não encontrado");
            }

            if (token.IsRevoked)
            {
                Log.Warning("Token já estava revogado: {TokenId}", token.Id);
                throw new InvalidOperationException("Token já foi revogado");
            }

            token.IsRevoked = true;
            token.DataRevogacao = DateTime.UtcNow;
            token.MotivoRevogacao = dto.Motivo ?? "Logout do usuário";

            await _context.SaveChangesAsync();

            Log.Information("Token revogado com sucesso: {TokenId} - {UserId}", token.Id, token.UserId);
        }

        public async Task<string> SolicitarRecuperacaoSenhaAsync(ForgotPasswordDto dto)
        {
            Log.Information("Solicitação de recuperação de senha para: {Email}", dto.Email);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                // Por segurança, não revelar se o email existe
                Log.Warning("Tentativa de recuperação para email não encontrado: {Email}", dto.Email);
                return "Se o email existir, você receberá instruções para redefinir sua senha";
            }

            if (!user.Ativo)
            {
                Log.Warning("Tentativa de recuperação para usuário inativo: {Email}", dto.Email);
                return "Se o email existir, você receberá instruções para redefinir sua senha";
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // TODO: Implementar envio de email com o token
            // Por enquanto, registrar no log (REMOVER EM PRODUÇÃO!)
            Log.Information("Token de reset gerado para {Email}: {Token}", dto.Email, token);

            return "Se o email existir, você receberá instruções para redefinir sua senha";
        }

        public async Task ResetarSenhaAsync(ResetPasswordDto dto)
        {
            Log.Information("Tentativa de reset de senha para: {Email}", dto.Email);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                Log.Warning("Tentativa de reset para email não encontrado: {Email}", dto.Email);
                throw new InvalidOperationException("Token inválido ou expirado");
            }

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NovaSenha);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                Log.Error("Erro ao resetar senha para {Email}: {Errors}", dto.Email, errors);
                throw new InvalidOperationException($"Erro ao resetar senha: {errors}");
            }

            // Revogar todos os refresh tokens do usuário
            await RevogarTodosTokensUsuarioAsync(user.Id, "Reset de senha");

            Log.Information("Senha resetada com sucesso para: {UserId} - {Email}", user.Id, user.Email);
        }

        public async Task AlterarSenhaAsync(Guid userId, ChangePasswordDto dto)
        {
            Log.Information("Tentativa de alteração de senha para: {UserId}", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                Log.Warning("Usuário não encontrado na alteração de senha: {UserId}", userId);
                throw new InvalidOperationException("Usuário não encontrado");
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.SenhaAtual, dto.NovaSenha);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                Log.Error("Erro ao alterar senha para {UserId}: {Errors}", userId, errors);
                throw new InvalidOperationException($"Erro ao alterar senha: {errors}");
            }

            Log.Information("Senha alterada com sucesso para: {UserId}", userId);
        }

        public async Task<UsuarioDto> ObterUsuarioAtualAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new InvalidOperationException("Usuário não encontrado");
            }

            return new UsuarioDto
            {
                Id = user.Id,
                NomeCompleto = user.NomeCompleto,
                Email = user.Email!,
                CPF = user.CPF,
                Telefone = user.PhoneNumber,
                DataNascimento = user.DataNascimento,
                FotoPerfil = user.FotoPerfil,
                EmailConfirmado = user.EmailConfirmed,
                TelefoneConfirmado = user.PhoneNumberConfirmed,
                DataCriacao = user.DataCriacao,
                DataUltimoLogin = user.DataUltimoLogin
            };
        }

        public async Task RevogarTodosTokensUsuarioAsync(Guid userId, string motivo)
        {
            Log.Information("Revogando todos os tokens do usuário: {UserId}", userId);

            var tokens = await _context.Set<RefreshToken>()
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.DataRevogacao = DateTime.UtcNow;
                token.MotivoRevogacao = motivo;
            }

            await _context.SaveChangesAsync();

            Log.Information("Total de {Count} tokens revogados para usuário {UserId}", tokens.Count, userId);
        }

        #region Métodos Privados

        private async Task<TokenResponseDto> GerarTokensAsync(ApplicationUser user, bool lembrarMe, string? ipAddress, string? userAgent)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JwtSettings:SecretKey não configurado");
            var issuer = jwtSettings["Issuer"] ?? "ApiAtsKungFu";
            var audience = jwtSettings["Audience"] ?? "ApiAtsKungFu";
            var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

            // Gerar JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("nome_completo", user.NomeCompleto),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            // Gerar Refresh Token
            var refreshToken = GerarRefreshTokenSeguro();
            var refreshTokenExpiration = lembrarMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(7);

            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = refreshToken,
                JwtId = tokenDescriptor.Subject.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value,
                DataCriacao = DateTime.UtcNow,
                DataExpiracao = refreshTokenExpiration,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            _context.Set<RefreshToken>().Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            var usuarioDto = new UsuarioDto
            {
                Id = user.Id,
                NomeCompleto = user.NomeCompleto,
                Email = user.Email!,
                CPF = user.CPF,
                Telefone = user.PhoneNumber,
                DataNascimento = user.DataNascimento,
                FotoPerfil = user.FotoPerfil,
                EmailConfirmado = user.EmailConfirmed,
                TelefoneConfirmado = user.PhoneNumberConfirmed,
                DataCriacao = user.DataCriacao,
                DataUltimoLogin = user.DataUltimoLogin
            };

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresIn = expirationMinutes * 60,
                ExpiresAt = tokenDescriptor.Expires.Value,
                Usuario = usuarioDto
            };
        }

        private string GerarRefreshTokenSeguro()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private ClaimsPrincipal? ObterPrincipalDeTokenExpirado(string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JwtSettings:SecretKey não configurado");
                var issuer = jwtSettings["Issuer"] ?? "ApiAtsKungFu";
                var audience = jwtSettings["Audience"] ?? "ApiAtsKungFu";

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false, // Não validar expiração
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
