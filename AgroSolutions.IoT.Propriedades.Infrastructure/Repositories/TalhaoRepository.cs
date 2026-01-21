using AgroSolutions.IoT.Propriedades.Domain.Contracts;
using AgroSolutions.IoT.Propriedades.Domain.Entities;
using AgroSolutions.IoT.Propriedades.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.IoT.Propriedades.Infrastructure.Repositories;

public class TalhaoRepository : ITalhaoRepository
{
    private readonly PropriedadesDbContext _context;

    public TalhaoRepository(PropriedadesDbContext context)
    {
        _context = context;
    }

    public async Task<Talhao?> ObterPorIdAsync(Guid id)
    {
        return await _context.Talhoes.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Talhao>> ListarPorPropriedadeIdAsync(Guid propriedadeId)
    {
        return await _context.Talhoes
            .Where(t => t.PropriedadeId == propriedadeId)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Talhao talhao)
    {
        await _context.Talhoes.AddAsync(talhao);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Talhao talhao)
    {
        _context.Talhoes.Update(talhao);
        await _context.SaveChangesAsync();
    }
}
