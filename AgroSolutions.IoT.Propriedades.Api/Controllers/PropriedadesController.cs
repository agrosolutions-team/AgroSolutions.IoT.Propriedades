using AgroSolutions.IoT.Propriedades.Application.DTOs.Requests;
using AgroSolutions.IoT.Propriedades.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgroSolutions.IoT.Propriedades.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PropriedadesController : ControllerBase
{
    private readonly PropriedadeService _propriedadeService;

    public PropriedadesController(PropriedadeService propriedadeService)
    {
        _propriedadeService = propriedadeService;
    }

    [HttpPost]
    public async Task<IActionResult> CriarPropriedade([FromBody] CriarPropriedadeRequest request)
    {
        try
        {
            var produtorId = ObterProdutorId();
            var response = await _propriedadeService.CriarPropriedadeAsync(request, produtorId);
            return CreatedAtAction(nameof(ObterPropriedadePorId), new { id = response.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao criar propriedade", details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPropriedades()
    {
        try
        {
            var produtorId = ObterProdutorId();
            var propriedades = await _propriedadeService.ListarPropriedadesDoProdutorAsync(produtorId);
            return Ok(propriedades);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao listar propriedades", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPropriedadePorId(Guid id)
    {
        try
        {
            var produtorId = ObterProdutorId();
            var propriedade = await _propriedadeService.ObterPropriedadePorIdAsync(id, produtorId);

            if (propriedade == null)
                return NotFound(new { message = "Propriedade não encontrada" });

            return Ok(propriedade);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao obter propriedade", details = ex.Message });
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
