using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using Codebook = DomainServices.CodebookService.Contracts.Endpoints;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class GeneralChangeTemplateData : AggregatedData
{
    private List<Codebook.Countries.CountriesItem> _countries = null!;
    private List<Codebook.RealEstateTypes.RealEstateTypeItem> _realEstateTypes = null!;
    private List<Codebook.RealEstatePurchaseTypes.RealEstatePurchaseTypeItem> _purchaseTypes = null!;

    protected SalesArrangementParametersGeneralChange GeneralChange => SalesArrangement.GeneralChange;

    public string PaymentAccount => Mortgage.PaymentAccount.Prefix + "-" + Mortgage.PaymentAccount.Number;

    public string FullName => Customer.NaturalPerson.FirstName + " " + Customer.NaturalPerson.LastName;

    public string PermanentAddress => FormatAddress(Customer.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Permanent));

    public string RepaymentAccount => GeneralChange.RepaymentAccount.Prefix + "-" + GeneralChange.RepaymentAccount.Number;

    public string RepaymentAccountOwner => $"{GeneralChange.RepaymentAccount.OwnerFirstName} {GeneralChange.RepaymentAccount.OwnerLastName}";

    public string RealEstateTypes => string.Join(", ", GetRealEstateTypes());

    public string RealEstatePurchaseTypes => string.Join(", ", GetRealEstatePurchaseTypes());

    public override async Task LoadCodebooks(ICodebookServiceClients codebookService, CancellationToken cancellationToken)
    {
        _countries = await codebookService.Countries(cancellationToken);
        _realEstateTypes = await codebookService.RealEstateTypes(cancellationToken);
        _purchaseTypes = await codebookService.RealEstatePurchaseTypes(cancellationToken);
    }

    private string FormatAddress(GrpcAddress? address)
    {
        if (address is null)
            return string.Empty;

        var countryName = _countries.First(c => c.Id == address.CountryId).LongName;

        return $"{address.Street} {address.HouseNumber}/{address.StreetNumber}, {address.Postcode} {address.City}, {countryName}";
    }

    private IEnumerable<string> GetRealEstateTypes() =>
        GeneralChange.LoanRealEstate
                     .LoanRealEstates
                     .Join(_realEstateTypes, x => x.RealEstateTypeId, y => y.Id, (_, y) => y.Name);

    private IEnumerable<string> GetRealEstatePurchaseTypes() =>
        GeneralChange.LoanRealEstate
                     .LoanRealEstates
                     .Join(_purchaseTypes, x => x.RealEstatePurchaseTypeId, y => y.Id, (_, y) => y.Name);
}