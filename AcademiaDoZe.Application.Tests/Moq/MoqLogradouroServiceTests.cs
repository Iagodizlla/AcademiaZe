//Iago Henrique Schlemper
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using Moq;

namespace AcademiaDoZe.Application.Tests;

public class MoqLogradouroServiceTests
{
    private readonly Mock<ILogradouroService> _logradouroServiceMock;
    private readonly ILogradouroService _logradouroService;
    public MoqLogradouroServiceTests()
    {
        // para testar um serviço, em vez de se conectar ao banco de dados real, vamos injetar um mock do repositório.
        // isso permite que você teste a lógica de negócio do seu serviço sem depender do banco de dados.

        _logradouroServiceMock = new Mock<ILogradouroService>();
        _logradouroService = _logradouroServiceMock.Object;
    }
    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarLogradouro_QuandoExistir()
    {
        // Arrange

        var logradouroId = 1;
        var logradouroDto = new LogradouroDTO { Id = logradouroId, Cep = "12345678", Nome = "Rua Teste", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" };

        // configura o objeto mock do serviço (_logradouroServiceMock).

        // essa configuração diz ao mock para retornar o logradouroDto sempre que o método ObterPorIdAsync for chamado com o logradouroId definido.
        // isso simula o comportamento de uma busca bem sucedida, sem a necessidade de acessar um banco de dados real.

        _logradouroServiceMock.Setup(s => s.ObterPorIdAsync(logradouroId)).ReturnsAsync(logradouroDto);

        // Act
        // o método que está sendo testado é executado.
        // chama o serviço mock, que, por sua vez, retorna o objeto configurado na etapa de Arrange.
        var result = await _logradouroService.ObterPorIdAsync(logradouroId);

        // Assert

        Assert.NotNull(result);
        Assert.Equal(logradouroDto.Cep, result.Cep);
        // confirma que o método ObterPorIdAsync no mock foi chamado exatamente uma vez com o logradouroId correto.
        // essa verificação é crucial para garantir que o comportamento do seu código está de acordo com o esperado,
        // ou seja, que a chamada ao serviço realmente aconteceu como deveria.
        _logradouroServiceMock.Verify(s => s.ObterPorIdAsync(logradouroId), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornarLogradouros_QuandoExistirem()
    {
        // Arrange

        var logradourosDto = new List<LogradouroDTO>

{
new LogradouroDTO {Id = 1, Cep = "12345678", Nome = "Rua Teste 1", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" },
new LogradouroDTO {Id = 2, Cep = "87654321", Nome = "Rua Teste 2", Bairro = "Centro", Cidade = "Rio de Janeiro", Estado = "RJ", Pais = "Brasil"}
};
        _logradouroServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(logradourosDto);
        // Act

        var result = await _logradouroService.ObterTodosAsync();

        // Assert

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _logradouroServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaVazia_QuandoNaoHouverLogradouros()
    {
        // Arrange
        _logradouroServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(new List<LogradouroDTO>());
        // Act

        var result = await _logradouroService.ObterTodosAsync();

        // Assert

        Assert.NotNull(result);
        Assert.Empty(result);

        _logradouroServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }
    [Fact]
    public async Task ObterPorCepAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        // Arrange

        var cep = "00000000";

        _logradouroServiceMock.Setup(s => s.ObterPorCepAsync(cep)).ReturnsAsync((LogradouroDTO)null!);
        // Act

        var result = await _logradouroService.ObterPorCepAsync(cep);

        // Assert

        Assert.Null(result);

        _logradouroServiceMock.Verify(s => s.ObterPorCepAsync(cep), Times.Once);
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarLogradouro_QuandoDadosValidos()
    {
        // Arrange
        var logradouroDto = new LogradouroDTO { Cep = "12345678", Nome = "Nova Rua", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" };
        var logradouroCriado = new LogradouroDTO
        {
            Id = 1,
            Cep = logradouroDto.Cep,
            Nome = logradouroDto.Nome,
            Bairro = logradouroDto.Bairro,
            Cidade = logradouroDto.Cidade,
            Estado = logradouroDto.Estado,
            Pais = logradouroDto.Pais
        };
        _logradouroServiceMock.Setup(s => s.AdicionarAsync(It.IsAny<LogradouroDTO>())).ReturnsAsync(logradouroCriado);
        // Act
        var result = await _logradouroService.AdicionarAsync(logradouroDto);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _logradouroServiceMock.Verify(s => s.AdicionarAsync(It.IsAny<LogradouroDTO>()), Times.Once);
    }
    [Fact]
    public async Task ObterPorCepAsync_DeveRetornarLogradouro_QuandoExistir()
    {
        // Arrange
        var cep = "12345678";
        var logradouroDto = new LogradouroDTO { Id = 1, Cep = cep, Nome = "Rua do CEP", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" };
        _logradouroServiceMock.Setup(s => s.ObterPorCepAsync(cep)).ReturnsAsync(logradouroDto);
        // Act
        var result = await _logradouroService.ObterPorCepAsync(cep);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(cep, result.Cep);
        _logradouroServiceMock.Verify(s => s.ObterPorCepAsync(cep), Times.Once);
    }
    [Fact]
    public async Task ObterPorCidadeAsync_DeveRetornarListaVazia_QuandoNaoHouverLogradourosNaCidade()
    {
        // Arrange
        var cidade = "Cidade Inexistente";
        _logradouroServiceMock.Setup(s => s.ObterPorCidadeAsync(cidade)).ReturnsAsync(new List<LogradouroDTO>());
        // Act
        var result = await _logradouroService.ObterPorCidadeAsync(cidade);
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _logradouroServiceMock.Verify(s => s.ObterPorCidadeAsync(cidade), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarLogradouro_QuandoDadosValidos()
    {
        // Arrange
        var logradouroId = 1;
        var logradouroExistente = new LogradouroDTO { Id = logradouroId, Cep = "12345678", Nome = "Rua Antiga", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" };
        var logradouroAtualizado = new LogradouroDTO { Id = logradouroId, Cep = "12345678", Nome = "Rua Atualizada", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" };
        _logradouroServiceMock.Setup(s => s.AtualizarAsync(It.IsAny<LogradouroDTO>())).ReturnsAsync(logradouroAtualizado);
        // Act
        var result = await _logradouroService.AtualizarAsync(logradouroAtualizado);
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Rua Atualizada", result.Nome);
        _logradouroServiceMock.Verify(s => s.AtualizarAsync(It.IsAny<LogradouroDTO>()), Times.Once);
    }
    [Fact]
    public async Task RemoverAsync_DeveRemoverLogradouro_QuandoExistir()
    {
        // Arrange
        var logradouroId = 1;
        _logradouroServiceMock.Setup(s => s.RemoverAsync(logradouroId)).ReturnsAsync(true);
        // Act
        var result = await _logradouroService.RemoverAsync(logradouroId);
        // Assert
        Assert.True(result);
        _logradouroServiceMock.Verify(s => s.RemoverAsync(logradouroId), Times.Once);
    }
    [Fact]
    public async Task ObterPorCidadeAsync_DeveRetornarLogradouros_QuandoExistirem()
    {
        // Arrange
        var cidade = "São Paulo";
        var logradourosDto = new List<LogradouroDTO>
{
new LogradouroDTO {Id = 1, Cep = "12345678", Nome = "Rua Teste 1", Bairro = "Centro", Cidade = cidade, Estado = "SP", Pais = "Brasil" },
new LogradouroDTO {Id = 2, Cep = "87654321", Nome = "Rua Teste 2", Bairro = "Bela Vista", Cidade = cidade, Estado = "SP", Pais = "Brasil" }
};
        _logradouroServiceMock.Setup(s => s.ObterPorCidadeAsync(cidade)).ReturnsAsync(logradourosDto);
        // Act
        var result = await _logradouroService.ObterPorCidadeAsync(cidade);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, l => Assert.Equal(cidade, l.Cidade));
        _logradouroServiceMock.Verify(s => s.ObterPorCidadeAsync(cidade), Times.Once);
    }
}