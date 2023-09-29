﻿using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.Shared;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.CustomerChange;

internal class CustomerInfo
{
    private readonly CustomerDetailResponse _customer;
    private readonly ICollection<GenericCodebookResponse.Types.GenericCodebookItem> _degreesBefore;

    public CustomerInfo(CustomerDetailResponse customer, ICollection<GenericCodebookResponse.Types.GenericCodebookItem> degreesBefore)
    {
        _customer = customer;
        _degreesBefore = degreesBefore;
    }

    public string FullName => CustomerHelper.FullName(_customer, _degreesBefore);

    public string Address => CustomerHelper.FullAddress(_customer, AddressTypes.Permanent);

    public string? BirthNumberText => string.IsNullOrWhiteSpace(_customer.NaturalPerson.BirthNumber) ? default : "Rodné číslo:";

    public string? BirthNumber => _customer.NaturalPerson.BirthNumber;

    public string? DateOfBirthText => string.IsNullOrWhiteSpace(_customer.NaturalPerson.BirthNumber) ? "Datum narození:" : default;

    public DateTime? DateOfBirth => string.IsNullOrWhiteSpace(_customer.NaturalPerson.BirthNumber) ? _customer.NaturalPerson.DateOfBirth : default;
}