using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("CustomerIncome", Schema = "dbo")]
internal class CustomerIncome 
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerIncomeId { get; set; }

    public int CustomerOnSAId { get; set; }
    public CIS.Foms.Enums.CustomerIncomeTypes IncomeTypeId { get; set; }
    public int? Sum { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Data { get; set; }
}
