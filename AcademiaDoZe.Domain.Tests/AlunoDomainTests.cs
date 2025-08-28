//Iago Henrique Schlemper
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests;

public class AlunoDomainTests
{
    // Padrão AAA (Arrange, Act, Assert)
    // Arrange (Organizar): Preparamos tudo que o teste precisa.
    private Logradouro GetValidLogradouro() => Logradouro.Criar(6, "12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
    private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1]);
    [Fact] // [Fact] é um atributo que marca este método como um teste para o xUnit.
    public void CriarAluno_ComDadosValidos_DeveCriarObjeto() // Padrão de Nomenclatura: MetodoTestado_Cenario_ResultadoEsperado
    {
        // Arrange
        var id = 1;  var nome = "João da Silva"; var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
        var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
        // Act
        var aluno = Aluno.Criar(id, nome, cpf, dataNascimento, telefone, email, logradouro, numero, complemento, senha, foto);
        // Assert
        Assert.NotNull(aluno);
    }
    [Fact]
    public void CriarAluno_ComNomeVazio_DeveLancarExcecao()
    {
        // Arrange
        var id = 1;  var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
        var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@123"; var foto = GetValidArquivo();
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
        Aluno.Criar(
        id, 
        "",
        cpf,
        dataNascimento,
        telefone,
        email,
        logradouro,
        numero,
        complemento,
        senha,
        foto
        ));
        Assert.Equal("NOME_OBRIGATORIO", ex.Message);
    }
}