namespace NOBY.Api.Endpoints.Cases.IdentifyCase.Dto;

public class PaymentAccount
{
    /// <summary>
    /// Prefix čísla úvěrového účtu, podle kterého se má vyhledávat
    /// </summary>
    /// <example>43</example>
    public string? Prefix { get; set; }

    /// <summary>
    /// Core čísla úvěrového účtu, podle kterého se má vyhledávat
    /// </summary>
    /// <example>9105464622</example>
    public string Number { get; set; } = null!;
}