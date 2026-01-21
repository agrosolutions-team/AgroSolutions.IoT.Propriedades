namespace AgroSolutions.IoT.Propriedades.Application.DTOs.Responses;

public class TalhaoResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal AreaEmHectares { get; set; }
    public string CulturaPlantada { get; set; } = string.Empty;
    public Guid PropriedadeId { get; set; }
}
