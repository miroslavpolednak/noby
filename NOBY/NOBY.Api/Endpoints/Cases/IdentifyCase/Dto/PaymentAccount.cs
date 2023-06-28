using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Cases.IdentifyCase.Dto;

public class PaymentAccount
{
    /// <summary>
    /// Prefix čísla úvěrového účtu, podle kterého se má vyhledávat
    /// </summary>
    /// <example>43</example>
    [MaxLength(6)]
    public string? Prefix { get; set; }

    /// <summary>
    /// Core čísla úvěrového účtu, podle kterého se má vyhledávat
    /// </summary>
    /// <example>9105464622</example>
    [MinLength(3)]
    [MaxLength(10)]
    public string Number { get; set; } = null!;
}