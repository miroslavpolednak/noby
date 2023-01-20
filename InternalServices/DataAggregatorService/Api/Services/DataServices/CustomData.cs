using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.Dto;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

internal class CustomData
{
    private readonly AggregatedData _data;

    public CustomData(AggregatedData data)
    {
        _data = data;
    }

    public IEnumerable<SalesArrangementPayout> SalesArrangementPayoutList =>
        _data.SalesArrangement.Drawing.PayoutList.Select(p => new SalesArrangementPayout(p));
}