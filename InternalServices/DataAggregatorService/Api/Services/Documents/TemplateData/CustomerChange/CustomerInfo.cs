using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.Countries;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.CustomerChange;

internal class CustomerInfo
{
    private readonly CustomerDetailResponse _customer;
    private readonly ICollection<GenericCodebookItem> _degreesBefore;
    private readonly ICollection<CountriesItem> _countries;

    public CustomerInfo(CustomerDetailResponse customer, ICollection<GenericCodebookItem> degreesBefore, ICollection<CountriesItem> countries)
    {
        _customer = customer;
        _degreesBefore = degreesBefore;
        _countries = countries;
    }

    public string FullName => CustomerHelper.FullName(_customer, _degreesBefore);

    public string Address => CustomerHelper.FullAddress(_customer, _countries);
}