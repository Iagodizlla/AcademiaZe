//Iago Henrique Schlemper
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using Moq;

namespace AcademiaDoZe.Application.Tests;

public class MoqMatriculaServiceTests
{
    private readonly Mock<IMatriculaService> _matriculaServiceMock;
    private readonly IMatriculaService _matriculaService;
    public MoqMatriculaServiceTests()
    {
        // para testar um serviço, em vez de se conectar ao banco de dados real, vamos injetar um mock do repositório.
        // isso permite que você teste a lógica de negócio do seu serviço sem depender do banco de dados.

        _matriculaServiceMock = new Mock<IMatriculaService>();
        _matriculaService = _matriculaServiceMock.Object;
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

    private MatriculaDTO CriarMatriculaPadrao(int id = 1)
    {
        return new MatriculaDTO
        {
            Id = id,
            AlunoMatricula = CriarAlunoPadrao(),
            Plano = Enums.EAppMatriculaPlano.Mensal,
            DataInicio = new DateOnly(2025, 05, 22),
            DataFim = new DateOnly(2025, 06, 22),
            Objetivo = "Hipertrofia",
            RestricoesMedicas = Enums.EAppMatriculaRestricoes.Alergias,
            ObservacoesRestricoes = "Nenhuma"
        };
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarMatricula_QuandoExistir()
    {
        // Arrange
        var matriculaId = 1;
        var logradouroDto = CriarMatriculaPadrao(matriculaId);

        // configura o objeto mock do serviço (_logradouroServiceMock).

        // essa configuração diz ao mock para retornar o logradouroDto sempre que o método ObterPorIdAsync for chamado com o logradouroId definido.
        // isso simula o comportamento de uma busca bem sucedida, sem a necessidade de acessar um banco de dados real.

        _matriculaServiceMock.Setup(s => s.ObterPorIdAsync(matriculaId)).ReturnsAsync(logradouroDto);

        // Act
        // o método que está sendo testado é executado.
        // chama o serviço mock, que, por sua vez, retorna o objeto configurado na etapa de Arrange.
        var result = await _matriculaService.ObterPorIdAsync(matriculaId);

        // Assert

        Assert.NotNull(result);
        Assert.Equal(logradouroDto.AlunoMatricula, result.AlunoMatricula);
        // confirma que o método ObterPorIdAsync no mock foi chamado exatamente uma vez com o logradouroId correto.
        // essa verificação é crucial para garantir que o comportamento do seu código está de acordo com o esperado,
        // ou seja, que a chamada ao serviço realmente aconteceu como deveria.
        _matriculaServiceMock.Verify(s => s.ObterPorIdAsync(matriculaId), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornaMatriculas_QuandoExistirem()
    {
        // Arrange

        var matriculasDto = new List<MatriculaDTO>

{
CriarMatriculaPadrao(1),
CriarMatriculaPadrao(2)
};
        _matriculaServiceMock.Setup(s => s.ObterTodasAsync()).ReturnsAsync(matriculasDto);
        // Act

        var result = await _matriculaService.ObterTodasAsync();

        // Assert

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _matriculaServiceMock.Verify(s => s.ObterTodasAsync(), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaVazia_QuandoNaoHouverMatriculas()
    {
        // Arrange
        _matriculaServiceMock.Setup(s => s.ObterTodasAsync()).ReturnsAsync(new List<MatriculaDTO>());
        // Act

        var result = await _matriculaService.ObterTodasAsync();

        // Assert

        Assert.NotNull(result);
        Assert.Empty(result);

        _matriculaServiceMock.Verify(s => s.ObterTodasAsync(), Times.Once);
    }
    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        // Arrange

        var id = 1;

        _matriculaServiceMock.Setup(s => s.ObterPorIdAsync(id)).ReturnsAsync((MatriculaDTO)null!);
        // Act

        var result = await _matriculaService.ObterPorIdAsync(id);

        // Assert

        Assert.Null(result);

        _matriculaServiceMock.Verify(s => s.ObterPorIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarMatricula_QuandoDadosValidos()
    {
        // Arrange
        var matriculaDto = CriarMatriculaPadrao(1);
        var matriculaCriado = new MatriculaDTO
        {
            Id = 1,
            AlunoMatricula = matriculaDto.AlunoMatricula    ,
            DataInicio = matriculaDto.DataInicio,
            DataFim = matriculaDto.DataFim,
            Objetivo = matriculaDto.Objetivo,
            Plano = matriculaDto.Plano,
            RestricoesMedicas = matriculaDto.RestricoesMedicas,
            LaudoMedico = matriculaDto.LaudoMedico,
            ObservacoesRestricoes = matriculaDto.ObservacoesRestricoes
        };
        _matriculaServiceMock.Setup(s => s.AdicionarAsync(It.IsAny<MatriculaDTO>())).ReturnsAsync(matriculaCriado);
        // Act
        var result = await _matriculaService.AdicionarAsync(matriculaDto);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _matriculaServiceMock.Verify(s => s.AdicionarAsync(It.IsAny<MatriculaDTO>()), Times.Once);
    }
    [Fact]
    public async Task ObterIdCepAsync_DeveRetornarMatricula_QuandoExistir()
    {
        // Arrange
        var id = 1;
        var matriculaDto = CriarMatriculaPadrao(id);
        _matriculaServiceMock.Setup(s => s.ObterPorIdAsync(id)).ReturnsAsync(matriculaDto);
        // Act
        var result = await _matriculaService.ObterPorIdAsync(id);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        _matriculaServiceMock.Verify(s => s.ObterPorIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarMatricula_QuandoDadosValidos()
    {
        // Arrange
        var matriculaId = 1;
        var matriculaExistente = CriarMatriculaPadrao(matriculaId);
        var matriculaAtualizado = new MatriculaDTO
        {
            Id = matriculaId,
            AlunoMatricula = matriculaExistente.AlunoMatricula,
            DataInicio = matriculaExistente.DataInicio,
            DataFim = matriculaExistente.DataFim,
            Objetivo = "Emagrecer", // objetivo atualizado
            Plano = matriculaExistente.Plano,
            RestricoesMedicas = matriculaExistente.RestricoesMedicas,
            LaudoMedico = matriculaExistente.LaudoMedico,
            ObservacoesRestricoes = matriculaExistente.ObservacoesRestricoes
        };
        _matriculaServiceMock.Setup(s => s.AtualizarAsync(It.IsAny<MatriculaDTO>())).ReturnsAsync(matriculaAtualizado);
        // Act
        var result = await _matriculaService.AtualizarAsync(matriculaAtualizado);
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Emagrecer", result.Objetivo);
        _matriculaServiceMock.Verify(s => s.AtualizarAsync(It.IsAny<MatriculaDTO>()), Times.Once);
    }
    [Fact]
    public async Task RemoverAsync_DeveRemoverMatricula_QuandoExistir()
    {
        // Arrange
        var matriculaId = 1;
        _matriculaServiceMock.Setup(s => s.RemoverAsync(matriculaId)).ReturnsAsync(true);
        // Act
        var result = await _matriculaService.RemoverAsync(matriculaId);
        // Assert
        Assert.True(result);
        _matriculaServiceMock.Verify(s => s.RemoverAsync(matriculaId), Times.Once);
    }
}