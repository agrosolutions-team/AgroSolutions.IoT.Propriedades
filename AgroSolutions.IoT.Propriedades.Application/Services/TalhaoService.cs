using AgroSolutions.IoT.Propriedades.Application.DTOs.Requests;
using AgroSolutions.IoT.Propriedades.Application.DTOs.Responses;
using AgroSolutions.IoT.Propriedades.Domain.Contracts;
using AgroSolutions.IoT.Propriedades.Domain.Entities;

namespace AgroSolutions.IoT.Propriedades.Application.Services;

public class TalhaoService
{
    private readonly ITalhaoRepository _talhaoRepository;
    private readonly IPropriedadeRepository _propriedadeRepository;

    public TalhaoService(ITalhaoRepository talhaoRepository, IPropriedadeRepository propriedadeRepository)
    {
        _talhaoRepository = talhaoRepository;
        _propriedadeRepository = propriedadeRepository;
    }

    public async Task<TalhaoResponse> AdicionarTalhaoAsync(Guid propriedadeId, CriarTalhaoRequest request, Guid produtorId)
    {
        var propriedade = await _propriedadeRepository.ObterPorIdAsync(propriedadeId);

        if (propriedade == null)
            throw new InvalidOperationException("Propriedade não encontrada");

        if (propriedade.ProdutorId != produtorId)
            throw new UnauthorizedAccessException("Propriedade não pertence ao produtor");

        var talhao = Talhao.Criar(
            request.Nome,
            request.AreaEmHectares,
            request.CulturaPlantada,
            propriedadeId
        );

        await _talhaoRepository.AdicionarAsync(talhao);

        return MapearParaResponse(talhao);
    }

    public async Task<IEnumerable<TalhaoResponse>> ListarTalhoesPorPropriedadeAsync(Guid propriedadeId, Guid produtorId)
    {
        var propriedade = await _propriedadeRepository.ObterPorIdAsync(propriedadeId);

        if (propriedade == null)
            throw new InvalidOperationException("Propriedade não encontrada");

        if (propriedade.ProdutorId != produtorId)
            throw new UnauthorizedAccessException("Propriedade não pertence ao produtor");

        var talhoes = await _talhaoRepository.ListarPorPropriedadeIdAsync(propriedadeId);
        return talhoes.Select(MapearParaResponse);
    }

    private static TalhaoResponse MapearParaResponse(Talhao talhao)
    {
        return new TalhaoResponse
        {
            Id = talhao.Id,
            Nome = talhao.Nome,
            AreaEmHectares = talhao.AreaEmHectares,
            CulturaPlantada = talhao.CulturaPlantada,
            PropriedadeId = talhao.PropriedadeId
        };
    }
}
