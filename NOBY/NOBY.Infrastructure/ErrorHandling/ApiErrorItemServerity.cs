using System.ComponentModel.DataAnnotations;

namespace NOBY.Infrastructure.ErrorHandling;

/// <summary>
/// Zavaznost chyby
/// </summary>
public enum ApiErrorItemServerity
{
    [Display(Name = "Chyba")]
    Error = 1,

    [Display(Name = "Varování")]
    Warning = 2,

    [Display(Name = "Informace")]
    Info = 3
}

