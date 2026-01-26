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

    public async Task<TalhaoResponse> AdicionarTalhaoAsync(Guid propriedadeId, CriarTalhaoRequest request)
    {
        var propriedade = await _propriedadeRepository.ObterPorIdAsync(propriedadeId);

        if (propriedade == null)
            throw new InvalidOperationException("Propriedade não encontrada");

        var talhao = Talhao.Criar(
            request.Nome,
            request.AreaEmHectares,
            request.CulturaPlantada,
            propriedadeId
        );

        await _talhaoRepository.AdicionarAsync(talhao);

        return MapearParaResponse(talhao);
    }

    public async Task<IEnumerable<TalhaoResponse>> ListarTalhoesPorPropriedadeAsync(Guid propriedadeId)
    {
        var propriedade = await _propriedadeRepository.ObterPorIdAsync(propriedadeId);

        if (propriedade == null)
            throw new InvalidOperationException("Propriedade não encontrada");

        var talhoes = await _talhaoRepository.ListarPorPropriedadeIdAsync(propriedadeId);
        return talhoes.Select(MapearParaResponse);
    }

    public async Task<TalhaoResponse> ObterPorIdAsync(Guid id)
    {
        var talhao = await _talhaoRepository.ObterPorIdAsync(id);

        if (talhao == null)
            throw new InvalidOperationException("Talhão não encontrado");

        return MapearParaResponse(talhao);
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
