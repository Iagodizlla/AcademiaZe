//Iago Henrique Schlemper
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.Tests;

public class AlunoApplicationTests
{
    // Configurações de conexão
    const string connectionString = "Server=localhost;Database=db_academia_do_ze;User Id=root;Password=lilica123@;";
    const EAppDatabaseType databaseType = EAppDatabaseType.MySql;
    [Fact(Timeout = 60000)]
    public async Task AlunoService_Integracao_Adicionar_Obter_Atualizar_Remover()
    {
        // Arrange: DI usando repositório real (Infra)
        // Configuração dos serviços usando a classe DependencyInjection
        var services = DependencyInjection.ConfigureServices(connectionString, databaseType);
        // Cria o provedor de serviços
        var provider = DependencyInjection.BuildServiceProvider(services);
        // Obtém os serviços necessários via injeção de dependência
        var alunoService = provider.GetRequiredService<IAlunoService>();
        var logradouroService = provider.GetRequiredService<ILogradouroService>();
        // Gera um CPF único para evitar conflito
        var _cpf = GerarCpfFake();
        // obter o logradouro
        var logradouro = await logradouroService.ObterPorIdAsync(5);
        Assert.NotNull(logradouro);
        Assert.Equal(5, logradouro!.Id);
        // cria um arquivo (para facilitar, copie uma foto para dentro do diretório com os fontes do teste)
        // caminho relativo da foto
        var caminhoFoto = Path.Combine("..", "..", "..", "foto_teste.png");
        ArquivoDTO foto = new();

        if (File.Exists(caminhoFoto)) { foto.Conteudo = await File.ReadAllBytesAsync(caminhoFoto); }

        else { foto.Conteudo = null; Assert.Fail("Foto de teste não encontrada."); }
        var dto = new AlunoDTO
        {
            Nome = "Aluno Teste",
            Cpf = _cpf,
            DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
            Telefone = "11999999999",
            Email = "aluno@teste.com",
            Endereco = logradouro,
            Numero = "100",
            Complemento = "Apto 1",
            Senha = "Senha@1",
            Foto = foto
        };
        AlunoDTO? criado = null;

        try
        {
            // Act - Adicionar
            criado = await alunoService.AdicionarAsync(dto);
            // Assert - criação
            Assert.NotNull(criado);
            Assert.True(criado!.Id > 0);
            Assert.Equal(_cpf, criado.Cpf);
            // Act - Obter por cpf
            var obtido = await alunoService.ObterPorCpfAsync(criado.Cpf);
            // Assert - obter
            Assert.NotNull(obtido);
            Assert.Equal(criado.Id, obtido!.Id);
            Assert.Equal(_cpf, obtido.Cpf);
            // Act - Atualizar
            var atualizar = new AlunoDTO
            {
                Id = criado.Id,
                Nome = "Aluno Atualizado",
                Cpf = criado.Cpf,
                DataNascimento = criado.DataNascimento,
                Telefone = criado.Telefone,
                Email = criado.Email,
                Endereco = criado.Endereco,
                Numero = criado.Numero,
                Complemento = criado.Complemento,
                Senha = criado.Senha,
                Foto = criado.Foto
            };
            var atualizado = await alunoService.AtualizarAsync(atualizar);
            // Assert - atualizar
            Assert.NotNull(atualizado);
            Assert.Equal("Aluno Atualizado", atualizado.Nome);
            // Act - Remover
            var removido = await alunoService.RemoverAsync(criado.Id);
            Assert.True(removido);
            // Act - Conferir remoção
            var aposRemocao = await alunoService.ObterPorIdAsync(criado.Id);
            Assert.Null(aposRemocao);
        }
        finally
        {
            // Clean-up defensivo (se algo falhar antes do remove)
            if (criado is not null)
            {
                try { await alunoService.RemoverAsync(criado.Id); } catch { }
            }
        }
    }
    // Helper simples para gerar um CPF numérico de 11 dígitos (sem validação de dígito verificador)
    private static string GerarCpfFake()
    {
        var rnd = new Random();
        return string.Concat(Enumerable.Range(0, 11).Select(_ => rnd.Next(0, 10).ToString()));
    }
}