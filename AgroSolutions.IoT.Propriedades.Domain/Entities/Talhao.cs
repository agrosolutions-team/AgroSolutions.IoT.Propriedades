namespace AgroSolutions.IoT.Propriedades.Domain.Entities;

public class Talhao
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public decimal AreaEmHectares { get; private set; }
    public string CulturaPlantada { get; private set; } = string.Empty;
    public Guid PropriedadeId { get; private set; }
    
    public Propriedade? Propriedade { get; private set; }

    private Talhao() { }

    public static Talhao Criar(string nome, decimal areaEmHectares, string culturaPlantada, Guid propriedadeId)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do talhão é obrigatório", nameof(nome));

        if (areaEmHectares <= 0)
            throw new ArgumentException("Área deve ser maior que zero", nameof(areaEmHectares));

        if (string.IsNullOrWhiteSpace(culturaPlantada))
            throw new ArgumentException("Cultura plantada é obrigatória", nameof(culturaPlantada));

        if (propriedadeId == Guid.Empty)
            throw new ArgumentException("PropriedadeId inválido", nameof(propriedadeId));

        return new Talhao
        {
            Nome = nome,
            AreaEmHectares = areaEmHectares,
            CulturaPlantada = culturaPlantada,
            PropriedadeId = propriedadeId
        };
    }

    public void AtualizarDados(string nome, decimal areaEmHectares, string culturaPlantada)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do talhão é obrigatório", nameof(nome));

        if (areaEmHectares <= 0)
            throw new ArgumentException("Área deve ser maior que zero", nameof(areaEmHectares));

        if (string.IsNullOrWhiteSpace(culturaPlantada))
            throw new ArgumentException("Cultura plantada é obrigatória", nameof(culturaPlantada));

        Nome = nome;
        AreaEmHectares = areaEmHectares;
        CulturaPlantada = culturaPlantada;
    }
}
