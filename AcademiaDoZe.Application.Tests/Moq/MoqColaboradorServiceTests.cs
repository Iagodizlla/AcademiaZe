using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Moq;

namespace AcademiaDoZe.Application.Tests;

public class MoqColaboradorServiceTests
{
    private readonly Mock<IColaboradorService> _colaboradorServiceMock;
    private readonly IColaboradorService _colaboradorService;
    public MoqColaboradorServiceTests()
    {
        _colaboradorServiceMock = new Mock<IColaboradorService>();
        _colaboradorService = _colaboradorServiceMock.Object;
    }
    private ColaboradorDTO CriarColaboradorPadrao(int id = 1)
    {
        return new ColaboradorDTO
        {
            Id = id,
            Nome = "Colaborador Teste",
            Cpf = "12345678901",
            DataNascimento = DateOnly.FromDateTime(DateTime.Now.AddYears(-30)),
            Telefone = "11999999999",
            Email = "colaborador@teste.com",
            Endereco = new LogradouroDTO { Id = 1, Cep = "12345678", Nome = "Rua Teste", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" },
            Numero = "100",
            Complemento = "Apto 101",
            Senha = "Senha@123",
            DataAdmissao = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)),
            Tipo = EAppColaboradorTipo.Administrador,
            Vinculo = EAppColaboradorVinculo.CLT
        };
    }
    // [Theory] indica um método de teste que pode receber dados de diferentes fontes.
    // [InlineData] fornece diretamente esses dados (cpf, id, e resultadoEsperado) para que o teste seja executado múltiplas vezes com valores distintos, testando diversos cenários de uma vez.
    [Theory]
    [InlineData("12345678901", null, true)]
    [InlineData("12345678901", 1, false)]
    [InlineData("99999999999", null, false)]
    public async Task CpfJaExisteAsync_DeveRetornarResultadoCorreto(string cpf, int? id, bool resultadoEsperado)
    {
        // Arrange
        _colaboradorServiceMock.Setup(s => s.CpfJaExisteAsync(cpf, id)).ReturnsAsync(resultadoEsperado);

        // Act
        var resultado = await _colaboradorService.CpfJaExisteAsync(cpf, id);

        // Assert

        Assert.Equal(resultadoEsperado, resultado);

        _colaboradorServiceMock.Verify(s => s.CpfJaExisteAsync(cpf, id), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarColaborador_QuandoExistir()
    {
        // Arrange
        var colaboradorId = 1;
        var colaboradorDto = CriarColaboradorPadrao(colaboradorId);
        _colaboradorServiceMock.Setup(s => s.ObterPorIdAsync(colaboradorId)).ReturnsAsync(colaboradorDto);
        // Act
        var result = await _colaboradorService.ObterPorIdAsync(colaboradorId);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(colaboradorDto.Cpf, result.Cpf);
        _colaboradorServiceMock.Verify(s => s.ObterPorIdAsync(colaboradorId), Times.Once);
    }
    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        // Arrange
        var colaboradorId = 999;
        _colaboradorServiceMock.Setup(s => s.ObterPorIdAsync(colaboradorId)).ReturnsAsync((ColaboradorDTO)null!);
        // Act
        var result = await _colaboradorService.ObterPorIdAsync(colaboradorId);
        // Assert
        Assert.Null(result);
        _colaboradorServiceMock.Verify(s => s.ObterPorIdAsync(colaboradorId), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornarColaboradores_QuandoExistirem()
    {
        // Arrange
        var colaboradoresDto = new List<ColaboradorDTO>
{
CriarColaboradorPadrao(1),
CriarColaboradorPadrao(2)
};
        _colaboradorServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(colaboradoresDto);
        // Act
        var result = await _colaboradorService.ObterTodosAsync();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _colaboradorServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaVazia_QuandoNaoHouverColaboradores()
    {
        // Arrange
        _colaboradorServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(new List<ColaboradorDTO>());
        // Act
        var result = await _colaboradorService.ObterTodosAsync();
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _colaboradorServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }
    [Fact]
    public async Task AdicionarAsync_DeveAdicionarColaborador_QuandoDadosValidos()
    {
        // Arrange
        var colaboradorDto = CriarColaboradorPadrao(0); // ID 0 para novo registro
        var colaboradorCriado = CriarColaboradorPadrao(1); // ID 1 após criação
                                                           // It.IsAny faz com que o Moq aceite qualquer objeto do tipo ColaboradorDTO
        _colaboradorServiceMock.Setup(s => s.AdicionarAsync(It.IsAny<ColaboradorDTO>())).ReturnsAsync(colaboradorCriado);
        // Act
        var result = await _colaboradorService.AdicionarAsync(colaboradorDto);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _colaboradorServiceMock.Verify(s => s.AdicionarAsync(It.IsAny<ColaboradorDTO>()), Times.Once);
    }
    [Fact]
    public async Task AtualizarAsync_DeveAtualizarColaborador_QuandoDadosValidos()
    {
        // Arrange
        var colaboradorId = 1;
        var colaboradorAtualizado = CriarColaboradorPadrao(colaboradorId);
        colaboradorAtualizado.Nome = "Nome Atualizado";
        _colaboradorServiceMock.Setup(s => s.AtualizarAsync(It.IsAny<ColaboradorDTO>())).ReturnsAsync(colaboradorAtualizado);
        // Act
        var result = await _colaboradorService.AtualizarAsync(colaboradorAtualizado);
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Nome Atualizado", result.Nome);
        _colaboradorServiceMock.Verify(s => s.AtualizarAsync(It.IsAny<ColaboradorDTO>()), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_DeveRemoverColaborador_QuandoExistir()
    {
        // Arrange

        var colaboradorId = 1;

        _colaboradorServiceMock.Setup(s => s.RemoverAsync(colaboradorId)).ReturnsAsync(true);
        // Act

        var result = await _colaboradorService.RemoverAsync(colaboradorId);

        // Assert

        Assert.True(result);

        _colaboradorServiceMock.Verify(s => s.RemoverAsync(colaboradorId), Times.Once);
    }
    [Fact]
    public async Task RemoverAsync_DeveRetornarFalse_QuandoNaoExistir()
    {
        // Arrange

        var colaboradorId = 999;

        _colaboradorServiceMock.Setup(s => s.RemoverAsync(colaboradorId)).ReturnsAsync(false);
        // Act

        var result = await _colaboradorService.RemoverAsync(colaboradorId);

        // Assert

        Assert.False(result);

        _colaboradorServiceMock.Verify(s => s.RemoverAsync(colaboradorId), Times.Once);
    }
    [Fact]
    public async Task ObterPorCpfAsync_DeveRetornarColaborador_QuandoExistir()
    {
        // Arrange

        var cpf = "12345678901";
        var colaboradorDto = CriarColaboradorPadrao(1);

        colaboradorDto.Cpf = cpf;
        _colaboradorServiceMock.Setup(s => s.ObterPorCpfAsync(cpf)).ReturnsAsync(colaboradorDto);
        // Act

        var result = await _colaboradorService.ObterPorCpfAsync(cpf);

        // Assert

        Assert.NotNull(result);
        Assert.Equal(cpf, result.Cpf);

        _colaboradorServiceMock.Verify(s => s.ObterPorCpfAsync(cpf), Times.Once);
    }

    [Fact]
    public async Task ObterPorCpfAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        // Arrange

        var cpf = "99999999999";

        _colaboradorServiceMock.Setup(s => s.ObterPorCpfAsync(cpf)).ReturnsAsync((ColaboradorDTO)null!);
        // Act

        var result = await _colaboradorService.ObterPorCpfAsync(cpf);

        // Assert

        Assert.Null(result);

        _colaboradorServiceMock.Verify(s => s.ObterPorCpfAsync(cpf), Times.Once);
    }
    [Fact]
    public async Task TrocarSenhaAsync_DeveRetornarTrue_QuandoSucesso()
    {
        // Arrange

        var colaboradorId = 1;
        var novaSenha = "NovaSenha@123";

        _colaboradorServiceMock.Setup(s => s.TrocarSenhaAsync(colaboradorId, novaSenha)).ReturnsAsync(true);
        // Act

        var resultado = await _colaboradorService.TrocarSenhaAsync(colaboradorId, novaSenha);

        // Assert

        Assert.True(resultado);

        _colaboradorServiceMock.Verify(s => s.TrocarSenhaAsync(colaboradorId, novaSenha), Times.Once);
    }
}