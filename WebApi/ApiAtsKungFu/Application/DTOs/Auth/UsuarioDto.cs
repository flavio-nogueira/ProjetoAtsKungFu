namespace ApiAtsKungFu.Application.DTOs.Auth
{
    /// <summary>
    /// DTO com informações do usuário autenticado
    /// </summary>
    public class UsuarioDto
    {
        /// <summary>
        /// ID único do usuário
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        public string NomeCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string? CPF { get; set; }

        /// <summary>
        /// Telefone do usuário
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// Data de nascimento
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// URL da foto de perfil
        /// </summary>
        public string? FotoPerfil { get; set; }

        /// <summary>
        /// Indica se o email foi confirmado
        /// </summary>
        public bool EmailConfirmado { get; set; }

        /// <summary>
        /// Indica se o telefone foi confirmado
        /// </summary>
        public bool TelefoneConfirmado { get; set; }

        /// <summary>
        /// Data de criação da conta
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Data do último login
        /// </summary>
        public DateTime? DataUltimoLogin { get; set; }
    }
}
