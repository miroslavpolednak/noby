using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.Shared;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData;

[TransientService, SelfService]
internal class ApplicationTerminationTemplateData : AggregatedData
{
    private readonly CaseServiceWrapper _caseServiceWrapper;
    private readonly SalesArrangementServiceWrapper _salesArrangementServiceWrapper;

    public ApplicationTerminationTemplateData(CaseServiceWrapper caseServiceWrapper, SalesArrangementServiceWrapper salesArrangementServiceWrapper)
    {
        _caseServiceWrapper = caseServiceWrapper;
        _salesArrangementServiceWrapper = salesArrangementServiceWrapper;
    }

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

    public string AnnouncementText
    {
        get
        {
            if (Custom.DocumentOnSa.DocumentsOnSa.Any(d => d.IsSigned))
            {
                
                return $"dovolujeme si Vás informovat, že žádost o poskytnutí úvěru ve výši {((decimal)Case.Data.TargetAmount).ToString("C0", CultureProvider.GetProvider())} " +
                       $"ze dne {((DateTime)SalesArrangement.Created.DateTime).ToString("d", CultureProvider.GetProvider())}, vedená pod registračním číslem {Case.Data.ContractNumber} byla ukončena na základě Vaší žádosti.";
            }

            return $"dovolujeme si Vás informovat, že na základě Vašeho požadavku bylo jednání o žádosti o poskytnutí úvěru ve výši {((decimal)Case.Data.TargetAmount).ToString("C0", CultureProvider.GetProvider())} ukončeno.";
        }
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore();
    }

    public override async Task LoadAdditionalData(InputParameters parameters, CancellationToken cancellationToken)
    {
        await _caseServiceWrapper.LoadData(parameters, this, cancellationToken);
        await _salesArrangementServiceWrapper.LoadData(parameters, this, cancellationToken);
    }

    private GrpcAddress GetPermanentAddress() => Customer.Addresses.First(a => a.AddressTypeId == (int)AddressTypes.Permanent);
}