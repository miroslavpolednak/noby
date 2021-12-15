using Dapper.Contrib.Extensions;

namespace DomainServices.SalesArrangementService.Api.Dto;

[Table("SalesArrangementInstance")]
internal class CreateSalesArrangementModel : CIS.Core.Data.BaseInsertable
{
    [Key]
    public int SalesArrangementId { get; set; }

    public long CaseId { get; set; }

    public long? ProductInstanceId { get; set; }

    public int SalesArrangementType { get; set; }
}
