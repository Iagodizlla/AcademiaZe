using System.ComponentModel.DataAnnotations;

namespace AcademiaDoZe.Domain.Enum;

public enum EColaboradorVinculo
{
    [Display(Name = "CLT")]
    CLT = 0,
    [Display(Name = "Estagiário")]
    Estagio = 1
}