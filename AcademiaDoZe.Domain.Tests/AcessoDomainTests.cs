//Iago Henrique Schlemper
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests;

public class AcessoDomainTests
{
    private Logradouro GetValidLogradouro() => Logradouro.Criar("12345670", "Rua A", "Centro", "Cidade", "SP", "Brasil");
    private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");
    private Aluno GetValidPessoa1() => Aluno.Criar("Iago Henrique", "111.111.111-11", DateOnly.MinValue, "11999999999", "Test@gmail.com", GetValidLogradouro(),
        "44", "casa", "senha123", GetValidArquivo());
    private Colaborador GetValidPessoa2() => Colaborador.Criar("Iago Henrique", "111.111.111-11", DateOnly.MinValue, "11999999999", "Test@gmail.com", GetValidLogradouro(),
        "44", "casa", "senha123", GetValidArquivo(), DateOnly.MaxValue, EColaboradorTipo.Administrador, EColaboradorVinculo.CLT);

    [Fact]
    public void CriarAcesso_Valido_NaoDeveLancarExcecao()
    {
        var acesso = Acesso.Criar(EPessoaTipo.Colaborador, GetValidPessoa2(), DateTime.Now);
        Assert.NotNull(acesso); // validando criação de acesso, não deve lançar exceção e não deve ser nulo
    }

    [Fact]
    public void CriarAcesso_Invalido_DeveLancarExcecao()
    {
        Assert.Throws<DomainException>(() => Acesso.Criar(EPessoaTipo.Aluno, GetValidPessoa1(), DateTime.Now)); // validando acesso com pessoa nula, deve lançar exceção
    }

    [Fact]
    public void CriarAcesso_Valido_VerificarNormalizado()
    {
        var acesso = Acesso.Criar(EPessoaTipo.Colaborador, GetValidPessoa2(), DateTime.Now);
        Assert.Equal(EPessoaTipo.Colaborador, acesso.Tipo); // validando tipo de pessoa
        Assert.Equal(GetValidPessoa2(), acesso.AlunoColaborador); // validando pessoa associada ao acesso
        Assert.InRange(acesso.DataHora, DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1)); // validando data e hora do acesso
    }

    [Fact]
    public void CriarAcesso_Invalido_VerificarMessageExcecao()
    {
        var exception = Assert.Throws<DomainException>(() => Acesso.Criar(EPessoaTipo.Aluno, GetValidPessoa1(), DateTime.Now));
        Assert.Equal("PESSOA_OBRIGATORIA", exception.Message); // validando a mensagem de exceção para pessoa nula
    }
}