using DomainServices.OfferService.Contracts;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;

internal class AggregatedData
{
    public GetMortgageOfferDetailResponse Offer { get; set; }

    public OfferCustomData OfferCustom { get; set; }

    public User User { get; set; }
}