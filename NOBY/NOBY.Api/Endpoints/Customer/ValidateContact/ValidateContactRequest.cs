using System.ComponentModel.DataAnnotations;
using NOBY.Api.Endpoints.Customer.ValidateContact.Dto;

namespace NOBY.Api.Endpoints.Customer.ValidateContact;

public sealed class ValidateContactRequest : IRequest<ValidateContactResponse>
{
    /// <summary>
    /// telefonní číslo s mezinárodní předvolbou nebo email
    /// </summary>
    /// <example>+420 777 457 283</example>
    [Required]
    public string Contact { get; set; } = null!;
    
    /// <summary>
    /// Typ kontatku
    /// </summary>
    /// <example>1</example>
    [Required]
    public ContactType ContactType { get; set; }
}