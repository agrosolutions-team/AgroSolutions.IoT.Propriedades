using AgroSolutions.IoT.Propriedades.Application.DTOs.Requests;
using AgroSolutions.IoT.Propriedades.Application.Services;
using AgroSolutions.IoT.Propriedades.Domain.Contracts;
using AgroSolutions.IoT.Propriedades.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace AgroSolutions.IoT.Propriedades.Tests.Application.Services;

public class PropriedadeServiceTests
{
    private readonly Mock<IPropriedadeRepository> _mockRepository;
    private readonly PropriedadeService _service;

    public PropriedadeServiceTests()
    {
        _mockRepository = new Mock<IPropriedadeRepository>();
        _service = new PropriedadeService(_mockRepository.Object);
    }

    [Fact]
    public async Task CriarPropriedadeAsync_DeveCriarPropriedadeComSucesso_QuandoDadosForemValidos()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var request = new CriarPropriedadeRequest
        {
            Nome = "Fazenda Teste",
            Descricao = "Descrição",
            Talhoes = new List<CriarTalhaoRequest>
            {
                new CriarTalhaoRequest
                {
                    Nome = "Talhão 01",
                    AreaEmHectares = 10m,
                    CulturaPlantada = "Soja"
                }
            }
        };

        _mockRepository
            .Setup(r => r.AdicionarAsync(It.IsAny<Propriedade>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _service.CriarPropriedadeAsync(request, produtorId);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be(request.Nome);
        resultado.Descricao.Should().Be(request.Descricao);
        resultado.ProdutorId.Should().Be(produtorId);
        resultado.Talhoes.Should().HaveCount(1);
        resultado.Talhoes.First().Nome.Should().Be("Talhão 01");

        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Propriedade>()), Times.Once);
    }

    [Fact]
    public async Task CriarPropriedadeAsync_DeveLancarException_QuandoNaoTiverTalhoes()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var request = new CriarPropriedadeRequest
        {
            Nome = "Fazenda Teste",
            Descricao = "Descrição",
            Talhoes = new List<CriarTalhaoRequest>()
        };

        // Act
        Func<Task> act = async () => await _service.CriarPropriedadeAsync(request, produtorId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("A propriedade deve possuir ao menos um talhão");

        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Propriedade>()), Times.Never);
    }

    [Fact]
    public async Task CriarPropriedadeAsync_DeveLancarException_QuandoTalhoesForNull()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var request = new CriarPropriedadeRequest
        {
            Nome = "Fazenda Teste",
            Descricao = "Descrição",
            Talhoes = null
        };

        // Act
        Func<Task> act = async () => await _service.CriarPropriedadeAsync(request, produtorId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("A propriedade deve possuir ao menos um talhão");

        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Propriedade>()), Times.Never);
    }

    [Fact]
    public async Task ListarPropriedadesDoProdutorAsync_DeveRetornarListaDePropriedades()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var propriedades = new List<Propriedade>
        {
            Propriedade.Criar("Fazenda 1", produtorId),
            Propriedade.Criar("Fazenda 2", produtorId)
        };

        _mockRepository
            .Setup(r => r.ListarPorProdutorIdAsync())
            .ReturnsAsync(propriedades);

        // Act
        var resultado = await _service.ListarPropriedadesDoProdutorAsync();

        // Assert
        resultado.Should().HaveCount(2);
        resultado.Select(p => p.Nome).Should().Contain(new[] { "Fazenda 1", "Fazenda 2" });

        _mockRepository.Verify(r => r.ListarPorProdutorIdAsync(), Times.Once);
    }

    [Fact]
    public async Task ListarPropriedadesDoProdutorAsync_DeveRetornarListaVazia_QuandoNaoTiverPropriedades()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        _mockRepository
            .Setup(r => r.ListarPorProdutorIdAsync())
            .ReturnsAsync(new List<Propriedade>());

        // Act
        var resultado = await _service.ListarPropriedadesDoProdutorAsync();

        // Assert
        resultado.Should().BeEmpty();

        _mockRepository.Verify(r => r.ListarPorProdutorIdAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterPropriedadePorIdAsync_DeveRetornarPropriedade_QuandoExistirEPertencerAoProdutor()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var propriedade = Propriedade.Criar("Fazenda Teste", produtorId);
        var propriedadeId = propriedade.Id;

        _mockRepository
            .Setup(r => r.ObterPorIdAsync(propriedadeId))
            .ReturnsAsync(propriedade);

        // Act
        var resultado = await _service.ObterPropriedadePorIdAsync(propriedadeId, produtorId);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be("Fazenda Teste");
        resultado.ProdutorId.Should().Be(produtorId);

        _mockRepository.Verify(r => r.ObterPorIdAsync(propriedadeId), Times.Once);
    }

    [Fact]
    public async Task ObterPropriedadePorIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var propriedadeId = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.ObterPorIdAsync(propriedadeId))
            .ReturnsAsync((Propriedade)null);

        // Act
        var resultado = await _service.ObterPropriedadePorIdAsync(propriedadeId, produtorId);

        // Assert
        resultado.Should().BeNull();

        _mockRepository.Verify(r => r.ObterPorIdAsync(propriedadeId), Times.Once);
    }

    [Fact]
    public async Task ObterPropriedadePorIdAsync_DeveRetornarNull_QuandoPropriedadeNaoPertencerAoProdutor()
    {
        // Arrange
        var produtorId = Guid.NewGuid();
        var outroProdutorId = Guid.NewGuid();
        var propriedade = Propriedade.Criar("Fazenda Teste", outroProdutorId);
        var propriedadeId = propriedade.Id;

        _mockRepository
            .Setup(r => r.ObterPorIdAsync(propriedadeId))
            .ReturnsAsync(propriedade);

        // Act
        var resultado = await _service.ObterPropriedadePorIdAsync(propriedadeId, produtorId);

        // Assert
        resultado.Should().BeNull();

        _mockRepository.Verify(r => r.ObterPorIdAsync(propriedadeId), Times.Once);
    }
}
