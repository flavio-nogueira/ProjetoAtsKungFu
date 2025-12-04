using System.ComponentModel.DataAnnotations;

namespace ApiAtsKungFu.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para revogar token (logout)
    /// </summary>
    public class RevokeTokenDto
    {
        /// <summary>
        /// Refresh token a ser revogado
        /// </summary>
        [Required(ErrorMessage = "Refresh token é obrigatório")]
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Motivo da revogação (opcional)
        /// </summary>
        public string? Motivo { get; set; }
    }
}
