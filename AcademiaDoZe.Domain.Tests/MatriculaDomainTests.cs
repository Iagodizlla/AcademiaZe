using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests;

public class MatriculaDomainTests
{
    private Logradouro GetValidLogradouro() => Logradouro.Criar("12345670", "Rua A", "Centro", "Cidade", "SP", "Brasil");
    private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");
    private Aluno GetValidPessoa() => Aluno.Criar("Iago Henrique", "111.111.111-11", DateOnly.MinValue, "11999999999", "Test@gmail.com", GetValidLogradouro(),
        "44", "casa", "senha123", GetValidArquivo());
    private Arquivo GetValidArquivo2() => Arquivo.Criar(new byte[1], ".png");

    [Fact]
    public void CriarMatricula_Valido_NaoDeveLancarExcecao()
    {
        var matricula = Matricula.Criar(GetValidPessoa(), Enums.EMatriculaPlano.Semestral, DateOnly.MinValue, DateOnly.MaxValue,
            "Emagrecer", Enums.EMatriculaRestricoes.None, GetValidArquivo2(), "Nao posso sexta");
        Assert.NotNull(matricula); // validando criação, não deve lançar exceção e não deve ser nulo
    }

    [Fact]
    public void CriarMatricula_Invalido_DeveLancarExcecao()
    {
        // validando a criação de logradouro com CEP inválido, deve lançar exceção
        Assert.Throws<DomainException>(() => Matricula.Criar(GetValidPessoa(), Enums.EMatriculaPlano.Semestral, DateOnly.MinValue, DateOnly.MaxValue,
            "Emagrecer", Enums.EMatriculaRestricoes.None, GetValidArquivo2(), "Nao posso sexta"));
    }

    [Fact]
    public void CriarMatricula_Valido_VerificarNormalizado()
    {
        var maticula = Matricula.Criar(GetValidPessoa(), Enums.EMatriculaPlano.Semestral, DateOnly.MinValue, DateOnly.MaxValue,
            "Emagrecer", Enums.EMatriculaRestricoes.None, GetValidArquivo2(), "Nao posso sexta");
        Assert.Equal(GetValidPessoa(), maticula.AlunoMatricula); // validando normalização
        Assert.Equal(Enums.EMatriculaPlano.Semestral, maticula.Plano);
        Assert.Equal(DateOnly.MinValue, maticula.DataInicio);
        Assert.Equal(DateOnly.MaxValue, maticula.DataFim);
        Assert.Equal("Emagrecer", maticula.Objetivo);
        Assert.Equal(Enums.EMatriculaRestricoes.None, maticula.RestricoesMedicas);
        Assert.Equal(GetValidArquivo2(), maticula.LaudoMedico);
        Assert.Equal("Nao posso sexta", maticula.ObservacoesRestricoes);
    }

    [Fact]
    public void CriarMatricula_Invalido_VerificarMessageExcecao()
    {
        var exception = Assert.Throws<DomainException>(() => Matricula.Criar(GetValidPessoa(), Enums.EMatriculaPlano.Semestral, DateOnly.MinValue, DateOnly.MaxValue,
            "Emagrecer", Enums.EMatriculaRestricoes.None, GetValidArquivo2(), "Nao posso sexta"));
        Assert.Equal("NOME_OBRIGATORIO", exception.Message); // validando a mensagem de exceção
    }
}