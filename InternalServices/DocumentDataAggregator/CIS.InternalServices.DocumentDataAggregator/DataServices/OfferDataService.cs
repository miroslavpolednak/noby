using CIS.Core.Results;
using CIS.InternalServices.DocumentDataAggregator.Dto;
using DomainServices.OfferService.Abstraction;
using DomainServices.OfferService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices;

internal class OfferDataService
{
    private readonly IOfferServiceAbstraction _offerService;

    public OfferDataService(IOfferServiceAbstraction offerService)
    {
        _offerService = offerService;
    }

    public async Task<OfferData> LoadData(int offerId)
    {
        var result = await _offerService.GetMortgageOfferDetail(offerId);

        var offerData = ServiceCallResult.ResolveAndThrowIfError<GetMortgageOfferDetailResponse>(result);

        return new OfferData
        {
            Offer = offerData,
            OfferCustom = new OfferDataCustom(offerData)
        };
    }
}