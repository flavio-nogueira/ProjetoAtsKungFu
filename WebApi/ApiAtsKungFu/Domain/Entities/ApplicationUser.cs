using Microsoft.AspNetCore.Identity;

namespace ApiAtsKungFu.Domain.Entities
{
    /// <summary>
    /// Entidade de usuário da aplicação com campos personalizados
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        public string NomeCompleto { get; set; } = string.Empty;

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string? CPF { get; set; }

        /// <summary>
        /// Data de nascimento
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// URL da foto de perfil
        /// </summary>
        public string? FotoPerfil { get; set; }

        /// <summary>
        /// Indica se o usuário está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Data de criação do registro
        /// </summary>
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Data da última alteração
        /// </summary>
        public DateTime? DataAlteracao { get; set; }

        /// <summary>
        /// Data do último login
        /// </summary>
        public DateTime? DataUltimoLogin { get; set; }

        /// <summary>
        /// Coleção de tokens de refresh do usuário
        /// </summary>
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
