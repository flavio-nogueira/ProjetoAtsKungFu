using ApiAtsKungFu.Domain.Entities;

namespace ApiAtsKungFu.Domain.Interfaces
{
    public interface IEscolaKungFuRepository
    {
        Task<EscolaKungFu?> ObterPorIdAsync(Guid id);
        Task<EscolaKungFu?> ObterPorCNPJAsync(string cnpj);
        Task<IEnumerable<EscolaKungFu>> ObterTodosAsync();
        Task<IEnumerable<EscolaKungFu>> ObterMatrizesAsync();
        Task<IEnumerable<EscolaKungFu>> ObterFiliaisPorMatrizIdAsync(Guid matrizId);
        Task<bool> CNPJExisteAsync(string cnpj);
        Task<bool> CNPJExisteExcluindoIdAsync(string cnpj, Guid excludeId);
        Task IncluirAsync(EscolaKungFu escola);
        Task SalvarAsync(EscolaKungFu escola);
        Task<bool> ExisteAsync(Guid id);
    }
}
