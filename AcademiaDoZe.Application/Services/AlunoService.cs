using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Repositories;

namespace AcademiaDoZe.Application_.Services;

public class AlunoService : IAlunoService
{
    private readonly Func<IAlunoRepository> _repoFactory;
    public AlunoService(Func<IAlunoRepository> repoFactory)
    {
        _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
    }

    public Task<AlunoDTO> AdicionarAsync(AlunoDTO alunoDto)
    {
        throw new NotImplementedException();
    }

    public Task<AlunoDTO> AtualizarAsync(AlunoDTO alunoDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CpfJaExisteAsync(string cpf, int? id = null)
    {
        throw new NotImplementedException();
    }

    public Task<AlunoDTO> ObterPorCpfAsync(string cpf)
    {
        throw new NotImplementedException();
    }

    public Task<AlunoDTO> ObterPorIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AlunoDTO>> ObterTodosAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoverAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TrocarSenhaAsync(int id, string novaSenha)
    {
        throw new NotImplementedException();
    }
}