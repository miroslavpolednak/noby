using CIS.InternalServices.DocumentDataAggregator.DataServices;
using DomainServices.OfferService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms.FormData.ProductRequest.ConditionalValues;

internal class ConditionalFormValues
{
    private readonly SpecificJsonKeys _specificJsonKeys;
    private readonly AggregatedData _data;

    public ConditionalFormValues(SpecificJsonKeys specificJsonKeys, AggregatedData data)
    {
        _specificJsonKeys = specificJsonKeys;
        _data = data;
    }

    public int? LoanKindId => _specificJsonKeys.GetValueOrNull(JsonKey.UvDruh, _data.Offer.SimulationInputs.LoanKindId);

    public int? DeveloperId => _specificJsonKeys.GetValueOrDefault(JsonKey.DeveloperId, NullIfZero(_data.Offer.SimulationInputs.Developer?.DeveloperId));

    public int? DeveloperProjectId => _specificJsonKeys.GetValueOrDefault(JsonKey.DeveloperProjektId, NullIfZero(_data.Offer.SimulationInputs.Developer?.ProjectId));

    public string? DeveloperDescription => _specificJsonKeys.GetValueOrDefault(JsonKey.DeveloperPopis, GetDeveloperDescription());

    public IEnumerable<LoanPurpose> LoanPurposes =>
        _specificJsonKeys.GetValueOrDefault(JsonKey.SeznamUcelu, _data.Offer.SimulationInputs.LoanPurposes) ?? Enumerable.Empty<LoanPurpose>();

    public decimal? FinancialResourcesOwn => _specificJsonKeys.GetValueOrDefault(JsonKey.FinKrytiVlastniZdroje, _data.Offer.BasicParameters.FinancialResourcesOwn);

    public decimal? FinancialResourcesOther => _specificJsonKeys.GetValueOrDefault(JsonKey.FinKrytiCiziZdroje, _data.Offer.BasicParameters.FinancialResourcesOther);

    public decimal? FinancialResourcesTotal =>
        _specificJsonKeys.GetValueOrDefault(JsonKey.FinKrytiCelkem, _data.Offer.SimulationResults.LoanAmount + (FinancialResourcesOwn ?? 0) + (FinancialResourcesOther ?? 0));

    public decimal? InsuranceSumRealEstate =>
        _specificJsonKeys.GetValueOrDefault(JsonKey.PojisteniNemSuma,
                                           _data.Offer.SimulationInputs.RealEstateInsurance == null ? null : (decimal?)_data.Offer.SimulationInputs.RealEstateInsurance.Sum);

    public IEnumerable<LoanRealEstate> LoanRealEstates =>
        _specificJsonKeys.GetValueOrDefault(JsonKey.SeznamObjektu, _data.SalesArrangement.Mortgage?.LoanRealEstates)
                        ?.Select((loanRealEstate, index) => new LoanRealEstate
                        {
                            RowNumber = index + 1,
                            LoanRealEstateData = loanRealEstate
                        }) ?? Enumerable.Empty<LoanRealEstate>();

    private string? GetDeveloperDescription()
    {
        var dev = _data.Offer.SimulationInputs.Developer;

        if (dev is null)
            return null;

        var texts = new[] { dev.NewDeveloperName, dev.NewDeveloperCin, dev.NewDeveloperProjectName }
                    .Where(str => !string.IsNullOrWhiteSpace(str))
                    .ToList();

        return texts.Count == 0 ? default : string.Join(",", texts);
    }

    private static int? NullIfZero(int? value) => value == 0 ? null : value;
}