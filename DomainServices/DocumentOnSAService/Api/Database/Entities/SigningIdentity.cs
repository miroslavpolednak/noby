using CIS.Foms.Enums;

namespace DomainServices.DocumentOnSAService.Api.Database.Entities;

public class SigningIdentity
{
    public int Id { get; set; }

    public int DocumentOnSAId { get; set; }

    public DocumentOnSa DocumentOnSa { get; set; } = null!;

    public SigningIdentityJson SigningIdentityJson { get; set; } = null!;
}

public class SigningIdentityJson
{
    public List<CustomerIdentifier> CustomerIdentifiers { get; set; } = new();

    public int? CustomerOnSAId { get; set; }

    public string? SignatureDataCode { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public MobilePhone? MobilePhone { get; set; }

    public string? EmailAddress { get; set; }

}

public class CustomerIdentifier
{
    public long IdentityId { get; set; }

    public IdentitySchemes IdentityScheme { get; set; }
}

public class MobilePhone
{
    public string? PhoneNumber { get; set; }

    public string? PhoneIDC { get; set; }
}
