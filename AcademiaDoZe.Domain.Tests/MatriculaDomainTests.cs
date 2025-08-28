//Iago Henrique Schlemper
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests;

public class MatriculaDomainTests
{
    private Logradouro GetValidLogradouro()
    => Logradouro.Criar(7, "12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");

    private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1]);

    private Aluno GetValidAluno() => Aluno.Criar(
            1,                               // Id
            "Iago Henrique",                  // Nome
            "111.111.111-11",                // CPF
            DateOnly.FromDateTime(DateTime.Today.AddYears(-18)), // Data de nascimento válida
            "(11) 99999-9999",               // Telefone
            "test@gmail.com",                // Email
            GetValidLogradouro(),           // Endereço (Logradouro)
            "123",                           // Número
            "Casa",                          // Complemento
            "Senha@123",                     // Senha válida
            GetValidArquivo()               // Arquivo (foto)
    );

    [Fact]
    public void CriarMatricula_Valido_NaoDeveLancarExcecao()
    {
        var id = 1;
        var aluno = GetValidAluno();
        var plano = EMatriculaPlano.Semestral;
        var dataInicio = DateOnly.FromDateTime(DateTime.Today);
        var dataFim = dataInicio.AddMonths(6);
        var objetivo = "Emagrecer";
        var restricoes = EMatriculaRestricoes.Diabetes;
        var laudoMedico = GetValidArquivo();
        var observacoes = "Não posso sexta";

        var matricula = Matricula.Criar(id, aluno, plano, dataInicio, dataFim, objetivo, restricoes, laudoMedico, observacoes);

        // Assert
        Assert.NotNull(matricula);
    }

    [Fact]
    public void CriarMatricula_ComObjetivoVazio_DeveLancarExcecao()
    {
        // Arrange
        var id = 1;
        var aluno = GetValidAluno();
        var plano = EMatriculaPlano.Semestral;
        var dataInicio = DateOnly.FromDateTime(DateTime.Today); // válido
        var dataFim = dataInicio.AddMonths(6);
        var objetivo = ""; // inválido
        var restricoes = EMatriculaRestricoes.Alergias;
        var laudoMedico = GetValidArquivo();
        var observacoes = "nao posso sexta";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            Matricula.Criar(id, aluno, plano, dataInicio, dataFim, objetivo, restricoes, laudoMedico, observacoes)
        );

        Assert.Equal("OBJETIVO_OBRIGATORIO", exception.Message);
    }
}