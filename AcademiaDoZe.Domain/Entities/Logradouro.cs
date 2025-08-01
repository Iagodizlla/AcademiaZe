using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;

//Iago Henrique Schlemper
namespace AcademiaDoZe.Domain.Entities;

public sealed class Logradouro : Entity
{
    // encapsulamento das propriedades, aplicando imutabilidade
    public string Cep { get; }
    public string Nome { get; }
    public string Bairro { get; }
    public string Cidade { get; }
    public string Estado { get; }
    public string Pais { get; }
    // construtor privado para evitar instância direta
    private Logradouro(string cep, string nome, string bairro, string cidade, string estado, string pais) : base()
    {
        Cep = cep;
        Nome = nome;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
    }
    // método de fábrica, ponto de entrada para criar um objeto válido e normalizado
    public static Logradouro Criar(string cep, string nome, string bairro, string cidade, string estado, string pais)
    {
        // Validações e normalizações

        if (NormalizadoService.TextoVazioOuNulo(cep)) throw new DomainException("CEP_OBRIGATORIO");

        cep = NormalizadoService.LimparEDigitos(cep);
        if (cep.Length != 8) throw new DomainException("CEP_DIGITOS");
        if (NormalizadoService.TextoVazioOuNulo(nome)) throw new DomainException("NOME_OBRIGATORIO");
        nome = NormalizadoService.LimparEspacos(nome);
        if (NormalizadoService.TextoVazioOuNulo(bairro)) throw new DomainException("BAIRRO_OBRIGATORIO");
        bairro = NormalizadoService.LimparEspacos(bairro);
        if (NormalizadoService.TextoVazioOuNulo(cidade)) throw new DomainException("CIDADE_OBRIGATORIO");
        cidade = NormalizadoService.LimparEspacos(cidade);
        if (NormalizadoService.TextoVazioOuNulo(estado)) throw new DomainException("ESTADO_OBRIGATORIO");
        estado = NormalizadoService.ParaMaiusculo(NormalizadoService.LimparTodosEspacos(estado));
        if (NormalizadoService.TextoVazioOuNulo(pais)) throw new DomainException("PAIS_OBRIGATORIO");
        pais = NormalizadoService.LimparEspacos(pais);
        // criação e retorno do objeto

        return new Logradouro(cep, nome, bairro, cidade, estado, pais);

    }
}