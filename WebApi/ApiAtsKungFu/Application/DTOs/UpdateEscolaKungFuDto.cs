using System.ComponentModel.DataAnnotations;

namespace ApiAtsKungFu.Application.DTOs
{
    public class UpdateEscolaKungFuDto
    {
        [Required(ErrorMessage = "Razão Social é obrigatória")]
        [StringLength(200)]
        public string RazaoSocial { get; set; } = string.Empty;

        [StringLength(200)]
        public string? NomeFantasia { get; set; }

        [StringLength(20)]
        public string? InscricaoEstadual { get; set; }

        [StringLength(20)]
        public string? InscricaoMunicipal { get; set; }

        [StringLength(10)]
        public string? CNAEPrincipal { get; set; }

        [StringLength(500)]
        public string? CNAESecundarios { get; set; }

        [StringLength(50)]
        public string? RegimeTributario { get; set; }

        // Endereço
        [Required(ErrorMessage = "Logradouro é obrigatório")]
        [StringLength(200)]
        public string Logradouro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Número é obrigatório")]
        [StringLength(10)]
        public string Numero { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "Bairro é obrigatório")]
        [StringLength(100)]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cidade é obrigatória")]
        [StringLength(100)]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "UF é obrigatório")]
        [StringLength(2, MinimumLength = 2)]
        public string UF { get; set; } = string.Empty;

        [Required(ErrorMessage = "CEP é obrigatório")]
        [StringLength(10)]
        public string CEP { get; set; } = string.Empty;

        [StringLength(50)]
        public string Pais { get; set; } = "Brasil";

        // Contato
        [StringLength(20)]
        public string? TelefoneFixo { get; set; }

        [StringLength(20)]
        public string? CelularWhatsApp { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? Site { get; set; }

        [StringLength(200)]
        public string? NomeResponsavel { get; set; }

        // Auditoria
        [Required(ErrorMessage = "ID do usuário que alterou é obrigatório")]
        public Guid IdUsuarioAlterou { get; set; }
    }
}
