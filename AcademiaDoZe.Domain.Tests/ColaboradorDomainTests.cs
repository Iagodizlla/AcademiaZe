//Iago Henrique Schlemper
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests;

public class ColaboradorDomainTests
{
    private Logradouro GetValidLogradouro() => Logradouro.Criar("12345670", "Rua A", "Centro", "Cidade", "SP", "Brasil");
    private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");

    [Fact]
    public void CriarColaborador_Valido_NaoDeveLancarExcecao()
    {
        var colaborador = Colaborador.Criar("Iago Henrique", "111.111.111-11", DateOnly.MinValue, "11999999999", "Teste@gmail.com",
            GetValidLogradouro(), "44", "casa", "senha123", GetValidArquivo(), DateOnly.MaxValue, Enums.EColaboradorTipo.Administrador,
            Enums.EColaboradorVinculo.Estagio);
        Assert.NotNull(colaborador); // validando criação, não deve lançar exceção e não deve ser nulo
    }
    [Fact]
    public void CriarColaborador_Invalido_DeveLancarExcecao()
    {
        // validando a criação de logradouro com CEP inválido, deve lançar exceção
        Assert.Throws<DomainException>(() => Colaborador.Criar("Iago Henrique", "111.111.111-11", DateOnly.MinValue, "11999999999", "Teste@gmail.com",
            GetValidLogradouro(), "44", "casa", "senha123", GetValidArquivo(), DateOnly.MaxValue, Enums.EColaboradorTipo.Administrador,
            Enums.EColaboradorVinculo.Estagio));
    }
    [Fact]
    public void CriarColaborador_Valido_VerificarNormalizado()
    {
        var colaborador = Colaborador.Criar("Iago Henrique ", "111.111.111-11 ", DateOnly.MinValue, "11999999999 ", "Teste@gmail.com",
            GetValidLogradouro(), "44", "casa", "senha123 ", GetValidArquivo(), DateOnly.MaxValue, Enums.EColaboradorTipo.Administrador,
            Enums.EColaboradorVinculo.Estagio);
        Assert.Equal("Iago Henrique", colaborador.Nome); // validando normalização
        Assert.Equal("111.111.111-11", colaborador.Cpf);
        Assert.Equal(DateOnly.MinValue, colaborador.DataNascimento);
        Assert.Equal("11999999999", colaborador.Telefone);
        Assert.Equal("Teste@gmail.com", colaborador.Email);
        Assert.Equal(GetValidLogradouro(), colaborador.Endereco);
        Assert.Equal("44", colaborador.Numero);
        Assert.Equal("casa", colaborador.Complemento);
        Assert.Equal("senha123", colaborador.Senha);
        Assert.Equal(DateOnly.MaxValue, colaborador.DataAdmissao);
        Assert.Equal(Enums.EColaboradorTipo.Administrador, colaborador.Tipo);
        Assert.Equal(Enums.EColaboradorVinculo.Estagio, colaborador.Vinculo);
    }
    [Fact]
    public void CriarColaborador_Invalido_VerificarMessageExcecao()
    {
        var exception = Assert.Throws<DomainException>(() => Colaborador.Criar("Iago Henrique", "111.111.111-11", DateOnly.MinValue, "11999999999", "Teste@gmail.com",
            GetValidLogradouro(), "44", "casa", "senha123", GetValidArquivo(), DateOnly.MaxValue, Enums.EColaboradorTipo.Administrador,
            Enums.EColaboradorVinculo.Estagio));
        Assert.Equal("NOME_OBRIGATORIO", exception.Message); // validando a mensagem de exceção
    }
}