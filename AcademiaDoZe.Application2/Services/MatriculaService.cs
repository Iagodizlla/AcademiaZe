using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
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

    public async Task<MatriculaDTO> AdicionarAsync(MatriculaDTO matriculaDto)
    {
        // Verifica se já existe uma matricula com o mesmo Id

        var idExistente = await _repoFactory().ObterPorId(matriculaDto.Id);
        if (idExistente != null)

        {
            throw new InvalidOperationException($"Matricula com ID {idExistente.Id}, já cadastrado com o Id {idExistente.Id}.");
        }
        // Cria a entidade de domínio a partir do DTO
        var matricula = matriculaDto.ToEntity();
        // Salva no repositório
        await _repoFactory().Adicionar(matricula);
        // converte e retorna o DTO - já com o ID gerado, setado pelo repositório
        return matricula.ToDto();

    }

    public async Task<MatriculaDTO> AtualizarAsync(MatriculaDTO matriculaDto)
    {
        // Verifica se a matricula existe

        var matriculaExistente = await _repoFactory().ObterPorId(matriculaDto.Id) ?? throw new KeyNotFoundException($"Matricula ID {matriculaDto.Id} não encontrado.");

        // Verifica se o novo Id já está em uso por outra matricula

        if (!string.Equals(matriculaExistente.Id.ToString(), matriculaDto.Id.ToString(), StringComparison.OrdinalIgnoreCase))

        {
            var idExistente = await _repoFactory().ObterPorId(matriculaDto.Id);
            if (idExistente != null && idExistente.Id != matriculaDto.Id)

            {
                throw new InvalidOperationException($"Matricula com ID {idExistente.Id}, já cadastrado com o Id {idExistente.Id}.");
            }
        }
        // a partir dos dados do dto e do existente, cria uma nova instância com os valores atualizados

        // respeitando o principio de Imutabilidade, onde a entidade original não é modificada, mas sim uma nova instância é criada com os dados atualizados
        var matriculaAtualizado = matriculaExistente.UpdateFromDto(matriculaDto);
        // Atualiza no repositório
        await _repoFactory().Atualizar(matriculaAtualizado);
        return matriculaAtualizado.ToDto();

    }

    public async Task<MatriculaDTO> ObterPorIdAsync(int id)
    {
        var matricula = await _repoFactory().ObterPorId(id);
        return (matricula != null) ? matricula.ToDto() : null!;

    }

    public async Task<IEnumerable<MatriculaDTO>> ObterTodasAsync()
    {
        var matriculas = await _repoFactory().ObterTodos();

        return [.. matriculas.Select(l => l.ToDto())]; // expressão de interpolação para criar uma nova lista de DTOs

    }

    public async Task<IEnumerable<MatriculaDTO>> ObterPorAlunoIdAsync(int alunoId)
    {
        var matriculas = await _repoFactory().ObterPorAluno(alunoId);
        return matriculas.Select(m => m.ToDto());
    }

    public async Task<IEnumerable<MatriculaDTO>> ObterAtivasAsync(int alunoId = 0)
    {
        var matriculas = await _repoFactory().ObterAtivas(alunoId);
        return matriculas.Select(m => m.ToDto());
    }

    public async Task<IEnumerable<MatriculaDTO>> ObterVencendoEmDiasAsync(int dias)
    {
        var matriculas = await _repoFactory().ObterVencendoEmDias(dias);
        return matriculas.Select(m => m.ToDto());
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var matricula = await _repoFactory().ObterPorId(id);

        if (matricula == null)

        {
            return false;
        }
        await _repoFactory().Remover(id);

        return true;

    }
}