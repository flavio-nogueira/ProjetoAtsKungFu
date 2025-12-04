using ApiAtsKungFu.Domain.Entities;
using ApiAtsKungFu.Domain.Interfaces;
using ApiAtsKungFu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiAtsKungFu.Infrastructure.Repositories
{
    public class EscolaKungFuRepository : IEscolaKungFuRepository
    {
        private readonly AppDbContext _context;

        public EscolaKungFuRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<EscolaKungFu?> ObterPorIdAsync(Guid id)
        {
            return await _context.EscolasKungFu
                .Include(e => e.Matriz)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<EscolaKungFu?> ObterPorCNPJAsync(string cnpj)
        {
            return await _context.EscolasKungFu
                .Include(e => e.Matriz)
                .FirstOrDefaultAsync(e => e.CNPJ == cnpj && e.Ativo);
        }

        public async Task<IEnumerable<EscolaKungFu>> ObterTodosAsync()
        {
            return await _context.EscolasKungFu
                .Include(e => e.Matriz)
                .Where(e => e.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<EscolaKungFu>> ObterMatrizesAsync()
        {
            return await _context.EscolasKungFu
                .Where(e => e.EMatriz && e.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<EscolaKungFu>> ObterFiliaisPorMatrizIdAsync(Guid matrizId)
        {
            return await _context.EscolasKungFu
                .Where(e => e.IdEmpresaMatriz == matrizId && e.Ativo)
                .ToListAsync();
        }

        public async Task<bool> CNPJExisteAsync(string cnpj)
        {
            return await _context.EscolasKungFu
                .AnyAsync(e => e.CNPJ == cnpj);
        }

        public async Task<bool> CNPJExisteExcluindoIdAsync(string cnpj, Guid excludeId)
        {
            return await _context.EscolasKungFu
                .AnyAsync(e => e.CNPJ == cnpj && e.Id != excludeId);
        }

        public async Task IncluirAsync(EscolaKungFu escola)
        {
            await _context.EscolasKungFu.AddAsync(escola);
            await _context.SaveChangesAsync();
        }

        public async Task SalvarAsync(EscolaKungFu escola)
        {
            _context.EscolasKungFu.Update(escola);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(Guid id)
        {
            return await _context.EscolasKungFu.AnyAsync(e => e.Id == id);
        }
    }
}
