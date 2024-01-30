﻿using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData;

internal class ApplicationTerminationTemplateData : AggregatedData
{
    public string FullName => CustomerHelper.FullName(Customer.Source, _codebookManager.DegreesBefore);

    public string Street
    {
        get
        {
            var address = GetPermanentAddress();
            
            var streetName = new[] { address.Street, address.CityDistrict, address.City }.First(str => !string.IsNullOrWhiteSpace(str));

            return $"{streetName} {string.Join("/", new[] { address.HouseNumber, address.StreetNumber }.Where(str => !string.IsNullOrWhiteSpace(str)))}";
        }
    }

    public string City
    {
        get
        {
            var address = GetPermanentAddress();

            return $"{address.Postcode}, {address.City}";
        }
    }

    public string AnnouncementText => "dovolujeme si Vás informovat, že na základě Vašeho požadavku byla Vaše žádost o poskytnutí úvěru " +
                                      $"č. {SalesArrangement.CaseId} ze dne {((DateTime)SalesArrangement.Created.DateTime).ToString("d", CultureProvider.GetProvider())} v našem systému ukončena.";

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore();
    }

    private GrpcAddress GetPermanentAddress() => Customer.Addresses.First(a => a.AddressTypeId == (int)AddressTypes.Permanent);
}