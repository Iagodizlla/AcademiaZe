//Iago Henrique Schlemper
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Entities;

public class Acesso : Entity
{
    // encapsulamento das propriedades, aplicando imutabilidade
    public EPessoaTipo Tipo { get; private set; }
    public Pessoa AlunoColaborador { get; private set; }
    public DateTime DataHora { get; private set; }
    // construtor privado para evitar instância direta
    private Acesso(EPessoaTipo tipo, Pessoa pessoa, DateTime dataHora) : base()
    {
        Tipo = tipo;
        AlunoColaborador = pessoa;
        DataHora = dataHora;
    }
    // método de fábrica, ponto de entrada para criar um objeto válido e normalizado
    public static Acesso Criar(EPessoaTipo tipo, Pessoa pessoa, DateTime dataHora)
    {
        // Validações e normalizações

        if (!Enum.IsDefined(tipo)) throw new DomainException("TIPO_OBRIGATORIO");
        if (pessoa == null) throw new DomainException("PESSOA_OBRIGATORIA");
        if (dataHora < DateTime.Now) throw new DomainException("DATAHORA_INVALIDA");
        if (dataHora.TimeOfDay < new TimeSpan(6, 0, 0) || dataHora.TimeOfDay > new TimeSpan(22, 0, 0))
            throw new DomainException("DATAHORA_INTERVALO");
        // validações especificas de aluno ou colaborador
        if (pessoa is Aluno aluno)

        {
            // Validar se possui matrícula ativa - depende da persistência de aluno e matrícula.

            // Na entrada, mostrar quanto tempo ainda tem de plano - depende da persistência de aluno e matrícula.
            // Na saída, mostrar o tempo que permaneceu na academia - depende da persistência de aluno e matrícula.

        }
        else if (pessoa is Colaborador colaborador)
        {
            // Validar se já não ultrapassa o limite de: 8 horas se for ctl, 6 horas se for estágio.

            // Na saída, mostrar o tempo que permaneceu na academia, devendo ser somado todos os registros do dia.

        }
        // cria e retorna o objeto
        return new Acesso(tipo, pessoa, dataHora);
    }
}