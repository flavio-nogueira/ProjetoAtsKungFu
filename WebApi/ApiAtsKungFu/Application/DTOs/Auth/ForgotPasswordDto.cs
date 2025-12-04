using System.ComponentModel.DataAnnotations;

namespace ApiAtsKungFu.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para solicitar recuperação de senha
    /// </summary>
    public class ForgotPasswordDto
    {
        /// <summary>
        /// Email do usuário
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
    }
}
