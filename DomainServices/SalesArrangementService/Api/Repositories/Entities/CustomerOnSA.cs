using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("CustomerOnSA", Schema = "dbo")]
internal class CustomerOnSA 
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerOnSAId { get; set; }
    
    public int SalesArrangementId { get; set; }
    
    public CIS.Foms.Enums.CustomerRoles CustomerRoleId { get; set; }

    public string? FirstNameNaturalPerson { get; set; } = null!;
    
    public string Name { get; set; }= null!;
    
    public DateTime? DateOfBirthNaturalPerson { get; set; }

    public DateTime? LockedIncomeDateTime { get; set; }

    // kdyby me to nekdy v budoucnu napadlo - EF neumoznuje linka na jinou entitu pro temporal tables
    public virtual List<CustomerOnSAIdentity>? Identities { get; set; } = null!;
}