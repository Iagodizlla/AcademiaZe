using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Repositories;

namespace AcademiaDoZe.Application_.Services;

public class MatriculaService : IMatriculaService
{
    private readonly Func<IMatriculaRepository> _repoFactory;
    public MatriculaService(Func<IMatriculaRepository> repoFactory)
    {
        _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
    }

    public Task<MatriculaDTO> AdicionarAsync(MatriculaDTO matriculaDto)
    {
        throw new NotImplementedException();
    }

    public Task<MatriculaDTO> AtualizarAsync(MatriculaDTO matriculaDto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MatriculaDTO>> ObterAtivasAsync(int alunoId = 0)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MatriculaDTO>> ObterPorAlunoIdAsync(int alunoId)
    {
        throw new NotImplementedException();
    }

    public Task<MatriculaDTO> ObterPorIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MatriculaDTO>> ObterTodasAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MatriculaDTO>> ObterVencendoEmDiasAsync(int dias)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoverAsync(int id)
    {
        throw new NotImplementedException();
    }
}