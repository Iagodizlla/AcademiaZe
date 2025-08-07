//Iago Henrique Schlemper
namespace AcademiaDoZe.Infrastructure.Exceptions;

// Classe base para exceções de infraestrutura
public class InfrastructureException : Exception
{
    public InfrastructureException(string message) : base(message)
    {
    }
    public InfrastructureException(string message, Exception innerException) : base(message, innerException)
    {
    }
}