using System.ComponentModel.DataAnnotations;

namespace ApiAtsKungFu.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para renovação de token
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// Access token expirado
        /// </summary>
        [Required(ErrorMessage = "Access token é obrigatório")]
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Refresh token válido
        /// </summary>
        [Required(ErrorMessage = "Refresh token é obrigatório")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
