namespace AcademiaDoZe.Domain;

public class Logradouro
{
    public string Cep { get; set; }
    public string Pais { get; set; }
    public string Estado { get; set; }
    public string Cidade { get; set; }
    public string Bairro { get; set; }
    public string Nome { get; set; }

    public Logradouro(string cep, string pais, string estado, string cidade, string bairro, string nome)
    {
        Cep = cep;
        Pais = pais;
        Estado = estado;
        Cidade = cidade;
        Bairro = bairro;
        Nome = nome;
    }
}