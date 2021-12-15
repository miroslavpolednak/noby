using Dapper.Contrib.Extensions;

namespace DomainServices.CaseService.Api.Dto;

[Table("CaseInstance")]
internal class CreateCaseModel : CIS.Core.Data.BaseInsertable
{
    [ExplicitKey]
    public long CaseId { get; set; }

    public int ProductInstanceType { get; set; }
    
    public int State { get; set; }
    
    public int PartyId { get; set; }
    
    public CIS.Core.IdentitySchemes CustomerIdentityScheme { get; set; }

    public int? CustomerIdentityId { get; set; }
    
    public string? FirstNameNaturalPerson { get; set; }
    
    public string? Name { get; set; }

    public DateTime? DateOfBirthNaturalPerson { get; set; }
}
