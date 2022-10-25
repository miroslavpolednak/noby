using CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.TemplateData;

internal class OfferTemplateData : AggregatedData
{
    public string Domicile
    {
        get
        {
            if (Offer.SimulationResults.LoanInterestRateAnnouncedType == 4)
                return "--";

            return Offer.AdditionalSimulationResults.MarketingActions.Any(x => x.Code == "DOMICILACE" && x.Applied == 1) ? "Test" : "nesjednána";
        }
    }
}