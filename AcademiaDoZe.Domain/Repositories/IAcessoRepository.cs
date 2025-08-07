using AcademiaDoZe.Domain.Entities;

namespace AcademiaDoZe.Domain.Repositories;

public interface IAcessoRepository : IRepository<Acesso>
{
    // Métodos específicos do domínio
    Task<IEnumerable<Acesso>> GetAcessosPorAlunoPeriodo(int? alunoId = null, DateOnly? inicio = null, DateOnly? fim = null);
    Task<IEnumerable<Acesso>> GetAcessosPorColaboradorPeriodo(int? colaboradorId = null, DateOnly? inicio = null, DateOnly? fim = null);
    // horário mensal de maior procura, baseado na entrada, por exemplo, em dezembro o horário de maior procura é entre 18h e 20h
    // Retorna um dicionário onde a chave é o horário e o valor é a quantidade de acessos nesse horário
    Task<Dictionary<TimeOnly, int>> GetHorarioMaisProcuradoPorMes(int mes);
    // Permanência média dos alunos na academia, mensal.
    // retorna um dicionário onde a chave é o mês e o valor é a média de permanência dos alunos nesse mês
    Task<Dictionary<int, TimeSpan>> GetPermanenciaMediaPorMes(int mes);
    // alunos que não registraram acesso nos últimos x dias
    Task<IEnumerable<Aluno>> GetAlunosSemAcessoNosUltimosDias(int dias);
}