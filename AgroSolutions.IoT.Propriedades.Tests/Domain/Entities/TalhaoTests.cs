using AgroSolutions.IoT.Propriedades.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace AgroSolutions.IoT.Propriedades.Tests.Domain.Entities;

public class TalhaoTests
{
    [Fact]
    public void Criar_DeveRetornarTalhaoValido_QuandoDadosForemCorretos()
    {
        // Arrange
        var nome = "Talhão 01";
        var area = 15.5m;
        var cultura = "Milho";
        var propriedadeId = Guid.NewGuid();

        // Act
        var talhao = Talhao.Criar(nome, area, cultura, propriedadeId);

        // Assert
        talhao.Should().NotBeNull();
        talhao.Nome.Should().Be(nome);
        talhao.AreaEmHectares.Should().Be(area);
        talhao.CulturaPlantada.Should().Be(cultura);
        talhao.PropriedadeId.Should().Be(propriedadeId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Criar_DeveLancarException_QuandoNomeForInvalido(string nomeInvalido)
    {
        // Arrange
        var propriedadeId = Guid.NewGuid();

        // Act
        Action act = () => Talhao.Criar(nomeInvalido, 10m, "Soja", propriedadeId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Nome do talhão é obrigatório*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10.5)]
    public void Criar_DeveLancarException_QuandoAreaForMenorOuIgualAZero(decimal areaInvalida)
    {
        // Arrange
        var propriedadeId = Guid.NewGuid();

        // Act
        Action act = () => Talhao.Criar("Talhão 01", areaInvalida, "Soja", propriedadeId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Área deve ser maior que zero*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Criar_DeveLancarException_QuandoCulturaForInvalida(string culturaInvalida)
    {
        // Arrange
        var propriedadeId = Guid.NewGuid();

        // Act
        Action act = () => Talhao.Criar("Talhão 01", 10m, culturaInvalida, propriedadeId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Cultura plantada é obrigatória*");
    }

    [Fact]
    public void Criar_DeveLancarException_QuandoPropriedadeIdForVazio()
    {
        // Act
        Action act = () => Talhao.Criar("Talhão 01", 10m, "Soja", Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("PropriedadeId inválido*");
    }

    [Fact]
    public void AtualizarDados_DeveAtualizarCorretamente_QuandoDadosForemValidos()
    {
        // Arrange
        var talhao = Talhao.Criar("Talhão 01", 10m, "Soja", Guid.NewGuid());
        var novoNome = "Talhão 02";
        var novaArea = 20m;
        var novaCultura = "Milho";

        // Act
        talhao.AtualizarDados(novoNome, novaArea, novaCultura);

        // Assert
        talhao.Nome.Should().Be(novoNome);
        talhao.AreaEmHectares.Should().Be(novaArea);
        talhao.CulturaPlantada.Should().Be(novaCultura);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void AtualizarDados_DeveLancarException_QuandoNomeForInvalido(string nomeInvalido)
    {
        // Arrange
        var talhao = Talhao.Criar("Talhão 01", 10m, "Soja", Guid.NewGuid());

        // Act
        Action act = () => talhao.AtualizarDados(nomeInvalido, 20m, "Milho");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Nome do talhão é obrigatório*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AtualizarDados_DeveLancarException_QuandoAreaForInvalida(decimal areaInvalida)
    {
        // Arrange
        var talhao = Talhao.Criar("Talhão 01", 10m, "Soja", Guid.NewGuid());

        // Act
        Action act = () => talhao.AtualizarDados("Talhão 02", areaInvalida, "Milho");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Área deve ser maior que zero*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void AtualizarDados_DeveLancarException_QuandoCulturaForInvalida(string culturaInvalida)
    {
        // Arrange
        var talhao = Talhao.Criar("Talhão 01", 10m, "Soja", Guid.NewGuid());

        // Act
        Action act = () => talhao.AtualizarDados("Talhão 02", 20m, culturaInvalida);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Cultura plantada é obrigatória*");
    }
}
