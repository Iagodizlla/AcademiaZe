////Iago Henrique Schlemper
//using AcademiaDoZe.Domain.Entities;
//using AcademiaDoZe.Domain.Enums;
//using AcademiaDoZe.Domain.Repositories;
//using AcademiaDoZe.Domain.ValueObjects;
//using AcademiaDoZe.Infrastructure.Data;
//using System.Data;
//using System.Data.Common;

//namespace AcademiaDoZe.Infrastructure.Repositories;

//public class AcessoRepository : BaseRepository<Acesso>, IAcessoRepository
//{
//    public AcessoRepository(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType){  }

//    protected override async Task<Acesso> MapAsync(DbDataReader reader)
//    {
//        try
//        {
//            var pessoaId = Convert.ToInt32(reader["pessoa_id"]);
//            var tipo = (EPessoaTipo)Convert.ToInt32(reader["tipo"]);

//            Pessoa pessoa;
//            if (tipo == EPessoaTipo.Aluno)
//            {
//                var alunoRepo = new AlunoRepository(_connectionString, _databaseType);
//                pessoa = await alunoRepo.ObterPorId(pessoaId)
//                    ?? throw new InvalidOperationException($"Aluno com ID {pessoaId} não encontrado.");
//            }
//            else if (tipo == EPessoaTipo.Colaborador)
//            {
//                var colaboradorRepo = new ColaboradorRepository(_connectionString, _databaseType);
//                pessoa = await colaboradorRepo.ObterPorId(pessoaId)
//                    ?? throw new InvalidOperationException($"Colaborador com ID {pessoaId} não encontrado.");
//            }
//            else
//            {
//                throw new InvalidOperationException($"Tipo de pessoa desconhecido: {tipo}");
//            }

//            var acesso = Acesso.Criar(
//                tipo: tipo,
//                pessoa: pessoa,
//                dataHora: Convert.ToDateTime(reader["data_hora"])
//            );

//            // Seta o ID
//            typeof(Entity).GetProperty("Id")?.SetValue(acesso, Convert.ToInt32(reader["id_acesso"]));

//            return acesso;

//        }
//        catch (DbException ex) { throw new InvalidOperationException($"Erro ao mapear dados do acesso: {ex.Message}", ex); }
//    }

//    public override async Task<Acesso> Adicionar(Acesso entity)
//    {
//        try
//        {
//            await using var connection = await GetOpenConnectionAsync();
//            string query = _databaseType == DatabaseType.SqlServer
//            ? $"INSERT INTO {TableName} (tipo, pessoa, data_hora) "
//            + "OUTPUT INSERTED.id_acesso "
//            + "VALUES (@Tipo, @Pessoa, @Data_hora);"
//            : $"INSERT INTO {TableName} (tipo, pessoa, data_hora) "
//            + "VALUES (@Tipo, @Pessoa, @Data_hora); "
//            + "SELECT LAST_INSERT_ID();";
//            await using var command = DbProvider.CreateCommand(query, connection);
//            command.Parameters.Add(DbProvider.CreateParameter("@Tipo", entity.Tipo, DbType.String, _databaseType));
//            command.Parameters.Add(DbProvider.CreateParameter("@Pessoa", entity.AlunoColaborador, DbType.String, _databaseType));
//            command.Parameters.Add(DbProvider.CreateParameter("@Data_hora", entity.DataHora, DbType.String, _databaseType));
//            var id = await command.ExecuteScalarAsync();
//            if (id != null && id != DBNull.Value)
//            {
//                // Define o ID usando reflection
//                var idProperty = typeof(Entity).GetProperty("Id");
//                idProperty?.SetValue(entity, Convert.ToInt32(id));
//            }
//            return entity;
//        }
//        catch (DbException ex) { throw new InvalidOperationException($"Erro ao adicionar acesso: {ex.Message}", ex); }
//    }

//    public override async Task<Acesso> Atualizar(Acesso entity)
//    {
//        try
//        {
//            await using var connection = await GetOpenConnectionAsync();
//            string query = $"UPDATE {TableName} "
//            + "SET tipo = @Cpf, "
//            + "pessoa = @Pessoa, "
//            + "data_hora = @Data_hora "
//            + "WHERE id_acesso = @Id";
//            await using var command = DbProvider.CreateCommand(query, connection);
//            command.Parameters.Add(DbProvider.CreateParameter("@Tipo", entity.Tipo, DbType.String, _databaseType));
//            command.Parameters.Add(DbProvider.CreateParameter("@Pessoa", entity.AlunoColaborador, DbType.String, _databaseType));
//            command.Parameters.Add(DbProvider.CreateParameter("@Data_hora", entity.DataHora, DbType.String, _databaseType));
//            int rowsAffected = await command.ExecuteNonQueryAsync();
//            if (rowsAffected == 0)
//            {
//                throw new InvalidOperationException($"Nenhum acesso encontrado com o ID {entity.Id} para atualização.");
//            }
//            return entity;
//        }
//        catch (DbException ex)
//        {
//            throw new InvalidOperationException($"Erro ao atualizar acesso com ID {entity.Id}: {ex.Message}", ex);
//        }
//    }

//    public async Task<IEnumerable<Acesso>> GetAcessosPorAlunoPeriodo(int? alunoId = null, DateOnly? inicio = null, DateOnly? fim = null)
//    {
//        try
//        {
//            await using var connection = await GetOpenConnectionAsync();

//            var conditions = new List<string> { "tipo = @Tipo" }; // garantir que são alunos
//            if (alunoId.HasValue)
//                conditions.Add("pessoa_id = @PessoaId");
//            if (inicio.HasValue)
//                conditions.Add("datahora >= @Inicio");
//            if (fim.HasValue)
//                conditions.Add("datahora <= @Fim");

//            var whereClause = $"WHERE {string.Join(" AND ", conditions)}";
//            var query = $"SELECT * FROM {TableName} {whereClause} ORDER BY datahora";

//            await using var command = DbProvider.CreateCommand(query, connection);
//            command.Parameters.Add(DbProvider.CreateParameter("@Tipo", (int)EPessoaTipo.Aluno, DbType.Int32, _databaseType));
//            if (alunoId.HasValue)
//                command.Parameters.Add(DbProvider.CreateParameter("@PessoaId", alunoId.Value, DbType.Int32, _databaseType));
//            if (inicio.HasValue)
//                command.Parameters.Add(DbProvider.CreateParameter("@Inicio", inicio.Value.ToDateTime(new TimeOnly(0, 0)), DbType.DateTime, _databaseType));
//            if (fim.HasValue)
//                command.Parameters.Add(DbProvider.CreateParameter("@Fim", fim.Value.ToDateTime(new TimeOnly(23, 59, 59)), DbType.DateTime, _databaseType));

//            var acessos = new List<Acesso>();
//            using var reader = await command.ExecuteReaderAsync();
//            while (await reader.ReadAsync())
//            {
//                var acesso = await MapAsync(reader);
//                if (acesso != null)
//                    acessos.Add(acesso);
//            }

//            return acessos;
//        }
//        catch (DbException ex)
//        {
//            throw new InvalidOperationException($"Erro ao obter acessos para o aluno de ID {alunoId}: {ex.Message}", ex);
//        }
//    }

//    public async Task<IEnumerable<Acesso>> GetAcessosPorColaboradorPeriodo(int? colaboradorId = null, DateOnly? inicio = null, DateOnly? fim = null)
//    {
//        try
//        {
//            await using var connection = await GetOpenConnectionAsync();

//            var conditions = new List<string> { "tipo = @Tipo" }; // tipo = Colaborador
//            if (colaboradorId.HasValue)
//                conditions.Add("pessoa_id = @ColaboradorId");
//            if (inicio.HasValue)
//                conditions.Add("datahora >= @Inicio");
//            if (fim.HasValue)
//                conditions.Add("datahora <= @Fim");

//            var whereClause = $"WHERE {string.Join(" AND ", conditions)}";
//            var query = $"SELECT * FROM {TableName} {whereClause} ORDER BY datahora";

//            await using var command = DbProvider.CreateCommand(query, connection);
//            command.Parameters.Add(DbProvider.CreateParameter("@Tipo", (int)EPessoaTipo.Colaborador, DbType.Int32, _databaseType));
//            if (colaboradorId.HasValue)
//                command.Parameters.Add(DbProvider.CreateParameter("@ColaboradorId", colaboradorId.Value, DbType.Int32, _databaseType));
//            if (inicio.HasValue)
//                command.Parameters.Add(DbProvider.CreateParameter("@Inicio", inicio.Value.ToDateTime(new TimeOnly(0, 0)), DbType.DateTime, _databaseType));
//            if (fim.HasValue)
//                command.Parameters.Add(DbProvider.CreateParameter("@Fim", fim.Value.ToDateTime(new TimeOnly(23, 59, 59)), DbType.DateTime, _databaseType));

//            var acessos = new List<Acesso>();
//            using var reader = await command.ExecuteReaderAsync();
//            while (await reader.ReadAsync())
//            {
//                var acesso = await MapAsync(reader);
//                if (acesso != null)
//                    acessos.Add(acesso);
//            }

//            return acessos;
//        }
//        catch (DbException ex)
//        {
//            throw new InvalidOperationException($"Erro ao obter acessos do colaborador de ID {colaboradorId}: {ex.Message}", ex);
//        }
//    }

//    public async Task<IEnumerable<Aluno>> GetAlunosSemAcessoNosUltimosDias(int dias)
//    {
//        try
//        {
//            await using var connection = await GetOpenConnectionAsync();

//            string dateExprSql = _databaseType == DatabaseType.SqlServer
//                ? "DATEADD(day, -@Dias, GETDATE())"
//                : "DATE_SUB(NOW(), INTERVAL @Dias DAY)";

//            // Consulta para buscar IDs dos alunos que não acessaram nos últimos N dias
//            var query = $@"
//            SELECT p.id 
//            FROM pessoa p
//            WHERE p.tipo = @TipoAluno
//            AND NOT EXISTS (
//                SELECT 1 FROM {TableName} a 
//                WHERE a.pessoa_id = p.id 
//                  AND a.datahora >= {dateExprSql}
//            );";

//            await using var command = DbProvider.CreateCommand(query, connection);
//            command.Parameters.Add(DbProvider.CreateParameter("@TipoAluno", (int)EPessoaTipo.Aluno, DbType.Int32, _databaseType));
//            command.Parameters.Add(DbProvider.CreateParameter("@Dias", dias, DbType.Int32, _databaseType));

//            var alunos = new List<Aluno>();
//            using var reader = await command.ExecuteReaderAsync();

//            var alunoRepo = new AlunoRepository(_connectionString, _databaseType);
//            while (await reader.ReadAsync())
//            {
//                var alunoId = Convert.ToInt32(reader["id"]);
//                var aluno = await alunoRepo.ObterPorId(alunoId);
//                if (aluno != null)
//                    alunos.Add(aluno);
//            }

//            return alunos;
//        }
//        catch (DbException ex)
//        {
//            throw new InvalidOperationException(
//                $"Erro ao obter alunos sem acesso nos últimos {dias} dias: {ex.Message}", ex);
//        }
//    }

//    // retorna horário mais procurado (count) por mês: chave = TimeOnly (hora:minuto), valor = quantidade de acessos
//    public async Task<Dictionary<TimeOnly, int>> GetHorarioMaisProcuradoPorMes(int mes)
//    {
//        try
//        {
//            await using var connection = await GetOpenConnectionAsync();

//            // Agrupa por hora:minuto extraída de datahora e conta acessos no mês informado
//            string monthFilter = _databaseType == DatabaseType.SqlServer
//                ? "MONTH(datahora) = @Mes AND YEAR(datahora) = YEAR(GETDATE())"
//                : "MONTH(datahora) = @Mes AND YEAR(datahora) = YEAR(NOW())";

//            string query = $@"
//                SELECT DATEPART(HOUR, datahora) as Hora, DATEPART(MINUTE, datahora) as Minuto, COUNT(*) as Qtde
//                FROM {TableName}
//                WHERE {monthFilter}
//                GROUP BY DATEPART(HOUR, datahora), DATEPART(MINUTE, datahora)
//                ORDER BY Qtde DESC;";

//            await using var command = DbProvider.CreateCommand(query, connection);
//            command.Parameters.Add(DbProvider.CreateParameter("@Mes", mes, DbType.Int32, _databaseType));

//            var result = new Dictionary<TimeOnly, int>();
//            using var reader = await command.ExecuteReaderAsync();
//            while (await reader.ReadAsync())
//            {
//                var hora = Convert.ToInt32(reader["Hora"]);
//                var minuto = Convert.ToInt32(reader["Minuto"]);
//                var qtde = Convert.ToInt32(reader["Qtde"]);
//                var time = new TimeOnly(hora, minuto);
//                if (!result.ContainsKey(time))
//                    result.Add(time, qtde);
//            }

//            return result;
//        }
//        catch (DbException ex)
//        {
//            throw new InvalidOperationException($"Erro ao obter horário mais procurado para o mês {mes}: {ex.Message}", ex);
//        }
//    }

//    // retorna permanência média por mês: chave = pessoaId, valor = TimeSpan (média diária de permanência naquele mês)
//    public async Task<Dictionary<int, TimeSpan>> GetPermanenciaMediaPorMes(int mes)
//    {
//        try
//        {
//            await using var connection = await GetOpenConnectionAsync();

//            // Estratégia:
//            // - Assumimos que cada entrada no acesso representa um ponto (entrada ou saída).
//            // - Para cada pessoa, por dia, ordenamos datahora e pareamos (entrada->saída) para calcular permanência diária.
//            // - Calculamos média diária das permanências no mês.
//            // OBS: implementação SQL complexa depende do dialeto. Aqui faremos uma abordagem simples em duas etapas:
//            // 1) Buscar todos acessos do mês ordenados por pessoa/datahora.
//            // 2) No C# agrupamos e calculamos permanência média.

//            string monthFilter = _databaseType == DatabaseType.SqlServer
//                ? "MONTH(datahora) = @Mes AND YEAR(datahora) = YEAR(GETDATE())"
//                : "MONTH(datahora) = @Mes AND YEAR(datahora) = YEAR(NOW())";

//            var query = $"SELECT pessoa_id, datahora FROM {TableName} WHERE {monthFilter} ORDER BY pessoa_id, datahora";

//            await using var command = DbProvider.CreateCommand(query, connection);
//            command.Parameters.Add(DbProvider.CreateParameter("@Mes", mes, DbType.Int32, _databaseType));

//            var acessosPorPessoa = new Dictionary<int, List<DateTime>>();
//            using var reader = await command.ExecuteReaderAsync();
//            while (await reader.ReadAsync())
//            {
//                var pessoaId = Convert.ToInt32(reader["pessoa_id"]);
//                var datahora = Convert.ToDateTime(reader["datahora"]);
//                if (!acessosPorPessoa.TryGetValue(pessoaId, out var list))
//                {
//                    list = new List<DateTime>();
//                    acessosPorPessoa[pessoaId] = list;
//                }
//                list.Add(datahora);
//            }

//            var resultado = new Dictionary<int, TimeSpan>();
//            foreach (var kv in acessosPorPessoa)
//            {
//                var pessoaId = kv.Key;
//                var times = kv.Value;
//                // parear entradas por dia: assumindo sequência entrada-saída-entrada-saída...
//                var permanenciasDiarias = new List<TimeSpan>();
//                for (int i = 0; i + 1 < times.Count; i += 2)
//                {
//                    var entrada = times[i];
//                    var saida = times[i + 1];
//                    // Proteção: só considerar se mesma data (ou permitir crossing? aqui considera mesmo dia)
//                    if (entrada.Date == saida.Date && saida >= entrada)
//                        permanenciasDiarias.Add(saida - entrada);
//                }
//                if (permanenciasDiarias.Count > 0)
//                {
//                    var averageTicks = (long)permanenciasDiarias.Average(ts => ts.Ticks);
//                    resultado[pessoaId] = new TimeSpan(averageTicks);
//                }
//                else
//                    resultado[pessoaId] = TimeSpan.Zero;
//            }

//            return resultado;
//        }
//        catch (DbException ex)
//        {
//            throw new InvalidOperationException($"Erro ao calcular permanência média para o mês {mes}: {ex.Message}", ex);
//        }
//    }
//}