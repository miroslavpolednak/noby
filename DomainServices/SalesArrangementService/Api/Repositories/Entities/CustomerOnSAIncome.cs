using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("CustomerOnSAIncome", Schema = "dbo")]
internal class CustomerOnSAIncome 
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerOnSAIncomeId { get; set; }

    public int CustomerOnSAId { get; set; }
    
    public CIS.Foms.Enums.CustomerIncomeTypes IncomeTypeId { get; set; }
    
    public int? Sum { get; set; }
    
    public string? CurrencyCode { get; set; }
    
    public string? Data { get; set; }
    
    public byte[]? DataBin { get; set; }
}
