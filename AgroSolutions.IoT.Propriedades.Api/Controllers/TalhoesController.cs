using AgroSolutions.IoT.Propriedades.Application.DTOs.Requests;
using AgroSolutions.IoT.Propriedades.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [AllowAnonymous]
    public async Task<IActionResult> AdicionarTalhao(Guid propriedadeId, [FromBody] CriarTalhaoRequest request)
    {
        try
        {
            var response = await _talhaoService.AdicionarTalhaoAsync(propriedadeId, request);
            return CreatedAtAction(nameof(ListarTalhoes), new { propriedadeId }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao adicionar talhão", details = ex.Message });
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ListarTalhoes(Guid propriedadeId)
    {
        try
        {
            var talhoes = await _talhaoService.ListarTalhoesPorPropriedadeAsync(propriedadeId);
            return Ok(talhoes);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao listar talhões", details = ex.Message });
        }
    }

    [HttpGet("~/api/talhoes/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObterTalhaoPorId(Guid id)
    {
        try
        {
            var talhao = await _talhaoService.ObterPorIdAsync(id);
            return Ok(talhao);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao obter talhão", details = ex.Message });
        }
    }
}
