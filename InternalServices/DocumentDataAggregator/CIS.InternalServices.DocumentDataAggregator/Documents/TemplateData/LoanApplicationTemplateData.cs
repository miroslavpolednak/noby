using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DocumentDataAggregator.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.Countries;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.TemplateData;

internal class LoanApplicationTemplateData : AggregatedData
{
    private List<CountriesItem> _countries = null!;
    
    public string PersonName => $"{Customer.NaturalPerson.FirstName} {Customer.NaturalPerson.LastName}";

    public string PersonAddress =>
        Customer.Addresses
                .Where(c => c.AddressTypeId == (int)AddressTypes.Permanent)
                .DefaultIfEmpty(new GrpcAddress())
                .Select(a => $"{a.Street} {a.HouseNumber}/{a.StreetNumber}, " +
                             $"{a.Postcode} {a.City}, " +
                             $"{_countries.First(c => c.Id == a.CountryId).LongName}")
                .First();

    public string PaymentAccount
    {
        get
        {
            var bankAccount = $"{Mortgage.PaymentAccount.Prefix}-{Mortgage.PaymentAccount.Number}/{Mortgage.PaymentAccount.BankCode}";

            if (SalesArrangement.Drawing.IsImmediateDrawing)
                return bankAccount + " a to bezokladně";

            return bankAccount + $" a to k datu: {SalesArrangement.Drawing.DrawingDate}";
        }
    }

    public string RepaymentAccount
    {
        get
        {
            if (SalesArrangement.Drawing.RepaymentAccount is null)
                return string.Empty;

            var account = SalesArrangement.Drawing.RepaymentAccount;

            return $"{account.Prefix}-{account.Number}/{account.BankCode}";
        }
    }

    public override async Task LoadCodebooks(ICodebookServiceClients codebookService)
    {
        _countries = await codebookService.Countries();
    }
}