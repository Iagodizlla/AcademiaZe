//Iago Henrique Schlemper
namespace AcademiaDoZe.Domain.Entities;

public record Arquivo
{
    public byte[] Conteudo { get; }
    public Arquivo(byte[] conteudo)
    {
        Conteudo = conteudo;
    }
}