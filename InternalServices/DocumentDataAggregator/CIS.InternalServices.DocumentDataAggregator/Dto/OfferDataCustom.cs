using DomainServices.OfferService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.Dto;

internal class OfferDataCustom
{
    private readonly GetMortgageOfferDetailResponse _offerData;

    public OfferDataCustom(GetMortgageOfferDetailResponse offerData)
    {
        _offerData = offerData;
    }

    public IEnumerable<string> FeeNames => _offerData.AdditionalSimulationResults.Fees.Select(f => f.Name);

    public IEnumerable<decimal?> FeeFinalSums => _offerData.AdditionalSimulationResults.Fees.Select(f => (decimal?)f.FinalSum);
}