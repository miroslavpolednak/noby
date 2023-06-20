using NOBY.Api.Endpoints.Customer.ValidateContact.Dto;

namespace NOBY.Api.Endpoints.Customer.ValidateContact;

public sealed class ValidateContactResponse
{
    /// <summary>
    /// Je kontakt validni?
    /// </summary>
    public bool IsContactValid { get; set; } 
    
    public ContactType ContactType { get; set; }
    
    /// <summary>
    /// Je to mobilní číslo?
    /// </summary>
    public bool? IsMobile { get; set; }
}