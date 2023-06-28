using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

internal class CustomData
{
    private readonly AggregatedData _data;

    public CustomData(AggregatedData data)
    {
        _data = data;
    }

    public DateTime CurrentDateTime => DateTime.Now;

    public IEnumerable<SalesArrangementPayout> SalesArrangementPayoutList =>
        _data.SalesArrangement.Drawing.PayoutList.Select(p => new SalesArrangementPayout(p));

    public DocumentOnSaInfo DocumentOnSa { get; set; } = null!;
}