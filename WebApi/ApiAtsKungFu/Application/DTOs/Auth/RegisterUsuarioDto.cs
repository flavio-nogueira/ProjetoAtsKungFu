using System.ComponentModel.DataAnnotations;

namespace ApiAtsKungFu.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para registro de novo usuário
    /// </summary>
    public class RegisterUsuarioDto
    {
        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [StringLength(200, ErrorMessage = "Nome completo deve ter no máximo 200 caracteres")]
        public string NomeCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário (será usado como username)
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// CPF do usuário (opcional)
        /// </summary>
        [StringLength(14, ErrorMessage = "CPF inválido")]
        public string? CPF { get; set; }

        /// <summary>
        /// Telefone/celular do usuário
        /// </summary>
        [Phone(ErrorMessage = "Telefone inválido")]
        public string? Telefone { get; set; }

        /// <summary>
        /// Data de nascimento
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
        public string Senha { get; set; } = string.Empty;

        /// <summary>
        /// Confirmação da senha
        /// </summary>
        [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
