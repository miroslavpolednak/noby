using DomainServices.OfferService.Contracts;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator;

internal class AggregatedData
{
    public GetMortgageOfferDetailResponse Offer { get; set; }
    public User User { get; set; }
}