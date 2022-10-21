using CIS.InternalServices.DocumentDataAggregator.DataServices;

namespace CIS.InternalServices.DocumentDataAggregator;

internal class DataAggregator
{
    private readonly OfferDataService _offerDataService;
    private readonly UserDataService _userDataService;

    public DataAggregator(OfferDataService offerDataService, UserDataService userDataService)
    {
        _offerDataService = offerDataService;
        _userDataService = userDataService;
    }

    public async Task<ICollection<KeyValuePair<string, object>>> GetDocumentData(int offerId)
    {
        var offer = await _offerDataService.LoadData(offerId);

        var user = await _userDataService.LoadData(offer.Created.UserId.Value);

        var data = new AggregatedData
        {
            Offer = offer,
            User = user
        };

        return Enumerable.Empty<KeyValuePair<string, object>>().ToList();
    }
}