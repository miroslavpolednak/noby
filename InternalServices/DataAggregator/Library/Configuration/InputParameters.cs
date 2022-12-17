using CIS.Infrastructure.gRPC.CisTypes;

namespace CIS.InternalServices.DataAggregator.Configuration;

public class InputParameters
{
    public int? SalesArrangementId { get; set; }

    public long? CaseId { get; set; }

    public int? OfferId { get; set; }

    public int? UserId { get; set; }

    public Identity? CustomerIdentity { get; set; }
}