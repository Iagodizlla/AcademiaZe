//Iago Henrique Schlemper
using AcademiaDoZe.Domain.Enum;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;

namespace AcademiaDoZe.Domain.Entities;

public class Matricula : Entity
{
    public Aluno AlunoMatricula { get; private set; }
    public EMatriculaPlano Plano { get; private set; }
    public DateOnly DataInicio { get; private set; }
    public DateOnly DataFim { get; private set; }
    public string Objetivo { get; private set; }
    public EMatriculaRestricoes RestricoesMedicas { get; private set; }
    public string ObservacoesRestricoes { get; private set; }
    public Arquivo LaudoMedico { get; private set; }
    public Matricula(Aluno alunoMatricula, EMatriculaPlano plano, DateOnly dataInicio, DateOnly dataFim, string objetivo, EMatriculaRestricoes restricoesMedicas, Arquivo laudoMedico)
    : base()
    {
        AlunoMatricula = alunoMatricula;
        Plano = plano;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Objetivo = objetivo;
        RestricoesMedicas = restricoesMedicas;
        LaudoMedico = laudoMedico;
    }

    public static Matricula Criar(Aluno alunoMatricula, EMatriculaPlano plano, DateOnly dataInicio, DateOnly dataFim, string objetivo, EMatriculaRestricoes restricoesMedicas, Arquivo laudoMedico)
    {
        if (alunoMatricula == null) throw new DomainException("ALUNO_OBRIGATORIO");

        if (EMatriculaPlano.IsDefined(plano)) throw new DomainException("PLANO_OBRIGATORIO");

        if (dataInicio == default) throw new DomainException("DATA_INICIO_OBRIGATORIA");

        if (dataFim == default) throw new DomainException("DATA_FIM_OBRIGATORIA");

        if (dataFim < dataInicio) throw new DomainException("DATA_FIM_MENOR_DATA_INICIO");

        if (string.IsNullOrWhiteSpace(objetivo)) throw new DomainException("OBJETIVO_OBRIGATORIO");

        objetivo = TextoNormalizadoService.LimparEspacos(objetivo);

        if (EMatriculaRestricoes.IsDefined(restricoesMedicas)) throw new DomainException("RESTRICOES_MEDICAS_OBRIGATORIO");

        if (laudoMedico == null) throw new DomainException("LAUDO_MEDICO_OBRIGATORIO");

        return new Matricula(alunoMatricula, plano, dataInicio, dataFim, objetivo, restricoesMedicas, laudoMedico);
    }
}