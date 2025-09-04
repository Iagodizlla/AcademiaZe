using AcademiaDoZe.Application.DependencyInjection;
using AcademiaDoZe.Application.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.Tests;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(string connectionString, EAppDatabaseType databaseType)
    {
        var services = new ServiceCollection();
        // Configura os serviços da camada de aplicação
        services.AddApplicationServices();

        // AddScoped: cria uma instância do serviço por requisição HTTP.
        // AddSingleton: cria uma única instância do serviço durante toda a vida útil da aplicação.
        // AddTransient: cria uma nova instância do serviço toda vez que ele é solicitado.
        // Configura a fábrica de repositórios com a string de conexão e tipo de banco

        services.AddSingleton(new RepositoryConfig { ConnectionString = connectionString, DatabaseType = databaseType });
        return services;
    }
    public static IServiceProvider BuildServiceProvider(IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }
}