using ApiAtsKungFu.Application.DTOs;

namespace ApiAtsKungFu.Application.Interfaces
{
    public interface IEscolaKungFuService
    {
        Task<EscolaKungFuDto?> ObterPorIdAsync(Guid id);
        Task<EscolaKungFuDto?> ObterPorCNPJAsync(string cnpj);
        Task<IEnumerable<EscolaKungFuDto>> ObterTodosAsync();
        Task<IEnumerable<EscolaKungFuDto>> ObterMatrizesAsync();
        Task<IEnumerable<EscolaKungFuDto>> ObterFiliaisPorMatrizIdAsync(Guid matrizId);
        Task<EscolaKungFuDto> IncluirAsync(CreateEscolaKungFuDto createDto);
        Task<EscolaKungFuDto> AlterarAsync(Guid id, UpdateEscolaKungFuDto updateDto);
        Task<bool> ExcluirAsync(Guid id);
    }
}
