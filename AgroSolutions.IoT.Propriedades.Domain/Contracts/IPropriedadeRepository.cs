using AgroSolutions.IoT.Propriedades.Domain.Entities;

namespace AgroSolutions.IoT.Propriedades.Domain.Contracts;

public interface IPropriedadeRepository
{
    Task<Propriedade?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Propriedade>> ListarPorProdutorIdAsync();
    Task AdicionarAsync(Propriedade propriedade);
    Task AtualizarAsync(Propriedade propriedade);
    Task<bool> ExisteAsync(Guid id);
}
