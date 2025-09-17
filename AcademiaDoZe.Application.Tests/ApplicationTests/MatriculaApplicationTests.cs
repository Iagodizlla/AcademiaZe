//Iago Henrique Schlemper
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.Tests;

public class MatriculaApplicationTests
{
    // Configurações de conexão (iguais às que você já usa)
    const string connectionString = "Server=localhost;Database=db_academia_do_ze;User Id=root;Password=lilica123@;";
    const EAppDatabaseType databaseType = EAppDatabaseType.MySql;

    [Fact(Timeout = 60000)]
    public async Task MatriculaService_Integracao_Adicionar_Obter_Atualizar_Remover()
    {
        // Arrange: DI usando repositório real (Infra)
        var services = DependencyInjection.ConfigureServices(connectionString, databaseType);
        var provider = DependencyInjection.BuildServiceProvider(services);

        var alunoService = provider.GetRequiredService<IAlunoService>();
        var logradouroService = provider.GetRequiredService<ILogradouroService>();
        var matriculaService = provider.GetRequiredService<IMatriculaService>();

        // Gera um CPF único para evitar conflito
        var _cpf = GerarCpfFake();

        // obter o logradouro (mantendo seu padrão)
        var logradouro = await logradouroService.ObterPorIdAsync(5);
        Assert.NotNull(logradouro);
        Assert.Equal(5, logradouro!.Id);

        // cria um arquivo de foto (mantendo exatamente seu padrão)
        var caminhoFoto = Path.Combine("..", "..", "..", "foto_teste.png");
        ArquivoDTO foto = new();
        if (File.Exists(caminhoFoto))
        {
            foto.Conteudo = await File.ReadAllBytesAsync(caminhoFoto);
        }
        else
        {
            foto.Conteudo = null;
            Assert.Fail("Foto de teste não encontrada.");
        }

        // monta o aluno (igual ao seu estilo)
        var dtoAluno = new AlunoDTO
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

        AlunoDTO? alunoCriado = null;
        MatriculaDTO? matriculaCriada = null;

        try
        {
            // 1) Persiste o ALUNO primeiro (evita erro de FK)
            alunoCriado = await alunoService.AdicionarAsync(dtoAluno);
            Assert.NotNull(alunoCriado);
            Assert.True(alunoCriado!.Id > 0);

            // 2) Prepara LaudoMedico com bytes válidos (requerido porque há restrição médica != None)
            var laudo = new ArquivoDTO { Conteudo = new byte[] { 1, 2, 3 } };

            // 3) Cria a MATRÍCULA referenciando o aluno já persistido (não fixe Id; deixe 0/default)
            var dtoMatricula = new MatriculaDTO
            {
                Id = 1000,
                AlunoMatricula = alunoCriado,
                Plano = Enums.EAppMatriculaPlano.Mensal,
                DataInicio = new DateOnly(2025, 05, 22),
                DataFim = new DateOnly(2025, 06, 22),
                Objetivo = "Emagrecer",
                RestricoesMedicas = Enums.EAppMatriculaRestricoes.Alergias,
                ObservacoesRestricoes = "Nenhuma",
                LaudoMedico = laudo
            };

            // Act - Adicionar matrícula
            matriculaCriada = await matriculaService.AdicionarAsync(dtoMatricula);
            Assert.NotNull(matriculaCriada);
            Assert.True(matriculaCriada!.Id > 0);

            // Act - Obter por Id retornado
            var obtida = await matriculaService.ObterPorIdAsync(matriculaCriada.Id);
            Assert.NotNull(obtida);
            Assert.Equal("Emagrecer", obtida!.Objetivo);

            // Act - Atualizar (mantendo Laudo para não quebrar regra de domínio)
            var atualizarDto = new MatriculaDTO
            {
                Id = obtida.Id,
                AlunoMatricula = obtida.AlunoMatricula,
                Plano = Enums.EAppMatriculaPlano.Trimestral,
                DataInicio = obtida.DataInicio,
                DataFim = obtida.DataFim,
                Objetivo = "Hipertrofia",
                RestricoesMedicas = Enums.EAppMatriculaRestricoes.Diabetes,
                ObservacoesRestricoes = "Nenhuma",
                LaudoMedico = obtida.LaudoMedico // mantém laudo
            };

            var atualizado = await matriculaService.AtualizarAsync(atualizarDto);
            Assert.NotNull(atualizado);
            Assert.Equal("Hipertrofia", atualizado!.Objetivo);

            // Act - Remover
            var removido = await matriculaService.RemoverAsync(atualizado.Id);
            Assert.True(removido);

            var aposRemocao = await matriculaService.ObterPorIdAsync(atualizado.Id);
            Assert.Null(aposRemocao);
        }
        finally
        {
            // Clean-up defensivo
            if (matriculaCriada is not null)
            {
                try { await matriculaService.RemoverAsync(matriculaCriada.Id); } catch { }
            }

            if (alunoCriado is not null)
            {
                try { await alunoService.RemoverAsync(alunoCriado.Id); } catch { }
            }
        }
    }

    // Helper simples para gerar um CPF numérico de 11 dígitos
    private static string GerarCpfFake()
    {
        var rnd = new Random();
        return string.Concat(Enumerable.Range(0, 11).Select(_ => rnd.Next(0, 10).ToString()));
    }
}
