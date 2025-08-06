//Iago Henrique Schlemper
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests;

public class MatriculaDomainTests
{
    private Logradouro GetValidLogradouro()
    => Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");

    private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");

    private Aluno GetValidAluno() => Aluno.Criar(
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
        var matricula = Matricula.Criar(GetValidAluno(), Enums.EMatriculaPlano.Semestral, DateOnly.MinValue, DateOnly.MaxValue,
            "Emagrecer", Enums.EMatriculaRestricoes.None, GetValidArquivo(), "Nao posso sexta");
        Assert.NotNull(matricula); // validando criação, não deve lançar exceção e não deve ser nulo
    }

    [Fact]
    public void CriarMatricula_Invalido_DeveLancarExcecao()
    {
        // validando a criação de logradouro com CEP inválido, deve lançar exceção
        Assert.Throws<DomainException>(() => Matricula.Criar(GetValidAluno(), Enums.EMatriculaPlano.Semestral, DateOnly.MinValue, DateOnly.MaxValue,
            "Emagrecer", Enums.EMatriculaRestricoes.None, GetValidArquivo(), "Nao posso sexta"));
    }

    [Fact]
    public void CriarMatricula_Valido_VerificarNormalizado()
    {
        var maticula = Matricula.Criar(GetValidAluno(), Enums.EMatriculaPlano.Semestral, DateOnly.MinValue, DateOnly.MaxValue,
            "Emagrecer", Enums.EMatriculaRestricoes.None, GetValidArquivo(), "Nao posso sexta");
        Assert.Equal(GetValidAluno(), maticula.AlunoMatricula); // validando normalização
        Assert.Equal(Enums.EMatriculaPlano.Semestral, maticula.Plano);
        Assert.Equal(DateOnly.MinValue, maticula.DataInicio);
        Assert.Equal(DateOnly.MaxValue, maticula.DataFim);
        Assert.Equal("Emagrecer", maticula.Objetivo);
        Assert.Equal(Enums.EMatriculaRestricoes.None, maticula.RestricoesMedicas);
        Assert.Equal(GetValidArquivo(), maticula.LaudoMedico);
        Assert.Equal("Nao posso sexta", maticula.ObservacoesRestricoes);
    }

    [Fact]
    public void CriarMatricula_ComObjetivoVazio_DeveLancarExcecao()
    {
        var exception = Assert.Throws<DomainException>(() => Matricula.Criar(GetValidAluno(), Enums.EMatriculaPlano.Semestral, DateOnly.MinValue, DateOnly.MaxValue,
            "", Enums.EMatriculaRestricoes.None, GetValidArquivo(), "nao posso sexta"));
        Assert.Equal("OBJETIVO_OBRIGATORIO", exception.Message); // validando a mensagem de exceção
    }
}