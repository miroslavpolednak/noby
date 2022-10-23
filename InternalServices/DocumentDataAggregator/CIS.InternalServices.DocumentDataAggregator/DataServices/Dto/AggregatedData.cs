using DomainServices.OfferService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;

internal class AggregatedData
{
    public GetMortgageOfferDetailResponse Offer { get; set; }

    public OfferCustomData OfferCustom { get; set; }
}