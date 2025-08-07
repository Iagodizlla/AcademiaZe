//Iago Henrique Schlemper
using AcademiaDoZe.Domain.Entities;

namespace AcademiaDoZe.Domain.Repositories;

// Interface generic para repositórios, permite a criação de repositórios específicos para cada entidade do domínio.
// Define os contratos essenciais para a persistência de dados.
// Delega a implementação para a camada de infraestrutura, o que é correto.
// Herda de Entity para garantir que TEntity seja uma entidade válida, e seu uso somente no domain.
// Métodos assíncronos (Task), alinhados com práticas modernas de acesso a dados.
public interface IRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> ObterPorId(int id);
    Task<IEnumerable<TEntity>> ObterTodos();
    Task<TEntity> Adicionar(TEntity entity);
    Task<TEntity> Atualizar(TEntity entity);
    Task<bool> Remover(int id);
}