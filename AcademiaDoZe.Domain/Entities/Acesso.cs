using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Entities;

public class Acesso : Entity
{
    public Pessoa AlunoColaborador { get; private set; }
    public DateTime DataHora { get; private set; }
    public Acesso(Pessoa pessoa, DateTime dataHora)
    {
        AlunoColaborador = pessoa;
        DataHora = dataHora;
    }

    public static Acesso Criar(Pessoa pessoa, DateTime dataHora)
    {
        if (pessoa == null) throw new DomainException("PESSOA_OBRIGATORIA");

        if (dataHora == default) throw new DomainException("DATA_HORA_OBRIGATORIA");

        return new Acesso(pessoa, dataHora);
    }
}