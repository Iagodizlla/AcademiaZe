using AcademiaDoZe.Domain.Enum;

namespace AcademiaDoZe.Domain;

public class Colaborador : Pessoa
{
    public DateTime DataEmissao { get; set; }
    public TipoEnum Tipo { get; set; }
    public VinculoEnum Vinculo { get; set; }

    public Colaborador(string nome, string? email, string senha, string telefone, string endereco, DateTime dataNascimento, string cpf, Logradouro logradouro, int numero, DateTime dataEmissao, TipoEnum tipo, VinculoEnum vinculo, string? complemento = null) 
        : base(nome, email, senha, telefone, endereco, dataNascimento, cpf, logradouro, numero, complemento)
    {
        DataEmissao = dataEmissao;
        Tipo = tipo;
        Vinculo = vinculo;
    }
}