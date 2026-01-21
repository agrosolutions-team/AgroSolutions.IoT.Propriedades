using AgroSolutions.IoT.Propriedades.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace AgroSolutions.IoT.Propriedades.Tests.Domain.Entities;

public class PropriedadeTests
{
    [Fact]
    public void Criar_DeveRetornarPropriedadeValida_QuandoDadosForemCorretos()
    {
        // Arrange
        var nome = "Fazenda Teste";
        var produtorId = Guid.NewGuid();
        var descricao = "Descrição da fazenda";

        // Act
        var propriedade = Propriedade.Criar(nome, produtorId, descricao);

        // Assert
        propriedade.Should().NotBeNull();
        propriedade.Nome.Should().Be(nome);
        propriedade.ProdutorId.Should().Be(produtorId);
        propriedade.Descricao.Should().Be(descricao);
        propriedade.Talhoes.Should().BeEmpty();
    }

    [Fact]
    public void Criar_DeveRetornarPropriedadeValida_QuandoDescricaoForNula()
    {
        // Arrange
        var nome = "Fazenda Teste";
        var produtorId = Guid.NewGuid();

        // Act
        var propriedade = Propriedade.Criar(nome, produtorId);

        // Assert
        propriedade.Should().NotBeNull();
        propriedade.Nome.Should().Be(nome);
        propriedade.ProdutorId.Should().Be(produtorId);
        propriedade.Descricao.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Criar_DeveLancarException_QuandoNomeForInvalido(string nomeInvalido)
    {
        // Arrange
        var produtorId = Guid.NewGuid();

        // Act
        Action act = () => Propriedade.Criar(nomeInvalido, produtorId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Nome da propriedade é obrigatório*");
    }

    [Fact]
    public void Criar_DeveLancarException_QuandoProdutorIdForVazio()
    {
        // Arrange
        var nome = "Fazenda Teste";
        var produtorId = Guid.Empty;

        // Act
        Action act = () => Propriedade.Criar(nome, produtorId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("ProdutorId inválido*");
    }

    [Fact]
    public void AdicionarTalhao_DeveAdicionarTalhaoCorretamente()
    {
        // Arrange
        var propriedade = Propriedade.Criar("Fazenda Teste", Guid.NewGuid());
        var talhao = Talhao.Criar("Talhão 01", 10.5m, "Soja", propriedade.Id);

        // Act
        propriedade.AdicionarTalhao(talhao);

        // Assert
        propriedade.Talhoes.Should().HaveCount(1);
        propriedade.Talhoes.First().Should().Be(talhao);
    }

    [Fact]
    public void AdicionarTalhao_DeveLancarException_QuandoTalhaoForNulo()
    {
        // Arrange
        var propriedade = Propriedade.Criar("Fazenda Teste", Guid.NewGuid());

        // Act
        Action act = () => propriedade.AdicionarTalhao(null);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ValidarPossuiTalhoes_DeveLancarException_QuandoNaoTiverTalhoes()
    {
        // Arrange
        var propriedade = Propriedade.Criar("Fazenda Teste", Guid.NewGuid());

        // Act
        Action act = () => propriedade.ValidarPossuiTalhoes();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("A propriedade deve possuir ao menos um talhão");
    }

    [Fact]
    public void ValidarPossuiTalhoes_NaoDeveLancarException_QuandoTiverTalhoes()
    {
        // Arrange
        var propriedade = Propriedade.Criar("Fazenda Teste", Guid.NewGuid());
        var talhao = Talhao.Criar("Talhão 01", 10.5m, "Soja", propriedade.Id);
        propriedade.AdicionarTalhao(talhao);

        // Act
        Action act = () => propriedade.ValidarPossuiTalhoes();

        // Assert
        act.Should().NotThrow();
    }
}
