namespace ApiAtsKungFu.Application.DTOs
{
    public class EscolaKungFuDto
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public bool EMatriz { get; set; }
        public string CNPJ { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? CNAEPrincipal { get; set; }
        public string? CNAESecundarios { get; set; }
        public string? RegimeTributario { get; set; }

        // Endereço
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string Pais { get; set; } = "Brasil";

        // Contato
        public string? TelefoneFixo { get; set; }
        public string? CelularWhatsApp { get; set; }
        public string? Email { get; set; }
        public string? Site { get; set; }
        public string? NomeResponsavel { get; set; }

        // Dados específicos
        public int? QuantidadeFiliais { get; set; }
        public string? InscricoesAutorizacoes { get; set; }
        public Guid? IdEmpresaMatriz { get; set; }
        public string? CodigoFilial { get; set; }

        // Metadados e Auditoria
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public bool CadastroAtivo { get; set; }
        public Guid IdUsuarioCadastrou { get; set; }
        public Guid? IdUsuarioAlterou { get; set; }
    }
}
