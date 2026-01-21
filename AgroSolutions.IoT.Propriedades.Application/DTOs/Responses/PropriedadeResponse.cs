namespace AgroSolutions.IoT.Propriedades.Application.DTOs.Responses;

public class PropriedadeResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public Guid ProdutorId { get; set; }
    public List<TalhaoResponse> Talhoes { get; set; } = new();
}
