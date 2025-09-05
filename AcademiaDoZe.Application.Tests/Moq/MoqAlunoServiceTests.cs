using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Domain.Entities;
using Moq;

namespace AcademiaDoZe.Application.Tests;

public class MoqAlunoServiceTests
{
    private readonly Mock<IAlunoService> _alunoServiceMock;
    private readonly IAlunoService _alunoService;
    public MoqAlunoServiceTests()
    {
        _alunoServiceMock = new Mock<IAlunoService>();
        _alunoService = _alunoServiceMock.Object;
    }
    private AlunoDTO CriarAlunoPadrao(int id = 1)
    {
        return new AlunoDTO
        {
            Id = id,
            Nome = "Aluno Teste",
            Cpf = "12345678901",
            DataNascimento = DateOnly.FromDateTime(DateTime.Now.AddYears(-30)),
            Telefone = "11999999999",
            Email = "aluno@teste.com",
            Endereco = new LogradouroDTO { Id = 1, Cep = "12345678", Nome = "Rua Teste", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" },
            Numero = "100",
            Complemento = "Apto 101",
            Senha = "Senha@123"
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
        _alunoServiceMock.Setup(s => s.CpfJaExisteAsync(cpf, id)).ReturnsAsync(resultadoEsperado);

        // Act
        var resultado = await _alunoService.CpfJaExisteAsync(cpf, id);

        // Assert

        Assert.Equal(resultadoEsperado, resultado);

        _alunoServiceMock.Verify(s => s.CpfJaExisteAsync(cpf, id), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarAluno_QuandoExistir()
    {
        // Arrange
        var alunoId = 1;
        var alunoDto = CriarAlunoPadrao(alunoId);
        _alunoServiceMock.Setup(s => s.ObterPorIdAsync(alunoId)).ReturnsAsync(alunoDto);
        // Act
        var result = await _alunoService.ObterPorIdAsync(alunoId);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(alunoDto.Cpf, result.Cpf);
        _alunoServiceMock.Verify(s => s.ObterPorIdAsync(alunoId), Times.Once);
    }
    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        // Arrange
        var alunoId = 999;
        _alunoServiceMock.Setup(s => s.ObterPorIdAsync(alunoId)).ReturnsAsync((AlunoDTO)null!);
        // Act
        var result = await _alunoService.ObterPorIdAsync(alunoId);
        // Assert
        Assert.Null(result);
        _alunoServiceMock.Verify(s => s.ObterPorIdAsync(alunoId), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornarAlunos_QuandoExistirem()
    {
        // Arrange
        var alunosDto = new List<AlunoDTO>
{
CriarAlunoPadrao(1),
CriarAlunoPadrao(2)
};
        _alunoServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(alunosDto);
        // Act
        var result = await _alunoService.ObterTodosAsync();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _alunoServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaVazia_QuandoNaoHouverAlunos()
    {
        // Arrange
        _alunoServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(new List<AlunoDTO>());
        // Act
        var result = await _alunoService.ObterTodosAsync();
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _alunoServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }
    [Fact]
    public async Task AdicionarAsync_DeveAdicionarAluno_QuandoDadosValidos()
    {
        // Arrange
        var alunoDto = CriarAlunoPadrao(0); // ID 0 para novo registro
        var alunoCriado = CriarAlunoPadrao(1); // ID 1 após criação
                                               // It.IsAny faz com que o Moq aceite qualquer objeto do tipo AlunoDTO
        _alunoServiceMock.Setup(s => s.AdicionarAsync(It.IsAny<AlunoDTO>())).ReturnsAsync(alunoCriado);
        // Act
        var result = await _alunoService.AdicionarAsync(alunoDto);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _alunoServiceMock.Verify(s => s.AdicionarAsync(It.IsAny<AlunoDTO>()), Times.Once);
    }
    [Fact]
    public async Task AtualizarAsync_DeveAtualizaAluno_QuandoDadosValidos()
    {
        // Arrange
        var alunoId = 1;
        var alunoAtualizado = CriarAlunoPadrao(alunoId);
        alunoAtualizado.Nome = "Nome Atualizado";
        _alunoServiceMock.Setup(s => s.AtualizarAsync(It.IsAny<AlunoDTO>())).ReturnsAsync(alunoAtualizado);
        // Act
        var result = await _alunoService.AtualizarAsync(alunoAtualizado);
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Nome Atualizado", result.Nome);
        _alunoServiceMock.Verify(s => s.AtualizarAsync(It.IsAny<AlunoDTO>()), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_DeveRemoverAluno_QuandoExistir()
    {
        // Arrange

        var alunoId = 1;

        _alunoServiceMock.Setup(s => s.RemoverAsync(alunoId)).ReturnsAsync(true);
        // Act

        var result = await _alunoService.RemoverAsync(alunoId);

        // Assert

        Assert.True(result);

        _alunoServiceMock.Verify(s => s.RemoverAsync(alunoId), Times.Once);
    }
    [Fact]
    public async Task RemoverAsync_DeveRetornarFalse_QuandoNaoExistir()
    {
        // Arrange

        var alunoId = 999;

        _alunoServiceMock.Setup(s => s.RemoverAsync(alunoId)).ReturnsAsync(false);
        // Act

        var result = await _alunoService.RemoverAsync(alunoId);

        // Assert

        Assert.False(result);

        _alunoServiceMock.Verify(s => s.RemoverAsync(alunoId), Times.Once);
    }
    [Fact]
    public async Task ObterPorCpfAsync_DeveRetornarAluno_QuandoExistir()
    {
        // Arrange

        var cpf = "12345678901";
        var alunoDto = CriarAlunoPadrao(1);

        alunoDto.Cpf = cpf;
        _alunoServiceMock.Setup(s => s.ObterPorCpfAsync(cpf)).ReturnsAsync(alunoDto);
        // Act

        var result = await _alunoService.ObterPorCpfAsync(cpf);

        // Assert

        Assert.NotNull(result);
        Assert.Equal(cpf, result.Cpf);

        _alunoServiceMock.Verify(s => s.ObterPorCpfAsync(cpf), Times.Once);
    }

    [Fact]
    public async Task ObterPorCpfAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        // Arrange

        var cpf = "99999999999";

        _alunoServiceMock.Setup(s => s.ObterPorCpfAsync(cpf)).ReturnsAsync((AlunoDTO)null!);
        // Act

        var result = await _alunoService.ObterPorCpfAsync(cpf);

        // Assert

        Assert.Null(result);

        _alunoServiceMock.Verify(s => s.ObterPorCpfAsync(cpf), Times.Once);
    }
    [Fact]
    public async Task TrocarSenhaAsync_DeveRetornarTrue_QuandoSucesso()
    {
        // Arrange

        var alunoId = 1;
        var novaSenha = "NovaSenha@123";

        _alunoServiceMock.Setup(s => s.TrocarSenhaAsync(alunoId, novaSenha)).ReturnsAsync(true);
        // Act

        var resultado = await _alunoService.TrocarSenhaAsync(alunoId, novaSenha);

        // Assert

        Assert.True(resultado);

        _alunoServiceMock.Verify(s => s.TrocarSenhaAsync(alunoId, novaSenha), Times.Once);
    }
}