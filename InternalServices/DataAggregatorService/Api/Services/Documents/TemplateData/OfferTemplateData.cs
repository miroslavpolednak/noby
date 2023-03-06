using System.Globalization;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class OfferTemplateData : AggregatedData
{
    public string OfferHeader1 => GetLoanKindOfferHeader();

    public string OfferHeader2 => GetProductTypeOfferHeader();

    public string OfferHeader3
    {
        get
        {
            var text = GetLoanKindOfferHeader();

            if (string.IsNullOrWhiteSpace(text))
                text = GetProductTypeOfferHeader();

            return text;
        }
    }

    public string PersonName => $"{Case.Customer.FirstNameNaturalPerson} {Case.Customer.Name}";

    public string PersonContact
    {
        get
        {
            var phone = string.IsNullOrWhiteSpace(Case.OfferContacts.PhoneNumberForOffer?.PhoneNumber) ? null : $"telefon: {Case.OfferContacts.PhoneNumberForOffer?.PhoneIDC}{Case.OfferContacts.PhoneNumberForOffer?.PhoneNumber}";
            var email = string.IsNullOrWhiteSpace(Case.OfferContacts.EmailForOffer) ? null : $"e-mail: {Case.OfferContacts.EmailForOffer}";

            return string.Join(" | ", new[] { phone, email }.Where(str => !string.IsNullOrWhiteSpace(str)));
        }
    }

    public string LoanPurposes
    {
        get
        {
            if (Offer.SimulationInputs.LoanKindId == 2001)
                return "koupě/výstavba/rekonstrukce";

            return string.Join("; ",
                               Offer.SimulationInputs
                                    .LoanPurposes
                                    .Select(x => _codebookManager.LoanPurposes
                                                                 .Where(p => p.MandantId == 2 && p.Id == x.LoanPurposeId)
                                                                 .Select(p => p.Name)
                                                                 .FirstOrDefault()));
        }
    }

    public string FeeNames => string.Join(Environment.NewLine, Offer.AdditionalSimulationResults.Fees.Where(f => f.IncludeInRPSN).Select(f => f.Name));

    public string FeeFinalSums
    {
        get
        {
            var numberProvider = (NumberFormatInfo)CultureInfo.GetCultureInfo("cs").NumberFormat.Clone();
            numberProvider.CurrencySymbol = "Kč";

            return string.Join(Environment.NewLine,
                               Offer.AdditionalSimulationResults
                                    .Fees
                                    .Where(f => f.IncludeInRPSN)
                                    .Select(f => (decimal?)f.FinalSum ?? 0m)
                                    .Select(f => f.ToString("C2", numberProvider)));
        }
    }

    public string UserName => User.FullName;

    public string UserContacts
    {
        get
        {
            var phone = string.IsNullOrWhiteSpace(User.Phone) ? null : $"telefon: {User.Phone}";
            var email = string.IsNullOrWhiteSpace(User.Email) ? null : $"e-mail: {User.Email}";

            return string.Join(" | ", new[] { phone, email }.Where(str => !string.IsNullOrWhiteSpace(str)));
        }
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.ProductTypes().LoanKinds().LoanPurposes();
    }

    private string GetLoanKindOfferHeader()
    {
        if (Offer.SimulationInputs.LoanKindId == 2001)
            return string.Empty;

        return _codebookManager.ProductTypes
                               .Where(x => x.MandantId == 2 && x.Id == Offer.SimulationInputs.ProductTypeId)
                               .Select(x => x.Name.ToUpperInvariant())
                               .DefaultIfEmpty(string.Empty)
                               .First();
    }

    private string GetProductTypeOfferHeader()
    {
        if (Offer.SimulationInputs.LoanKindId != 2001)
            return string.Empty;

        return _codebookManager.LoanKinds.Where(x => x.MandantId == 2 && x.Id == Offer.SimulationInputs.LoanKindId)
                               .Select(x => x.Name.ToUpperInvariant())
                               .DefaultIfEmpty(string.Empty)
                               .First();
    }
}