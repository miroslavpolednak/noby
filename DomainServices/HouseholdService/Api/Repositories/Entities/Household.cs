using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.HouseholdService.Api.Repositories.Entities;

[Table("Household", Schema = "dbo")]
internal class Household : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int HouseholdId { get; set; }
    
    public long CaseId { get; set; }
    public int SalesArrangementId { get; set; }
    
    public CIS.Foms.Enums.HouseholdTypes HouseholdTypeId { get; set; }
    
    public int? ChildrenUpToTenYearsCount { get; set; }
    public int? ChildrenOverTenYearsCount { get; set; }
    public int? PropertySettlementId { get; set; }

    public bool? AreBothPartnersDeptors { get; set; }
    public bool? AreCustomersPartners { get; set; }

    public int? SavingExpenseAmount { get; set; }
    public int? InsuranceExpenseAmount { get; set; }
    public int? HousingExpenseAmount { get; set; }
    public int? OtherExpenseAmount { get; set; }
    
    public int? CustomerOnSAId1 { get; set; }
    public int? CustomerOnSAId2 { get; set; }
}