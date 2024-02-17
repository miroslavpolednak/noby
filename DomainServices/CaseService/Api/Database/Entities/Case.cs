using DomainServices.CaseService.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CaseService.Api.Database.Entities;

[Table("Case", Schema = "dbo")]
internal sealed class Case 
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key]
    public long CaseId { get; set; }

    public int ProductTypeId { get; set; }
    public int State { get; set; }
    public DateTime StateUpdateTime { get; set; }
    public byte StateUpdatedInStarbuild { get; set; }

    // vlastnik case
    public int OwnerUserId { get; set; }
    public string? OwnerUserName { get; set; }

    // informace o klientovi
    public SharedTypes.Enums.IdentitySchemes? CustomerIdentityScheme { get; set; }
    public long? CustomerIdentityId { get; set; }
    public string? FirstNameNaturalPerson { get; set; }
    public string Name { get; set; } = "";
    public DateOnly? DateOfBirthNaturalPerson { get; set; }
    public string? Cin { get; set; }
    public string? EmailForOffer { get; set; }
    public string? PhoneNumberForOffer { get; set; }
    public string? PhoneIDCForOffer { get; set; }
    public decimal? CustomerPriceSensitivity { get; set; }
    public decimal? CustomerChurnRisk { get; set; }

    // byznys data
    public string? ContractNumber { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    [Precision(12, 2)]
    public decimal TargetAmount { get; set; }

    public bool? IsEmployeeBonusRequested { get; set; }

    public List<ActiveTask>? ActiveTasks { get; set; }
}
