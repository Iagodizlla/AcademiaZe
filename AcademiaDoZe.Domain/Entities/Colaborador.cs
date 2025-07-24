using AcademiaDoZe.Domain.Enum;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;

namespace AcademiaDoZe.Domain.Entities;

public class Colaborador : Pessoa
{
    public DateOnly DataAdmissao { get; private set; }
    public EColaboradorTipo Tipo { get; private set; }
    public EColaboradorVinculo Vinculo { get; private set; }
    public Colaborador(string nomeCompleto, string cpf, DateOnly dataNascimento, string telefone, string email, Logradouro endereco, string numero, string complemento, string senha, Arquivo foto,
        DateOnly dataAdmissao, EColaboradorTipo tipo, EColaboradorVinculo vinculo)
    : base(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto)
    {
        DataAdmissao = dataAdmissao;
        Tipo = tipo;
        Vinculo = vinculo;
    }

    public static Colaborador Criar(string nomeCompleto, string cpf, DateOnly dataNascimento, string telefone, string email, Logradouro endereco, string numero, string complemento, string senha, Arquivo foto, string nome, string Cpf, DateOnly data, string Telefone, string Email, string Endereco, string Numero, string Complemento, string Senha, Arquivo Foto, DateOnly DataAdmissao, EColaboradorTipo Tipo, EColaboradorVinculo Vinculo)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto)) throw new DomainException("NOME_OBRIGATORIO");
        nomeCompleto = TextoNormalizadoService.LimparEspacos(nomeCompleto);

        if (string.IsNullOrWhiteSpace(cpf)) throw new DomainException("CPF_OBRIGATORIO");
        cpf = TextoNormalizadoService.LimparEDigitos(cpf);

        if (cpf.Length != 11) throw new DomainException("CPF_DIGITOS");

        if (dataNascimento == default) throw new DomainException("DATA_NASCIMENTO_OBRIGATORIA");

        if (string.IsNullOrWhiteSpace(telefone)) throw new DomainException("TELEFONE_OBRIGATORIO");
        telefone = TextoNormalizadoService.LimparEDigitos(telefone);
        if (telefone.Length < 10 || telefone.Length > 11) throw new DomainException("TELEFONE_DIGITOS");

        if (string.IsNullOrWhiteSpace(email)) throw new DomainException("EMAIL_OBRIGATORIO");
        email = TextoNormalizadoService.LimparEspacos(email);

        if (endereco == null) throw new DomainException("ENDERECO_OBRIGATORIO");

        if (string.IsNullOrWhiteSpace(numero)) throw new DomainException("NUMERO_OBRIGATORIO");
        numero = TextoNormalizadoService.LimparEspacos(numero);

        if (string.IsNullOrWhiteSpace(complemento)) complemento = string.Empty;
        complemento = TextoNormalizadoService.LimparEspacos(complemento);

        if (string.IsNullOrWhiteSpace(senha)) throw new DomainException("SENHA_OBRIGATORIA");
        senha = TextoNormalizadoService.LimparEspacos(senha);
        if (senha.Length < 6) throw new DomainException("SENHA_MINIMA_6_DIGITOS");

        if (foto == null) throw new DomainException("FOTO_OBRIGATORIA");

        if (DataAdmissao == default) throw new DomainException("DATA_ADMISSAO_OBRIGATORIA");

        if (EColaboradorTipo.IsDefined(Tipo)) throw new DomainException("TIPO_COLABORADOR_OBRIGATORIO");

        if (EColaboradorVinculo.IsDefined(Vinculo)) throw new DomainException("VINCULO_COLABORADOR_OBRIGATORIO");

        return new Colaborador(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, DataAdmissao, Tipo, Vinculo);
    }
}