using System.ComponentModel.DataAnnotations;

namespace ApiAtsKungFu.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para login de usuário
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Email do usuário
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário
        /// </summary>
        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;

        /// <summary>
        /// Lembrar-me (token de refresh com validade maior)
        /// </summary>
        public bool LembrarMe { get; set; } = false;
    }
}
