using AgroSolutions.IoT.Propriedades.Domain.Entities;

namespace AgroSolutions.IoT.Propriedades.Domain.Contracts;

public interface ITalhaoRepository
{
    Task<Talhao?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Talhao>> ListarPorPropriedadeIdAsync(Guid propriedadeId);
    Task AdicionarAsync(Talhao talhao);
    Task AtualizarAsync(Talhao talhao);
}
