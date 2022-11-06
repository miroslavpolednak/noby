using System.Globalization;
using CIS.InternalServices.DocumentDataAggregator.DataServices;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.TemplateData;

internal class OfferTemplateData : AggregatedData
{
    public string OfferHeader1 => Offer.SimulationInputs.LoanKindId == 2001 ? string.Empty : "Nabídka – test1".ToUpperInvariant();

    public string OfferHeader2 => Offer.SimulationInputs.LoanKindId != 2001 ? string.Empty : "Nabídka – test2".ToUpperInvariant();

    public string OfferHeader3 => "Nabídka – test3".ToUpperInvariant();

    public string PersonName => $"{Case.Customer.FirstNameNaturalPerson} {Case.Customer.Name}";

    public string PersonContact
    {
        get
        {
            var phone = string.IsNullOrWhiteSpace(Case.OfferContacts.PhoneNumberForOffer) ? null : Case.OfferContacts.PhoneNumberForOffer;
            var email = string.IsNullOrWhiteSpace(Case.OfferContacts.EmailForOffer) ? null : Case.OfferContacts.EmailForOffer;

            return string.Join('|', new[] { phone, email }.Where(str => !string.IsNullOrWhiteSpace(str)));
        }
    }

    public string LoanPurposes => string.Join("; ", Offer.SimulationInputs.LoanPurposes.Select(x => x.LoanPurposeId));

    public string FeeNames => string.Join(Environment.NewLine, Offer.AdditionalSimulationResults.Fees.Select(f => f.Name));

    public string FeeFinalSums
    {
        get
        {
            var numberProvider = (NumberFormatInfo)CultureInfo.GetCultureInfo("cs").NumberFormat.Clone();
            numberProvider.CurrencySymbol = "Kč";

            return string.Join(Environment.NewLine,
                               Offer.AdditionalSimulationResults
                                    .Fees
                                    .Select(f => (decimal?)f.FinalSum ?? 0m)
                                    .Select(f => f.ToString("C2", numberProvider)));
        }
    }
}