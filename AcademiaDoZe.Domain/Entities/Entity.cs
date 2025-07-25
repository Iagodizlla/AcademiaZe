//Iago Henrique Schlemper
namespace AcademiaDoZe.Domain.Entities;

public abstract class Entity
{
    public int Id { get; protected set; }
    public Entity(int id = 0)
    {
        Id = id;
    }
}