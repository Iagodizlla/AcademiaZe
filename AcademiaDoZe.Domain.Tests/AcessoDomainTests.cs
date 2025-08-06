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
        var pessoa = GetValidPessoa2();
        var horarioValido = DateTime.Today.AddHours(10); // 10:00 da manhã do mesmo dia

        var acesso = Acesso.Criar(EPessoaTipo.Colaborador, pessoa, horarioValido);

        Assert.NotNull(acesso);
    }

    [Fact]
    public void CriarAcesso_Invalido_DeveLancarExcecao()
    {
        Assert.Throws<DomainException>(() => Acesso.Criar(EPessoaTipo.Aluno, GetValidPessoa1(), DateTime.Now)); // validando acesso com pessoa nula, deve lançar exceção
    }

    [Fact]
    public void CriarAcesso_Valido_VerificarNormalizado()
    {
        var pessoa = GetValidPessoa2();
        var horarioValido = DateTime.Now.AddHours(1); // 1 hora no futuro

        var acesso = Acesso.Criar(EPessoaTipo.Colaborador, pessoa, horarioValido);

        Assert.Equal(EPessoaTipo.Colaborador, acesso.Tipo);
        Assert.Equal(pessoa, acesso.AlunoColaborador);
        Assert.Equal(horarioValido, acesso.DataHora);
    }

    [Fact]
    public void CriarAcesso_ComPessoaNula_DeveLancarExcecao()
    {
        var ex = Assert.Throws<DomainException>(() =>
            Acesso.Criar(EPessoaTipo.Aluno, null, DateTime.Now)
        );
        Assert.Equal("PESSOA_OBRIGATORIA", ex.Message);
    }
}