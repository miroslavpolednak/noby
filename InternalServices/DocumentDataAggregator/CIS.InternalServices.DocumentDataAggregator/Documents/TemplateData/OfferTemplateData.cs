using System.Globalization;
using CIS.InternalServices.DocumentDataAggregator.DataServices;
using DomainServices.CodebookService.Abstraction;
using Codebook = DomainServices.CodebookService.Contracts.Endpoints;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.TemplateData;

internal class OfferTemplateData : AggregatedData
{
    private List<Codebook.ProductTypes.ProductTypeItem> _productTypes = null!;
    private List<Codebook.LoanKinds.LoanKindsItem> _loanKinds = null!;
    private List<Codebook.LoanPurposes.LoanPurposesItem> _loanPurposes = null!;

    public string OfferHeader1 => GetLoanKindOfferHeader().ToUpperInvariant();

    public string OfferHeader2 => GetProductTypeOfferHeader().ToUpperInvariant();

    public string OfferHeader3
    {
        get
        {
            var text = GetLoanKindOfferHeader();

            if (string.IsNullOrWhiteSpace(text))
                text = GetProductTypeOfferHeader();

            return text.ToUpperInvariant();
        }
    }

    public string PersonName => $"{Case.Customer.FirstNameNaturalPerson} {Case.Customer.Name}";

    public string PersonContact
    {
        get
        {
            var phone = string.IsNullOrWhiteSpace(Case.OfferContacts.PhoneNumberForOffer) ? null : $"telefon: {Case.OfferContacts.PhoneNumberForOffer}";
            var email = string.IsNullOrWhiteSpace(Case.OfferContacts.EmailForOffer) ? null : $"e-mail: {Case.OfferContacts.EmailForOffer}";

            return string.Join(" | ", new[] { phone, email }.Where(str => !string.IsNullOrWhiteSpace(str)));
        }
    }

    public string LoanPurposes => string.Join("; ",
                                              Offer.SimulationInputs
                                                   .LoanPurposes
                                                   .Select(x => _loanPurposes.Where(p => p.MandantId == 2 && p.Id == x.LoanPurposeId)
                                                                             .Select(p => p.Name)
                                                                             .FirstOrDefault()));

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

    public override async Task LoadCodebooks(ICodebookServiceAbstraction codebookService)
    {
        _productTypes = await codebookService.ProductTypes();
        _loanKinds = await codebookService.LoanKinds();
        _loanPurposes = await codebookService.LoanPurposes();
    }

    private string GetLoanKindOfferHeader()
    {
        if (Offer.SimulationInputs.LoanKindId == 2001)
            return string.Empty;

        var loanKind = _loanKinds.Where(x => x.MandantId == 2 && x.Id == Offer.SimulationInputs.LoanKindId)
                                 .Select(x => x.Name.ToUpperInvariant())
                                 .FirstOrDefault();

        return $"NABÍDKA – {loanKind}";
    }

    private string GetProductTypeOfferHeader()
    {
        if (Offer.SimulationInputs.LoanKindId != 2001)
            return string.Empty;

        var productType = _productTypes.Where(x => x.MandantId == 2 && x.Id == Offer.SimulationInputs.ProductTypeId)
                                       .Select(x => x.Name.ToUpperInvariant())
                                       .FirstOrDefault();

        return $"NABÍDKA – {productType}";
    }
}