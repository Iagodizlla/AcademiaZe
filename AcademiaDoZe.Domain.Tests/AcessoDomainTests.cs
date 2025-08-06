//Iago Henrique Schlemper
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests;

public class AcessoDomainTests
{
    // Cria um Aluno válido para os testes
    private Aluno GetValidAluno()
    {
        var logradouro = Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");

        var foto = Arquivo.Criar(new byte[1], ".jpg");

        return Aluno.Criar(
            "João da Silva",
            "12345678901",
            DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
            "11999999999",
            "joao@email.com",
            logradouro,
            "123",
            "Apto 1",
            "Senha@1",
            foto
        );
    }

    [Fact]
    public void CriarAcesso_ComDadosValidos_DeveCriarObjeto()
    {
        // Arrange
        var aluno = GetValidAluno();
        var tipo = EPessoaTipo.Aluno;
        // Garante que é uma hora válida (entre 6h e 22h) e no futuro
        var dataHora = DateTime.Now.AddMinutes(1);
        if (dataHora.TimeOfDay < new TimeSpan(6, 0, 0))
            dataHora = DateTime.Today.AddHours(6);
        else if (dataHora.TimeOfDay > new TimeSpan(22, 0, 0))
            dataHora = DateTime.Today.AddDays(1).AddHours(6);
        // Act
        var acesso = Acesso.Criar(tipo, aluno, dataHora);
        // Assert
        Assert.NotNull(acesso);
    }

    [Fact]
    public void CriarAcesso_ComTipoInvalido_DeveLancarExcecao()
    {
        // Arrange
        var aluno = GetValidAluno();
        var tipo = (EPessoaTipo)999; // Tipo inválido
        var dataHora = DateTime.Today.AddHours(8);
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            Acesso.Criar(tipo, aluno, dataHora)
        );
        Assert.Equal("TIPO_OBRIGATORIO", ex.Message);
    }
}