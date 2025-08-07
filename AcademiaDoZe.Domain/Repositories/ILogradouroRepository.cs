//Iago Henrique Schlemper
using AcademiaDoZe.Domain.Entities;

namespace AcademiaDoZe.Domain.Repositories;

public interface ILogradouroRepository : IRepository<Logradouro>
{
    // Métodos específicos do domínio

    Task<Logradouro?> ObterPorCep(string cep);
    Task<IEnumerable<Logradouro>> ObterPorCidade(string cidade);
}