﻿//Iago Henrique Schlemper
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AcademiaDoZe.Domain.Enums;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.Name ?? value.ToString();

    }
}
// Console.WriteLine( EMatriculaRestricoes.ProblemasRespiratorios.GetDisplayName() );
// Exibe: Problemas Respiratórios