namespace AgroSolutions.IoT.Propriedades.Domain.Entities;

public class Propriedade
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public Guid ProdutorId { get; private set; }
    
    private readonly List<Talhao> _talhoes = new();
    public IReadOnlyCollection<Talhao> Talhoes => _talhoes.AsReadOnly();

    private Propriedade() { }

    public static Propriedade Criar(string nome, Guid produtorId, string? descricao = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da propriedade é obrigatório", nameof(nome));

        if (produtorId == Guid.Empty)
            throw new ArgumentException("ProdutorId inválido", nameof(produtorId));

        return new Propriedade
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Descricao = descricao,
            ProdutorId = produtorId
        };
    }

    public void AdicionarTalhao(Talhao talhao)
    {
        if (talhao == null)
            throw new ArgumentNullException(nameof(talhao));

        _talhoes.Add(talhao);
    }

    public void ValidarPossuiTalhoes()
    {
        if (!_talhoes.Any())
            throw new InvalidOperationException("A propriedade deve possuir ao menos um talhão");
    }
}
