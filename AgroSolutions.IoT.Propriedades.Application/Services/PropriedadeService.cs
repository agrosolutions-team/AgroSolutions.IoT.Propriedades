using AgroSolutions.IoT.Propriedades.Application.DTOs.Requests;
using AgroSolutions.IoT.Propriedades.Application.DTOs.Responses;
using AgroSolutions.IoT.Propriedades.Domain.Contracts;
using AgroSolutions.IoT.Propriedades.Domain.Entities;

namespace AgroSolutions.IoT.Propriedades.Application.Services;

public class PropriedadeService
{
    private readonly IPropriedadeRepository _propriedadeRepository;

    public PropriedadeService(IPropriedadeRepository propriedadeRepository)
    {
        _propriedadeRepository = propriedadeRepository;
    }

    public async Task<PropriedadeResponse> CriarPropriedadeAsync(CriarPropriedadeRequest request, Guid produtorId)
    {
        if (request.Talhoes == null || !request.Talhoes.Any())
            throw new ArgumentException("A propriedade deve possuir ao menos um talhão");

        var propriedade = Propriedade.Criar(request.Nome, produtorId, request.Descricao);

        foreach (var talhaoRequest in request.Talhoes)
        {
            var talhao = Talhao.Criar(
                talhaoRequest.Nome,
                talhaoRequest.AreaEmHectares,
                talhaoRequest.CulturaPlantada,
                propriedade.Id
            );
            propriedade.AdicionarTalhao(talhao);
        }

        propriedade.ValidarPossuiTalhoes();

        await _propriedadeRepository.AdicionarAsync(propriedade);

        return MapearParaResponse(propriedade);
    }

    public async Task<IEnumerable<PropriedadeResponse>> ListarPropriedadesDoProdutorAsync()
    {
        var propriedades = await _propriedadeRepository.ListarPorProdutorIdAsync();
        return propriedades.Select(MapearParaResponse);
    }

    public async Task<PropriedadeResponse?> ObterPropriedadePorIdAsync(Guid id, Guid produtorId)
    {
        var propriedade = await _propriedadeRepository.ObterPorIdAsync(id);

        if (propriedade == null)
            return null;

        if (propriedade.ProdutorId != produtorId)
            return null;

        return MapearParaResponse(propriedade);
    }

    private static PropriedadeResponse MapearParaResponse(Propriedade propriedade)
    {
        return new PropriedadeResponse
        {
            Id = propriedade.Id,
            Nome = propriedade.Nome,
            Descricao = propriedade.Descricao,
            ProdutorId = propriedade.ProdutorId,
            Talhoes = propriedade.Talhoes.Select(t => new TalhaoResponse
            {
                Id = t.Id,
                Nome = t.Nome,
                AreaEmHectares = t.AreaEmHectares,
                CulturaPlantada = t.CulturaPlantada,
                PropriedadeId = t.PropriedadeId
            }).ToList()
        };
    }
}
