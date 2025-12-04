namespace ApiAtsKungFu.Application.DTOs.Auth
{
    /// <summary>
    /// DTO de resposta com tokens de autenticação
    /// </summary>
    public class TokenResponseDto
    {
        /// <summary>
        /// Token JWT de acesso
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Token de refresh para renovação
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do token (sempre "Bearer")
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Tempo de expiração do access token em segundos
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Data/hora de expiração do token
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Dados do usuário autenticado
        /// </summary>
        public UsuarioDto? Usuario { get; set; }
    }
}
