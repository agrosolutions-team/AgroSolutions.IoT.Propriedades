namespace AgroSolutions.IoT.Propriedades.Application.DTOs.Requests;

public class AtualizarTalhaoRequest
{
    public string Nome { get; set; } = string.Empty;
    public decimal AreaEmHectares { get; set; }
    public string CulturaPlantada { get; set; } = string.Empty;
}
