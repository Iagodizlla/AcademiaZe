using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;

namespace AcademiaDoZe.Domain.Entities;

public sealed class Logradouro : Entity
{
    public string Cep { get; }
    public string Nome { get; }
    public string Bairro { get; }
    public string Cidade { get; }
    public string Estado { get; }
    public string Pais { get; }
    public Logradouro(string cep, string nome, string bairro, string cidade,  string estado, string pais) : base()
    {
        Cep = cep;
        Nome = nome;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
    }

    public static Logradouro Criar(string cep, string nome, string bairro, string cidade, string estado, string pais)
    {
        if (string.IsNullOrWhiteSpace(cep)) throw new DomainException("CEP_OBRIGATORIO");
        cep = TextoNormalizadoService.LimparEDigitos(cep);
        if (cep.Length != 8) throw new DomainException("CEP_DIGITOS");
        if (string.IsNullOrWhiteSpace(nome)) throw new DomainException("NOME_OBRIGATORIO");
        nome = TextoNormalizadoService.LimparEspacos(nome);
        if (string.IsNullOrWhiteSpace(bairro)) throw new DomainException("BAIRRO_OBRIGATORIO");
        bairro = TextoNormalizadoService.LimparEspacos(bairro);
        if (string.IsNullOrWhiteSpace(cidade)) throw new DomainException("CIDADE_OBRIGATORIO");
        cidade = TextoNormalizadoService.LimparEspacos(cidade);
        if (string.IsNullOrWhiteSpace(estado)) throw new DomainException("ESTADO_OBRIGATORIO");
        estado = TextoNormalizadoService.ParaMaiusculo(TextoNormalizadoService.LimparTodosEspacos(estado));
        if (string.IsNullOrWhiteSpace(pais)) throw new DomainException("PAIS_OBRIGATORIO");
        pais = TextoNormalizadoService.LimparEspacos(pais);

        return new Logradouro(cep, nome, bairro, cidade, estado, pais);

    }
}