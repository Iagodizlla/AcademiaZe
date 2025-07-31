using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Entities;

// Classe base para todas as entidades, garantindo identidade única e validação de Id
public abstract class Entity
{
    public int Id { get; protected set; }
    protected Entity(int id = 0)
    {
        if (id < 0) throw new DomainException("ID_NEGATIVO");
        Id = id;
    }
    // reescrita para realizar a igualdade baseada no Id e tipo da entidade - padrão DDD
    // o atributo [AllowNull] ao parâmetro 'obj' para corresponder à nulidade do método base.
    public override bool Equals([System.Diagnostics.CodeAnalysis.AllowNull] object obj)
    {
        if (obj is not Entity other)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id == other.Id && GetType() == other.GetType();
    }
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}