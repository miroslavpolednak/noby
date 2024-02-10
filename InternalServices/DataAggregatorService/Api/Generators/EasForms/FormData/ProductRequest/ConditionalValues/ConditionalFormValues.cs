using DomainServices.OfferService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.ProductRequest.ConditionalValues;

internal class ConditionalFormValues
{
    private readonly SpecificJsonKeys _specificJsonKeys;
    private readonly AggregatedData _data;

    public ConditionalFormValues(SpecificJsonKeys specificJsonKeys, AggregatedData data)
    {
        _specificJsonKeys = specificJsonKeys;
        _data = data;
    }

    public int? DeveloperId => _specificJsonKeys.GetValueOrDefault(JsonKey.DeveloperId, NullIfZero(_data.Offer.MortgageOffer.SimulationInputs.Developer?.DeveloperId));

    public int? DeveloperProjectId => _specificJsonKeys.GetValueOrDefault(JsonKey.DeveloperProjektId, NullIfZero(_data.Offer.MortgageOffer.SimulationInputs.Developer?.ProjectId));

    public string? DeveloperDescription => _specificJsonKeys.GetValueOrDefault(JsonKey.DeveloperPopis, _data.Offer.MortgageOffer.SimulationInputs.Developer?.Description);

    public IEnumerable<LoanPurpose> LoanPurposes =>
        _specificJsonKeys.GetValueOrDefault(JsonKey.SeznamUcelu, _data.Offer.MortgageOffer.SimulationInputs.LoanPurposes) ?? Enumerable.Empty<LoanPurpose>();

    public decimal? FinancialResourcesOwn => _specificJsonKeys.GetValueOrDefault(JsonKey.FinKrytiVlastniZdroje, _data.Offer.MortgageOffer.BasicParameters.FinancialResourcesOwn);

    public decimal? FinancialResourcesOther => _specificJsonKeys.GetValueOrDefault(JsonKey.FinKrytiCiziZdroje, _data.Offer.MortgageOffer.BasicParameters.FinancialResourcesOther);

    public decimal? FinancialResourcesTotal =>
        _specificJsonKeys.GetValueOrDefault(JsonKey.FinKrytiCelkem, _data.Offer.MortgageOffer.SimulationResults.LoanAmount + (FinancialResourcesOwn ?? 0) + (FinancialResourcesOther ?? 0));

    public decimal? InsuranceSumRealEstate =>
        _specificJsonKeys.GetValueOrDefault(JsonKey.PojisteniNemSuma,
                                           _data.Offer.MortgageOffer.SimulationInputs.RealEstateInsurance == null ? null : (decimal?)_data.Offer.MortgageOffer.SimulationInputs.RealEstateInsurance.Sum);

    public IEnumerable<LoanRealEstate> LoanRealEstates =>
        _specificJsonKeys.GetValueOrDefault(JsonKey.SeznamObjektu, _data.SalesArrangement.Mortgage?.LoanRealEstates)
                        ?.Select((loanRealEstate, index) => new LoanRealEstate
                        {
                            RowNumber = index + 1,
                            LoanRealEstateData = loanRealEstate
                        }) ?? Enumerable.Empty<LoanRealEstate>();
    
    private static int? NullIfZero(int? value) => value == 0 ? null : value;
}