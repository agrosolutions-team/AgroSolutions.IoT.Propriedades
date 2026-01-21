using AgroSolutions.IoT.Propriedades.Application.DTOs.Requests;
using AgroSolutions.IoT.Propriedades.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgroSolutions.IoT.Propriedades.Api.Controllers;

[ApiController]
[Route("api/propriedades/{propriedadeId}/[controller]")]
[Authorize]
public class TalhoesController : ControllerBase
{
    private readonly TalhaoService _talhaoService;

    public TalhoesController(TalhaoService talhaoService)
    {
        _talhaoService = talhaoService;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarTalhao(Guid propriedadeId, [FromBody] CriarTalhaoRequest request)
    {
        try
        {
            var produtorId = ObterProdutorId();
            var response = await _talhaoService.AdicionarTalhaoAsync(propriedadeId, request, produtorId);
            return CreatedAtAction(nameof(ListarTalhoes), new { propriedadeId }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao adicionar talhão", details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarTalhoes(Guid propriedadeId)
    {
        try
        {
            var produtorId = ObterProdutorId();
            var talhoes = await _talhaoService.ListarTalhoesPorPropriedadeAsync(propriedadeId, produtorId);
            return Ok(talhoes);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao listar talhões", details = ex.Message });
        }
    }

    private Guid ObterProdutorId()
    {
        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                    ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(subClaim) || !Guid.TryParse(subClaim, out var produtorId))
            throw new UnauthorizedAccessException("ProdutorId não encontrado no token");

        return produtorId;
    }
}
