using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using Codebook = DomainServices.CodebookService.Contracts.Endpoints;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class HUBNTemplateData : AggregatedData
{
    private List<Codebook.Countries.CountriesItem> _countries = null!;
    private List<Codebook.LoanPurposes.LoanPurposesItem> _loanPurposes = null!;
    private List<Codebook.RealEstateTypes.RealEstateTypeItem> _realEstateTypes = null!;
    private List<Codebook.RealEstatePurchaseTypes.RealEstatePurchaseTypeItem> _purchaseTypes = null!;

    public string PaymentAccount => Mortgage.PaymentAccount.Prefix + "-" + Mortgage.PaymentAccount.Number;

    public string FullName => Customer.NaturalPerson.FirstName + " " + Customer.NaturalPerson.LastName;

    public string PermanentAddress => FormatAddress(Customer.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Permanent));

    public IEnumerable<string> LoanPurposes => GetLoanPurposes();

    public IEnumerable<string> LoanRealEstates => GetLoanRealEstates();

    public override async Task LoadCodebooks(ICodebookServiceClients codebookService)
    {
        _countries = await codebookService.Countries();
        _loanPurposes = await codebookService.LoanPurposes();
        _realEstateTypes = await codebookService.RealEstateTypes();
        _purchaseTypes = await codebookService.RealEstatePurchaseTypes();
    }

    private string FormatAddress(GrpcAddress? address)
    {
        if (address is null)
            return string.Empty;

        var countryName = _countries.First(c => c.Id == address.CountryId).LongName;

        return $"{address.Street} {address.HouseNumber}/{address.StreetNumber}, {address.Postcode} {address.City}, {countryName}";
    }

    private IEnumerable<string> GetLoanPurposes() =>
        SalesArrangement.HUBN
                        .LoanPurposes
                        .Join(_loanPurposes, x => x.LoanPurposeId, y => y.Id, (_, y) => y.Name);

    private IEnumerable<string> GetLoanRealEstates()
    {
        var realEstates = from l in SalesArrangement.HUBN.LoanRealEstates
            join r in _realEstateTypes on l.RealEstateTypeId equals r.Id
            join p in _purchaseTypes on l.RealEstatePurchaseTypeId equals p.Id
            select new
            {
                LoanRealEstate = l,
                RealEstateTypeName = r.Name,
                PurchaseTypeName = p.Name
            };

        return realEstates.Select(r => r.LoanRealEstate.IsCollateral
                                      ? $"{r.RealEstateTypeName}, {r.PurchaseTypeName}, slouží k zajištění"
                                      : $"{r.RealEstateTypeName}, {r.PurchaseTypeName}");
    }
}