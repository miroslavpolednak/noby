using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class ApplicationTerminationTemplateData : AggregatedData
{
    public string FullName => CustomerHelper.FullName(Customer, _codebookManager.DegreesBefore);

    public string Street
    {
        get
        {
            var address = GetPermanentAddress();

            return $"{address.Street} {string.Join("/", new[] { address.HouseNumber, address.StreetNumber }.Where(str => !string.IsNullOrWhiteSpace(str)))}";
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
                                      $"č. {SalesArrangement.CaseId} ze dne {(DateTime)SalesArrangement.Created.DateTime} v našem systému ukončena.";

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore();
    }

    private GrpcAddress GetPermanentAddress() => Customer.Addresses.First(a => a.AddressTypeId == (int)AddressTypes.Permanent);
}