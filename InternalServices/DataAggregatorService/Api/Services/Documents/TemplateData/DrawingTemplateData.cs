using System.Globalization;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.Countries;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class DrawingTemplateData : AggregatedData
{
    private List<CountriesItem> _countries = null!;
    private List<GenericCodebookItem> _degreesBefore = null!;

    public string PersonName => GetFullName();

    public string PersonAddress =>
        Customer.Addresses
                .Where(c => c.AddressTypeId == (int)AddressTypes.Permanent)
                .Select(a => $"{a.Street} {string.Join("/", new[] { a.HouseNumber, a.StreetNumber }.Where(str => !string.IsNullOrWhiteSpace(str)))}, " +
                             $"{a.Postcode} {a.City}, " +
                             $"{_countries.First(c => c.Id == a.CountryId).LongName}")
                .FirstOrDefault(string.Empty);

    public string PaymentAccount
    {
        get
        {
            var bankAccount = $"{Mortgage.PaymentAccount.Prefix}-{Mortgage.PaymentAccount.Number}/{Mortgage.PaymentAccount.BankCode}";

            if (SalesArrangement.Drawing.IsImmediateDrawing)
                return bankAccount + " a to bezokladně.";

            return bankAccount + $" a to k datu: {((DateTime)SalesArrangement.Drawing.DrawingDate).ToString("d", CultureInfo.GetCultureInfo("cs"))}.";
        }
    }

    public string RepaymentAccount
    {
        get
        {
            if (SalesArrangement.Drawing.RepaymentAccount is null)
                return string.Empty;

            var account = SalesArrangement.Drawing.RepaymentAccount;

            return "Číslo účtu pro splácení úvěru: " + $"{account.Prefix}-{account.Number}/{account.BankCode}";
        }
    }

    public override async Task LoadCodebooks(ICodebookServiceClients codebookService, CancellationToken cancellationToken)
    {
        _countries = await codebookService.Countries(cancellationToken);
        _degreesBefore = await codebookService.AcademicDegreesBefore(cancellationToken);
    }

    private string GetFullName()
    {
        if (!Customer.NaturalPerson.DegreeBeforeId.HasValue)
            return $"{Customer.NaturalPerson.FirstName} {Customer.NaturalPerson.LastName}";

        var degree = _degreesBefore.First(d => d.Id == Customer.NaturalPerson.DegreeBeforeId.Value).Name;

        return $"{Customer.NaturalPerson.FirstName} {Customer.NaturalPerson.LastName}, {degree}";
    }
}