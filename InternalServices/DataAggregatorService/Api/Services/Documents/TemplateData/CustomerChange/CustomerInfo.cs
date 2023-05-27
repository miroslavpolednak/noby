using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.CustomerChange;

internal class CustomerInfo
{
    private readonly CustomerDetailResponse _customer;
    private readonly ICollection<GenericCodebookResponse.Types.GenericCodebookItem> _degreesBefore;
    private readonly ICollection<CountriesResponse.Types.CountryItem> _countries;

    public CustomerInfo(CustomerDetailResponse customer, ICollection<GenericCodebookResponse.Types.GenericCodebookItem> degreesBefore, ICollection<CountriesResponse.Types.CountryItem> countries)
    {
        _customer = customer;
        _degreesBefore = degreesBefore;
        _countries = countries;
    }

    public string FullName => CustomerHelper.FullName(_customer, _degreesBefore);

    public string Address => CustomerHelper.FullAddress(_customer, AddressTypes.Permanent, _countries);

    public string? BirthNumberText => string.IsNullOrWhiteSpace(_customer.NaturalPerson.BirthNumber) ? default : "Rodné číslo:";

    public string? BirthNumber => _customer.NaturalPerson.BirthNumber;

    public string? DateOfBirthText => string.IsNullOrWhiteSpace(_customer.NaturalPerson.BirthNumber) ? "Datum narození:" : default;

    public DateTime? DateOfBirth => string.IsNullOrWhiteSpace(_customer.NaturalPerson.BirthNumber) ? _customer.NaturalPerson.DateOfBirth : default;
}