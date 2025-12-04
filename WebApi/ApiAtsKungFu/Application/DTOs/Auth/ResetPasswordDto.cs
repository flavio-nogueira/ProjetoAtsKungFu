using System.ComponentModel.DataAnnotations;

namespace ApiAtsKungFu.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para resetar senha com token
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// Email do usuário
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Token de reset recebido por email
        /// </summary>
        [Required(ErrorMessage = "Token é obrigatório")]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Nova senha
        /// </summary>
        [Required(ErrorMessage = "Nova senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
        public string NovaSenha { get; set; } = string.Empty;

        /// <summary>
        /// Confirmação da nova senha
        /// </summary>
        [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;
    }
}
