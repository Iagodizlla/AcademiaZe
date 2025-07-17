namespace AcademiaDoZe.Domain;

public class Pessoa
{
    public string Nome { get; set; }
    public string? Email { get; set; }
    public string Senha { get; set; }
    public string Telefone { get; set; }
    public string Endereco { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Cpf { get; set; }
    public string? Foto { get; set; }
    public Logradouro Logradouro { get; set; }
    public int Numero { get; set; }
    public string? Complemento { get; set; }

    public Pessoa(string nome, string? email, string senha, string telefone, string endereco, DateTime dataNascimento, string cpf, Logradouro logradouro, int numero, string? complemento = null)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
        Telefone = telefone;
        Endereco = endereco;
        DataNascimento = dataNascimento;
        Cpf = cpf;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
    }
}