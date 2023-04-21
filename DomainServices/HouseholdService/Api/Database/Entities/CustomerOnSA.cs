using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.HouseholdService.Api.Database.Entities;

[Table("CustomerOnSA", Schema = "dbo")]
internal sealed class CustomerOnSA
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerOnSAId { get; set; }

    public int SalesArrangementId { get; set; }

    public CIS.Foms.Enums.CustomerRoles CustomerRoleId { get; set; }

    public string? FirstNameNaturalPerson { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime? DateOfBirthNaturalPerson { get; set; }

    public DateTime? LockedIncomeDateTime { get; set; }

    public int? MaritalStatusId { get; set; }

    public byte[]? AdditionalDataBin { get; set; }

    public string? AdditionalData { get; set; }

    public string? ChangeData { get; set; }

    // kdyby me to nekdy v budoucnu napadlo - EF neumoznuje link na jinou entitu pro temporal tables
    public List<CustomerOnSAIdentity>? Identities { get; set; }
}