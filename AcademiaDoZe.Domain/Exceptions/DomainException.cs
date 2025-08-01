//Iago Henrique Schlemper
namespace AcademiaDoZe.Domain.Exceptions;

// classe base para exceções de domínio
// permitindo exceções específicas de regras de negócio
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}