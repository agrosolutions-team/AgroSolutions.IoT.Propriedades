using AgroSolutions.IoT.Propriedades.Domain.Contracts;
using AgroSolutions.IoT.Propriedades.Domain.Entities;
using AgroSolutions.IoT.Propriedades.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.IoT.Propriedades.Infrastructure.Repositories;

public class PropriedadeRepository : IPropriedadeRepository
{
    private readonly PropriedadesDbContext _context;

    public PropriedadeRepository(PropriedadesDbContext context)
    {
        _context = context;
    }

    public async Task<Propriedade?> ObterPorIdAsync(Guid id)
    {
        return await _context.Propriedades
            .Include(p => p.Talhoes)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Propriedade>> ListarPorProdutorIdAsync()
    {
        return await _context.Propriedades
            .Include(p => p.Talhoes)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Propriedade propriedade)
    {
        await _context.Propriedades.AddAsync(propriedade);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Propriedade propriedade)
    {
        _context.Propriedades.Update(propriedade);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteAsync(Guid id)
    {
        return await _context.Propriedades.AnyAsync(p => p.Id == id);
    }
}
