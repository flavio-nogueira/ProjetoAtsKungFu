using ApiAtsKungFu.Application.DTOs;
using ApiAtsKungFu.Application.Interfaces;
using ApiAtsKungFu.Domain.Entities;
using ApiAtsKungFu.Domain.Interfaces;
using AutoMapper;

namespace ApiAtsKungFu.Application.Services
{
    public class EscolaKungFuService : IEscolaKungFuService
    {
        private readonly IEscolaKungFuRepository _repository;
        private readonly IMapper _mapper;

        public EscolaKungFuService(IEscolaKungFuRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<EscolaKungFuDto?> ObterPorIdAsync(Guid id)
        {
            var escola = await _repository.ObterPorIdAsync(id);
            return escola == null ? null : _mapper.Map<EscolaKungFuDto>(escola);
        }

        public async Task<EscolaKungFuDto?> ObterPorCNPJAsync(string cnpj)
        {
            var escola = await _repository.ObterPorCNPJAsync(cnpj);
            return escola == null ? null : _mapper.Map<EscolaKungFuDto>(escola);
        }

        public async Task<IEnumerable<EscolaKungFuDto>> ObterTodosAsync()
        {
            var escolas = await _repository.ObterTodosAsync();
            return _mapper.Map<IEnumerable<EscolaKungFuDto>>(escolas);
        }

        public async Task<IEnumerable<EscolaKungFuDto>> ObterMatrizesAsync()
        {
            var matrizes = await _repository.ObterMatrizesAsync();
            return _mapper.Map<IEnumerable<EscolaKungFuDto>>(matrizes);
        }

        public async Task<IEnumerable<EscolaKungFuDto>> ObterFiliaisPorMatrizIdAsync(Guid matrizId)
        {
            var filiais = await _repository.ObterFiliaisPorMatrizIdAsync(matrizId);
            return _mapper.Map<IEnumerable<EscolaKungFuDto>>(filiais);
        }

        public async Task<EscolaKungFuDto> IncluirAsync(CreateEscolaKungFuDto createDto)
        {
            // Validar se CNPJ já existe
            if (await _repository.CNPJExisteAsync(createDto.CNPJ))
            {
                throw new InvalidOperationException("CNPJ já cadastrado");
            }

            // Criar entidade usando factory method
            EscolaKungFu escola;

            if (createDto.Tipo.Equals("Matriz", StringComparison.OrdinalIgnoreCase))
            {
                escola = EscolaKungFu.CriarMatriz(
                    createDto.CNPJ,
                    createDto.RazaoSocial,
                    createDto.NomeFantasia,
                    createDto.Logradouro,
                    createDto.Numero,
                    createDto.Bairro,
                    createDto.Cidade,
                    createDto.UF,
                    createDto.CEP,
                    createDto.IdUsuarioCadastrou
                );
            }
            else if (createDto.Tipo.Equals("Filial", StringComparison.OrdinalIgnoreCase))
            {
                if (!createDto.IdEmpresaMatriz.HasValue)
                {
                    throw new InvalidOperationException("Filial deve ter uma matriz associada");
                }

                // Validar se matriz existe
                if (!await _repository.ExisteAsync(createDto.IdEmpresaMatriz.Value))
                {
                    throw new InvalidOperationException("Matriz não encontrada");
                }

                escola = EscolaKungFu.CriarFilial(
                    createDto.CNPJ,
                    createDto.RazaoSocial,
                    createDto.NomeFantasia,
                    createDto.IdEmpresaMatriz.Value,
                    createDto.Logradouro,
                    createDto.Numero,
                    createDto.Bairro,
                    createDto.Cidade,
                    createDto.UF,
                    createDto.CEP,
                    createDto.IdUsuarioCadastrou
                );
            }
            else
            {
                throw new InvalidOperationException("Tipo deve ser 'Matriz' ou 'Filial'");
            }

            // Atualizar dados opcionais
            escola.AtualizarDadosBasicos(
                createDto.RazaoSocial,
                createDto.NomeFantasia,
                createDto.InscricaoEstadual,
                createDto.InscricaoMunicipal,
                createDto.CNAEPrincipal,
                createDto.RegimeTributario,
                createDto.IdUsuarioCadastrou
            );

            escola.AtualizarContato(
                createDto.TelefoneFixo,
                createDto.CelularWhatsApp,
                createDto.Email,
                createDto.Site,
                createDto.NomeResponsavel,
                createDto.IdUsuarioCadastrou
            );

            await _repository.IncluirAsync(escola);

            return _mapper.Map<EscolaKungFuDto>(escola);
        }

        public async Task<EscolaKungFuDto> AlterarAsync(Guid id, UpdateEscolaKungFuDto updateDto)
        {
            var escola = await _repository.ObterPorIdAsync(id);

            if (escola == null)
            {
                throw new InvalidOperationException("Escola não encontrada");
            }

            // Atualizar dados
            escola.AtualizarDadosBasicos(
                updateDto.RazaoSocial,
                updateDto.NomeFantasia,
                updateDto.InscricaoEstadual,
                updateDto.InscricaoMunicipal,
                updateDto.CNAEPrincipal,
                updateDto.RegimeTributario,
                updateDto.IdUsuarioAlterou
            );

            escola.AtualizarEndereco(
                updateDto.Logradouro,
                updateDto.Numero,
                updateDto.Complemento,
                updateDto.Bairro,
                updateDto.Cidade,
                updateDto.UF,
                updateDto.CEP,
                updateDto.IdUsuarioAlterou,
                updateDto.Pais
            );

            escola.AtualizarContato(
                updateDto.TelefoneFixo,
                updateDto.CelularWhatsApp,
                updateDto.Email,
                updateDto.Site,
                updateDto.NomeResponsavel,
                updateDto.IdUsuarioAlterou
            );

            await _repository.SalvarAsync(escola);

            return _mapper.Map<EscolaKungFuDto>(escola);
        }

        public async Task<bool> ExcluirAsync(Guid id)
        {
            var escola = await _repository.ObterPorIdAsync(id);

            if (escola == null)
            {
                return false;
            }

            escola.Desativar(Guid.Empty); // TODO: Obter ID do usuário do contexto
            await _repository.SalvarAsync(escola);

            return true;
        }
    }
}
