//Iago Henrique Schlemper
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.Tests;

public class LogradouroApplicationTests
{
    // Configurações de conexão
    const string connectionString = "Server=localhost;Database=db_academia_do_ze;User Id=root;Password=lilica123@;";
    const EAppDatabaseType databaseType = EAppDatabaseType.MySql;
    [Fact(Timeout = 60000)]
    public async Task LogradouroService_Integracao_Adicionar_Obter_Atualizar_Remover()
    {
        // Arrange: DI usando repositório real (Infra)

        // Configuração dos serviços usando a classe DependencyInjection
        var services = DependencyInjection.ConfigureServices(connectionString, databaseType);

        // Cria o provedor de serviços

        var provider = DependencyInjection.BuildServiceProvider(services);
        // Obtém os serviços necessários via injeção de dependência

        var logradouroService = provider.GetRequiredService<ILogradouroService>();

        // CEP único para o teste
        var _cep = "99500001";

        var dto = new LogradouroDTO
        {
            Cep = _cep,
            Nome = "Rua Teste",
            Bairro = "Centro",
            Cidade = "Cidade X",
            Estado = "SP",
            Pais = "Brasil"
        };
        LogradouroDTO? criado = null;

        try
        {
            // Act - Adicionar
            criado = await logradouroService.AdicionarAsync(dto);
            Assert.NotNull(criado);
            Assert.True(criado!.Id > 0);
            // Act - Obter por CEP
            var obtido = await logradouroService.ObterPorCepAsync(_cep);
            Assert.NotNull(obtido);
            Assert.Equal("Rua Teste", obtido!.Nome);

            // Act - Atualizar

            var atualizarDto = new LogradouroDTO

            {
                Id = criado.Id,
                Cep = criado.Cep,
                Nome = "Rua Atualizada",
                Bairro = criado.Bairro,
                Cidade = criado.Cidade,
                Estado = "RJ",
                Pais = criado.Pais
            };
            var atualizado = await logradouroService.AtualizarAsync(atualizarDto);

            Assert.NotNull(atualizado);
            Assert.Equal("Rua Atualizada", atualizado.Nome);
            Assert.Equal("RJ", atualizado.Estado);

            // Act - Remover

            var removido = await logradouroService.RemoverAsync(criado.Id);
            Assert.True(removido);
            // Assert - Conferir remoção
            var aposRemocao = await logradouroService.ObterPorIdAsync(criado.Id);
            Assert.Null(aposRemocao);

        }
        finally
        {
            // Clean-up defensivo (se algo falhar antes do remove)
            if (criado is not null)

            {
                try { await logradouroService.RemoverAsync(criado.Id); } catch { }
            }
        }
    }
}