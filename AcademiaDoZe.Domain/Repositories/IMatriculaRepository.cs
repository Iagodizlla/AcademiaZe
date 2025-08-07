using AcademiaDoZe.Domain.Entities;

namespace AcademiaDoZe.Domain.Repositories;

public interface IMatriculaRepository : IRepository<Matricula>
{
    // Métodos específicos do domínio

    Task<IEnumerable<Matricula>> ObterPorAluno(int alunoId);
    Task<IEnumerable<Matricula>> ObterAtivas();
    Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias);
}