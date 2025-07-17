
namespace AcademiaDoZe.Domain;

public class Aluno : Pessoa
{
    public Aluno(string nome, string? email, string senha, string telefone, string endereco, DateTime dataNascimento, string cpf, Logradouro logradouro, int numero, string? complemento = null) : base(nome, email, senha, telefone, endereco, dataNascimento, cpf, logradouro, numero, complemento)
    {
    }
}