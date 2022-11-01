using DomainServices.CaseService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices;

internal class AggregatedData
{
    public SalesArrangement SalesArrangement { get; set; }

    public Case Case { get; set; }

    public GetMortgageOfferDetailResponse Offer { get; set; }

    public User User { get; set; }

    public DateTime CurrentDateTime => DateTime.Now;
}