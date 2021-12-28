using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CaseService.Api.Repositories.Entities;

[Table("CaseInstance", Schema = "dbo")]
internal class CaseInstance : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key]
    public long CaseId { get; set; }

    public int ProductInstanceType { get; set; }

    public int State { get; set; }

    public int UserId { get; set; }

    public CIS.Core.IdentitySchemes? CustomerIdentityScheme { get; set; }

    public int? CustomerIdentityId { get; set; }

    public string? FirstNameNaturalPerson { get; set; }

    public string Name { get; set; } = "";

    public DateOnly? DateOfBirthNaturalPerson { get; set; }

    public string? ContractNumber { get; set; }

    public int? TargetAmount { get; set; }

    public bool IsActionRequired { get; set; }
}
