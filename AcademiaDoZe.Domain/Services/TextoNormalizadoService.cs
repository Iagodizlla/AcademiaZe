//Iago Henrique Schlemper
using System.Text.RegularExpressions;

namespace AcademiaDoZe.Domain.Services;

public static partial class TextoNormalizadoService
{
    // Remove espaços repetidos e espaços no início e no final do texto
    public static string LimparEspacos(string? texto) => string.IsNullOrWhiteSpace(texto) ? string.Empty : EspacosRegex().Replace(texto, " ").Trim();
    // Limpa todos os espaços
    public static string LimparTodosEspacos(string? texto) => string.IsNullOrWhiteSpace(texto) ? string.Empty : texto.Replace(" ", string.Empty);
    // Converte o texto para maiúsculo
    public static string ParaMaiusculo(string? texto) => string.IsNullOrEmpty(texto) ? string.Empty : texto.ToUpperInvariant();
    // Manter somente digitos numericos
    public static string LimparEDigitos(string? texto) => string.IsNullOrEmpty(texto) ? string.Empty : new string([.. texto.Where(char.IsDigit)]);
    [GeneratedRegex(@"\s+")]
    private static partial Regex EspacosRegex();
}