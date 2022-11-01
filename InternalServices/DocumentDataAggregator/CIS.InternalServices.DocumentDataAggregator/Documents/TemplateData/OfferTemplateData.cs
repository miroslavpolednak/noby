using CIS.InternalServices.DocumentDataAggregator.DataServices;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.TemplateData;

internal class OfferTemplateData : AggregatedData
{
    public string OfferHeader
    {
        get
        {
            return "Nabídka – test".ToUpperInvariant();

            if (Offer.SimulationInputs.LoanKindId == 2001)
            {

            }
            else
            {

            }
        }
    }

    public string PersonName => $"{Case.Customer.FirstNameNaturalPerson} {Case.Customer.Name}";

    public string PersonContact
    {
        get
        {

        }
    }

    public string FeeNames => string.Join(Environment.NewLine, Offer.AdditionalSimulationResults.Fees.Select(f => f.Name));

    public string FeeFinalSums => string.Join(Environment.NewLine, Offer.AdditionalSimulationResults.Fees.Select(f => f.FinalSum));
}