using AgroSolutions.IoT.Propriedades.Application.DTOs.Requests;
using AgroSolutions.IoT.Propriedades.Application.Services;
using AgroSolutions.IoT.Propriedades.Domain.Contracts;
using AgroSolutions.IoT.Propriedades.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace AgroSolutions.IoT.Propriedades.Tests.Application.Services;

public class TalhaoServiceTests
{
    private readonly Mock<ITalhaoRepository> _mockTalhaoRepository;
    private readonly Mock<IPropriedadeRepository> _mockPropriedadeRepository;
    private readonly TalhaoService _service;

    public TalhaoServiceTests()
    {
        _mockTalhaoRepository = new Mock<ITalhaoRepository>();
        _mockPropriedadeRepository = new Mock<IPropriedadeRepository>();
        _service = new TalhaoService(_mockTalhaoRepository.Object, _mockPropriedadeRepository.Object);
    }

    [Fact]
    public async Task AdicionarTalhaoAsync_DeveAdicionarComSucesso_QuandoPropriedadePertencerAoProdutor()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var propriedade = Propriedade.Criar("Fazenda Teste", produtorId);
        var propriedadeId = propriedade.Id;

        var request = new CriarTalhaoRequest
        {
            Nome = "Talhão 01",
            AreaEmHectares = 10m,
            CulturaPlantada = "Soja"
        };

        _mockPropriedadeRepository
            .Setup(r => r.ObterPorIdAsync(propriedadeId))
            .ReturnsAsync(propriedade);

        _mockTalhaoRepository
            .Setup(r => r.AdicionarAsync(It.IsAny<Talhao>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _service.AdicionarTalhaoAsync(propriedadeId, request);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be(request.Nome);
        resultado.AreaEmHectares.Should().Be(request.AreaEmHectares);
        resultado.CulturaPlantada.Should().Be(request.CulturaPlantada);
        resultado.PropriedadeId.Should().Be(propriedadeId);

        _mockTalhaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Talhao>()), Times.Once);
    }

    [Fact]
    public async Task AdicionarTalhaoAsync_DeveLancarException_QuandoPropriedadeNaoExistir()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var propriedadeId = Guid.NewGuid();

        var request = new CriarTalhaoRequest
        {
            Nome = "Talhão 01",
            AreaEmHectares = 10m,
            CulturaPlantada = "Soja"
        };

        _mockPropriedadeRepository
            .Setup(r => r.ObterPorIdAsync(propriedadeId))
            .ReturnsAsync((Propriedade)null);

        // Act
        Func<Task> act = async () => await _service.AdicionarTalhaoAsync(propriedadeId, request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Propriedade não encontrada");

        _mockTalhaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Talhao>()), Times.Never);
    }

    [Fact]
    public async Task ListarTalhoesPorPropriedadeAsync_DeveRetornarTalhoes_QuandoPropriedadePertencerAoProdutor()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var propriedade = Propriedade.Criar("Fazenda Teste", produtorId);
        var propriedadeId = propriedade.Id;

        var talhoes = new List<Talhao>
        {
            Talhao.Criar("Talhão 01", 10m, "Soja", propriedadeId),
            Talhao.Criar("Talhão 02", 15m, "Milho", propriedadeId)
        };

        _mockPropriedadeRepository
            .Setup(r => r.ObterPorIdAsync(propriedadeId))
            .ReturnsAsync(propriedade);

        _mockTalhaoRepository
            .Setup(r => r.ListarPorPropriedadeIdAsync(propriedadeId))
            .ReturnsAsync(talhoes);

        // Act
        var resultado = await _service.ListarTalhoesPorPropriedadeAsync(propriedadeId);

        // Assert
        resultado.Should().HaveCount(2);
        resultado.Select(t => t.Nome).Should().Contain(new[] { "Talhão 01", "Talhão 02" });

        _mockTalhaoRepository.Verify(r => r.ListarPorPropriedadeIdAsync(propriedadeId), Times.Once);
    }

    [Fact]
    public async Task ListarTalhoesPorPropriedadeAsync_DeveLancarException_QuandoPropriedadeNaoExistir()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var propriedadeId = Guid.NewGuid();

        _mockPropriedadeRepository
            .Setup(r => r.ObterPorIdAsync(propriedadeId))
            .ReturnsAsync((Propriedade)null);

        // Act
        Func<Task> act = async () => await _service.ListarTalhoesPorPropriedadeAsync(propriedadeId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Propriedade não encontrada");

        _mockTalhaoRepository.Verify(r => r.ListarPorPropriedadeIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarTalhao_QuandoExistirEProduzidorForDono()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var propriedade = Propriedade.Criar("Fazenda Teste", produtorId);
        var talhao = Talhao.Criar("Talhão 01", 10m, "Soja", propriedade.Id);

        _mockTalhaoRepository
            .Setup(r => r.ObterPorIdAsync(talhao.Id))
            .ReturnsAsync(talhao);

        _mockPropriedadeRepository
            .Setup(r => r.ObterPorIdAsync(propriedade.Id))
            .ReturnsAsync(propriedade);

        // Act
        var resultado = await _service.ObterPorIdAsync(talhao.Id);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(talhao.Id);
        resultado.PropriedadeId.Should().Be(propriedade.Id);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarException_QuandoTalhaoNaoExistir()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var talhaoId = Guid.NewGuid();

        _mockTalhaoRepository
            .Setup(r => r.ObterPorIdAsync(talhaoId))
            .ReturnsAsync((Talhao)null);

        // Act
        Func<Task> act = async () => await _service.ObterPorIdAsync(talhaoId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Talhão não encontrado");
    }
}
