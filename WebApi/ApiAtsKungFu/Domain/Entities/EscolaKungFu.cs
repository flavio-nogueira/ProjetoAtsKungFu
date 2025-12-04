namespace ApiAtsKungFu.Domain.Entities
{
    public class EscolaKungFu
    {
        public Guid Id { get; private set; }

        // ========== Dados Básicos ==========
        public string Tipo { get; private set; } = string.Empty;
        public bool EMatriz { get; private set; }
        public string CNPJ { get; private set; } = string.Empty;
        public string RazaoSocial { get; private set; } = string.Empty;
        public string? NomeFantasia { get; private set; }
        public string? InscricaoEstadual { get; private set; }
        public string? InscricaoMunicipal { get; private set; }
        public string? CNAEPrincipal { get; private set; }
        public string? CNAESecundarios { get; private set; }
        public string? RegimeTributario { get; private set; }

        // ========== Endereço ==========
        public string Logradouro { get; private set; } = string.Empty;
        public string Numero { get; private set; } = string.Empty;
        public string? Complemento { get; private set; }
        public string Bairro { get; private set; } = string.Empty;
        public string Cidade { get; private set; } = string.Empty;
        public string UF { get; private set; } = string.Empty;
        public string CEP { get; private set; } = string.Empty;
        public string Pais { get; private set; } = "Brasil";

        // ========== Contato ==========
        public string? TelefoneFixo { get; private set; }
        public string? CelularWhatsApp { get; private set; }
        public string? Email { get; private set; }
        public string? Site { get; private set; }
        public string? NomeResponsavel { get; private set; }

        // ========== Dados Específicos de Matriz ==========
        public int? QuantidadeFiliais { get; private set; }
        public string? InscricoesAutorizacoes { get; private set; }

        // ========== Dados Específicos de Filial ==========
        public Guid? IdEmpresaMatriz { get; private set; }
        public EscolaKungFu? Matriz { get; private set; }
        public string? CodigoFilial { get; private set; }

        // ========== Metadados e Auditoria ==========
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAlteracao { get; private set; }
        public bool Ativo { get; private set; }
        public bool CadastroAtivo { get; private set; }
        public Guid IdUsuarioCadastrou { get; private set; }
        public Guid? IdUsuarioAlterou { get; private set; }

        // Construtor privado para EF Core
        private EscolaKungFu() { }

        // Factory Method para criar Matriz
        public static EscolaKungFu CriarMatriz(
            string cnpj,
            string razaoSocial,
            string? nomeFantasia,
            string logradouro,
            string numero,
            string bairro,
            string cidade,
            string uf,
            string cep,
            Guid idUsuarioCadastrou)
        {
            var escola = new EscolaKungFu
            {
                Id = Guid.NewGuid(),
                Tipo = "Matriz",
                EMatriz = true,
                CNPJ = cnpj,
                RazaoSocial = razaoSocial,
                NomeFantasia = nomeFantasia,
                Logradouro = logradouro,
                Numero = numero,
                Bairro = bairro,
                Cidade = cidade,
                UF = uf,
                CEP = cep,
                DataCriacao = DateTime.Now,
                Ativo = true,
                CadastroAtivo = true,
                IdUsuarioCadastrou = idUsuarioCadastrou
            };

            escola.Validar();
            return escola;
        }

        // Factory Method para criar Filial
        public static EscolaKungFu CriarFilial(
            string cnpj,
            string razaoSocial,
            string? nomeFantasia,
            Guid idMatriz,
            string logradouro,
            string numero,
            string bairro,
            string cidade,
            string uf,
            string cep,
            Guid idUsuarioCadastrou)
        {
            var escola = new EscolaKungFu
            {
                Id = Guid.NewGuid(),
                Tipo = "Filial",
                EMatriz = false,
                CNPJ = cnpj,
                RazaoSocial = razaoSocial,
                NomeFantasia = nomeFantasia,
                IdEmpresaMatriz = idMatriz,
                Logradouro = logradouro,
                Numero = numero,
                Bairro = bairro,
                Cidade = cidade,
                UF = uf,
                CEP = cep,
                DataCriacao = DateTime.Now,
                Ativo = true,
                CadastroAtivo = true,
                IdUsuarioCadastrou = idUsuarioCadastrou
            };

            escola.Validar();
            return escola;
        }

        // Métodos de atualização
        public void AtualizarDadosBasicos(
            string razaoSocial,
            string? nomeFantasia,
            string? inscricaoEstadual,
            string? inscricaoMunicipal,
            string? cnae,
            string? regimeTributario,
            Guid idUsuarioAlterou)
        {
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            InscricaoEstadual = inscricaoEstadual;
            InscricaoMunicipal = inscricaoMunicipal;
            CNAEPrincipal = cnae;
            RegimeTributario = regimeTributario;
            DataAlteracao = DateTime.Now;
            IdUsuarioAlterou = idUsuarioAlterou;
            Validar();
        }

        public void AtualizarEndereco(
            string logradouro,
            string numero,
            string? complemento,
            string bairro,
            string cidade,
            string uf,
            string cep,
            Guid idUsuarioAlterou,
            string pais = "Brasil")
        {
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            UF = uf;
            CEP = cep;
            Pais = pais;
            DataAlteracao = DateTime.Now;
            IdUsuarioAlterou = idUsuarioAlterou;
            Validar();
        }

        public void AtualizarContato(
            string? telefoneFixo,
            string? celularWhatsApp,
            string? email,
            string? site,
            string? nomeResponsavel,
            Guid idUsuarioAlterou)
        {
            TelefoneFixo = telefoneFixo;
            CelularWhatsApp = celularWhatsApp;
            Email = email;
            Site = site;
            NomeResponsavel = nomeResponsavel;
            DataAlteracao = DateTime.Now;
            IdUsuarioAlterou = idUsuarioAlterou;
        }

        public void Desativar(Guid idUsuarioAlterou)
        {
            Ativo = false;
            DataAlteracao = DateTime.Now;
            IdUsuarioAlterou = idUsuarioAlterou;
        }

        public void Ativar(Guid idUsuarioAlterou)
        {
            Ativo = true;
            DataAlteracao = DateTime.Now;
            IdUsuarioAlterou = idUsuarioAlterou;
        }

        public void DesativarCadastro(Guid idUsuarioAlterou)
        {
            CadastroAtivo = false;
            DataAlteracao = DateTime.Now;
            IdUsuarioAlterou = idUsuarioAlterou;
        }

        public void AtivarCadastro(Guid idUsuarioAlterou)
        {
            CadastroAtivo = true;
            DataAlteracao = DateTime.Now;
            IdUsuarioAlterou = idUsuarioAlterou;
        }

        // Validação de domínio
        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(CNPJ))
                throw new ArgumentException("CNPJ é obrigatório");

            if (string.IsNullOrWhiteSpace(RazaoSocial))
                throw new ArgumentException("Razão Social é obrigatória");

            if (string.IsNullOrWhiteSpace(Logradouro))
                throw new ArgumentException("Logradouro é obrigatório");

            if (string.IsNullOrWhiteSpace(Cidade))
                throw new ArgumentException("Cidade é obrigatória");

            if (string.IsNullOrWhiteSpace(UF) || UF.Length != 2)
                throw new ArgumentException("UF deve ter 2 caracteres");

            if (!EMatriz && IdEmpresaMatriz == null)
                throw new ArgumentException("Filial deve ter uma matriz associada");
        }
    }
}
