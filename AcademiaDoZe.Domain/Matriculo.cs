using AcademiaDoZe.Domain.Enum;

namespace AcademiaDoZe.Domain;

public class Matriculo
{
    public Aluno Aluno { get; set; }
    public PlanoEnum Plano { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Objetivo { get; set; }
    public List<RestricaoEnum>? Restricao { get; set; }
    public string? ObservacaoRestricao { get; set; }
    public string? LaudoMedico { get; set; }

    public Matriculo(Aluno aluno, PlanoEnum plano, DateTime dataInicio, DateTime dataFim, string objetivo, List<RestricaoEnum>? restricao = null, string? observacaoRestricao = null, string? laudoMedico = null)
    {
        Aluno = aluno;
        Plano = plano;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Objetivo = objetivo;
        Restricao = restricao;
        ObservacaoRestricao = observacaoRestricao;
        LaudoMedico = laudoMedico;
    }
}