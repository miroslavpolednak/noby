using NOBY.Api.Endpoints.Customer.ValidateContact.Dto;

namespace NOBY.Api.Endpoints.Customer.ValidateContact;

public sealed class ValidateContactResponse
{
    /// <summary>
    /// Je kontakt validni?
    /// </summary>
    public bool IsContactValid { get; set; } 
    
    /// <summary>
    /// Typ kontatku
    /// </summary>
    /// <example>1</example>
    public ContactType ContactType { get; set; }
    
    /// <summary>
    /// Je to mobilní číslo?
    /// </summary>
    public bool? IsMobile { get; set; }
}