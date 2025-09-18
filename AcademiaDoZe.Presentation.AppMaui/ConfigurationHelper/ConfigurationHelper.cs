using AcademiaDoZe.Application.DependencyInjection;
using AcademiaDoZe.Application.Enums;
namespace AcademiaDoZe.Presentation.AppMaui.Configuration
{
    public static class ConfigurationHelper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // dados conexão

            const string dbServer = "localhost";
            const string dbPort = "3306"; // porta padrão do MySQL
            const string dbDatabase = "db_academia_do_ze";
            const string dbUser = "root";
            const string dbPassword = "abcBolinhas12345";
            // se for necessário indicar a porta, incluir junto em dbComplemento

            // Configurações de conexão
            const string connectionString = $"Server={dbServer};Port={dbPort};Database={dbDatabase};User Id={dbUser};Password={dbPassword};";

            const EAppDatabaseType databaseType = EAppDatabaseType.SqlServer;
            // Configura a fábrica de repositórios com a string de conexão e tipo de banco
            services.AddSingleton(new RepositoryConfig
            {
                ConnectionString = connectionString,
                DatabaseType = databaseType
            });
            // configura os serviços da camada de aplicação
            services.AddApplicationServices();
        }
    }
}