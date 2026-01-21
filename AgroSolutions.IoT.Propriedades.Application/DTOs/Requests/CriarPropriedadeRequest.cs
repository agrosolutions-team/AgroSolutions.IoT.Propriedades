namespace AgroSolutions.IoT.Propriedades.Application.DTOs.Requests;

public class CriarPropriedadeRequest
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public List<CriarTalhaoRequest> Talhoes { get; set; } = new();
}
