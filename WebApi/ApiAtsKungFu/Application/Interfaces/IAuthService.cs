using ApiAtsKungFu.Application.DTOs.Auth;

namespace ApiAtsKungFu.Application.Interfaces
{
    /// <summary>
    /// Interface de serviço de autenticação
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registra um novo usuário no sistema
        /// </summary>
        Task<TokenResponseDto> RegistrarAsync(RegisterUsuarioDto dto, string? ipAddress = null, string? userAgent = null);

        /// <summary>
        /// Realiza login de usuário e retorna tokens
        /// </summary>
        Task<TokenResponseDto> LoginAsync(LoginDto dto, string? ipAddress = null, string? userAgent = null);

        /// <summary>
        /// Renova o access token usando refresh token
        /// </summary>
        Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenDto dto, string? ipAddress = null, string? userAgent = null);

        /// <summary>
        /// Revoga refresh token (logout)
        /// </summary>
        Task RevogarTokenAsync(RevokeTokenDto dto, string? ipAddress = null);

        /// <summary>
        /// Solicita recuperação de senha (envia email com token)
        /// </summary>
        Task<string> SolicitarRecuperacaoSenhaAsync(ForgotPasswordDto dto);

        /// <summary>
        /// Reseta senha com token recebido por email
        /// </summary>
        Task ResetarSenhaAsync(ResetPasswordDto dto);

        /// <summary>
        /// Altera senha de usuário autenticado
        /// </summary>
        Task AlterarSenhaAsync(Guid userId, ChangePasswordDto dto);

        /// <summary>
        /// Obtém informações do usuário autenticado
        /// </summary>
        Task<UsuarioDto> ObterUsuarioAtualAsync(Guid userId);

        /// <summary>
        /// Revoga todos os tokens de um usuário
        /// </summary>
        Task RevogarTodosTokensUsuarioAsync(Guid userId, string motivo);
    }
}
