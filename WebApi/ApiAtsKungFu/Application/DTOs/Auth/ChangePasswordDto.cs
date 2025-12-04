using System.ComponentModel.DataAnnotations;

namespace ApiAtsKungFu.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para alterar senha (usuário autenticado)
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// Senha atual
        /// </summary>
        [Required(ErrorMessage = "Senha atual é obrigatória")]
        public string SenhaAtual { get; set; } = string.Empty;

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
