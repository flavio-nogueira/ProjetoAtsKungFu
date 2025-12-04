using ApiAtsKungFu.Application.DTOs.Auth;
using ApiAtsKungFu.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiAtsKungFu.Controllers
{
    /// <summary>
    /// Controller de autenticação e autorização
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Registra um novo usuário no sistema
        /// </summary>
        /// <remarks>
        /// Cria uma nova conta de usuário com email, senha e dados pessoais.
        ///
        /// Exemplo de requisição:
        ///
        ///     POST /api/Auth/register
        ///     {
        ///        "nomeCompleto": "João Silva",
        ///        "email": "joao.silva@email.com",
        ///        "cpf": "123.456.789-00",
        ///        "telefone": "(11) 98765-4321",
        ///        "dataNascimento": "1990-01-15",
        ///        "senha": "SenhaForte@123",
        ///        "confirmarSenha": "SenhaForte@123"
        ///     }
        ///
        /// </remarks>
        /// <param name="dto">Dados do novo usuário</param>
        /// <returns>Tokens de autenticação e dados do usuário</returns>
        /// <response code="201">Usuário criado com sucesso, retorna tokens</response>
        /// <response code="400">Dados inválidos ou email já cadastrado</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponseDto>> Register([FromBody] RegisterUsuarioDto dto)
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

                var response = await _authService.RegistrarAsync(dto, ipAddress, userAgent);

                return CreatedAtAction(nameof(Me), response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar usuário");
                return StatusCode(500, new { message = "Erro ao registrar usuário" });
            }
        }

        /// <summary>
        /// Realiza login de usuário
        /// </summary>
        /// <remarks>
        /// Autentica um usuário e retorna tokens JWT para acesso à API.
        ///
        /// Exemplo de requisição:
        ///
        ///     POST /api/Auth/login
        ///     {
        ///        "email": "joao.silva@email.com",
        ///        "senha": "SenhaForte@123",
        ///        "lembrarMe": true
        ///     }
        ///
        /// </remarks>
        /// <param name="dto">Credenciais de login</param>
        /// <returns>Tokens de autenticação e dados do usuário</returns>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="401">Credenciais inválidas</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto dto)
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

                var response = await _authService.LoginAsync(dto, ipAddress, userAgent);

                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer login");
                return StatusCode(500, new { message = "Erro ao fazer login" });
            }
        }

        /// <summary>
        /// Renova o access token usando refresh token
        /// </summary>
        /// <remarks>
        /// Permite renovar um access token expirado usando um refresh token válido.
        /// Implementa token rotation para maior segurança.
        ///
        /// Exemplo de requisição:
        ///
        ///     POST /api/Auth/refresh
        ///     {
        ///        "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        ///        "refreshToken": "CfDJ8O+..."
        ///     }
        ///
        /// </remarks>
        /// <param name="dto">Tokens para renovação</param>
        /// <returns>Novos tokens de autenticação</returns>
        /// <response code="200">Tokens renovados com sucesso</response>
        /// <response code="401">Tokens inválidos ou expirados</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

                var response = await _authService.RefreshTokenAsync(dto, ipAddress, userAgent);

                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao renovar token");
                return StatusCode(500, new { message = "Erro ao renovar token" });
            }
        }

        /// <summary>
        /// Revoga refresh token (logout)
        /// </summary>
        /// <remarks>
        /// Revoga um refresh token específico, efetivamente fazendo logout do dispositivo.
        ///
        /// Exemplo de requisição:
        ///
        ///     POST /api/Auth/revoke
        ///     {
        ///        "refreshToken": "CfDJ8O+...",
        ///        "motivo": "Logout do usuário"
        ///     }
        ///
        /// </remarks>
        /// <param name="dto">Token a ser revogado</param>
        /// <returns>Confirmação de revogação</returns>
        /// <response code="200">Token revogado com sucesso</response>
        /// <response code="400">Token inválido ou já revogado</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto dto)
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                await _authService.RevogarTokenAsync(dto, ipAddress);

                return Ok(new { message = "Token revogado com sucesso" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao revogar token");
                return StatusCode(500, new { message = "Erro ao revogar token" });
            }
        }

        /// <summary>
        /// Solicita recuperação de senha
        /// </summary>
        /// <remarks>
        /// Envia um email com token para redefinição de senha.
        ///
        /// Exemplo de requisição:
        ///
        ///     POST /api/Auth/forgot-password
        ///     {
        ///        "email": "joao.silva@email.com"
        ///     }
        ///
        /// </remarks>
        /// <param name="dto">Email do usuário</param>
        /// <returns>Mensagem de confirmação</returns>
        /// <response code="200">Email de recuperação enviado (se existir)</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            try
            {
                var message = await _authService.SolicitarRecuperacaoSenhaAsync(dto);

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao solicitar recuperação de senha");
                return StatusCode(500, new { message = "Erro ao solicitar recuperação de senha" });
            }
        }

        /// <summary>
        /// Reseta senha com token recebido por email
        /// </summary>
        /// <remarks>
        /// Permite redefinir a senha usando o token recebido por email.
        ///
        /// Exemplo de requisição:
        ///
        ///     POST /api/Auth/reset-password
        ///     {
        ///        "email": "joao.silva@email.com",
        ///        "token": "CfDJ8O+...",
        ///        "novaSenha": "NovaSenhaForte@123",
        ///        "confirmarNovaSenha": "NovaSenhaForte@123"
        ///     }
        ///
        /// </remarks>
        /// <param name="dto">Dados para reset de senha</param>
        /// <returns>Confirmação de senha alterada</returns>
        /// <response code="200">Senha resetada com sucesso</response>
        /// <response code="400">Token inválido ou expirado</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                await _authService.ResetarSenhaAsync(dto);

                return Ok(new { message = "Senha resetada com sucesso" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao resetar senha");
                return StatusCode(500, new { message = "Erro ao resetar senha" });
            }
        }

        /// <summary>
        /// Altera senha do usuário autenticado
        /// </summary>
        /// <remarks>
        /// Permite que o usuário autenticado altere sua senha informando a senha atual.
        ///
        /// **Requer autenticação (Bearer token)**
        ///
        /// Exemplo de requisição:
        ///
        ///     POST /api/Auth/change-password
        ///     Authorization: Bearer {seu_token}
        ///     {
        ///        "senhaAtual": "SenhaAtual@123",
        ///        "novaSenha": "NovaSenhaForte@123",
        ///        "confirmarNovaSenha": "NovaSenhaForte@123"
        ///     }
        ///
        /// </remarks>
        /// <param name="dto">Senhas para alteração</param>
        /// <returns>Confirmação de senha alterada</returns>
        /// <response code="200">Senha alterada com sucesso</response>
        /// <response code="400">Senha atual incorreta</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Usuário não autenticado" });
                }

                await _authService.AlterarSenhaAsync(Guid.Parse(userId), dto);

                return Ok(new { message = "Senha alterada com sucesso" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar senha");
                return StatusCode(500, new { message = "Erro ao alterar senha" });
            }
        }

        /// <summary>
        /// Obtém informações do usuário autenticado
        /// </summary>
        /// <remarks>
        /// Retorna os dados completos do usuário logado.
        ///
        /// **Requer autenticação (Bearer token)**
        ///
        /// Exemplo de requisição:
        ///
        ///     GET /api/Auth/me
        ///     Authorization: Bearer {seu_token}
        ///
        /// </remarks>
        /// <returns>Dados do usuário autenticado</returns>
        /// <response code="200">Retorna dados do usuário</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioDto>> Me()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Usuário não autenticado" });
                }

                var usuario = await _authService.ObterUsuarioAtualAsync(Guid.Parse(userId));

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter dados do usuário");
                return StatusCode(500, new { message = "Erro ao obter dados do usuário" });
            }
        }
    }
}
