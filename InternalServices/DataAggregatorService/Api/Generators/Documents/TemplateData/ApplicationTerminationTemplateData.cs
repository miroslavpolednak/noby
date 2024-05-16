using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.Shared;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData;

[TransientService, SelfService]
internal class ApplicationTerminationTemplateData : AggregatedData
{
    private readonly CaseServiceWrapper _caseServiceWrapper;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public ApplicationTerminationTemplateData(CaseServiceWrapper caseServiceWrapper, ISalesArrangementServiceClient salesArrangementService)
    {
        _caseServiceWrapper = caseServiceWrapper;
        _salesArrangementService = salesArrangementService;
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
            if (!Custom.DocumentOnSa.DocumentsOnSa.Any(d => d.DocumentTypeId is (int)DocumentTypes.ZADOSTHU or (int)DocumentTypes.ZADOSTHD && d.IsSigned))
                return $"dovolujeme si Vás informovat, že na základě Vašeho požadavku bylo jednání o žádosti o poskytnutí úvěru ve výši {((decimal)Case.Data.TargetAmount).ToString("C0", CultureProvider.GetProvider())} ukončeno.";
            
            var signedDate = Custom.DocumentOnSa.DocumentsOnSa.Where(d => d.DocumentTypeId is (int)DocumentTypes.ZADOSTHU or (int)DocumentTypes.ZADOSTHD && d.IsSigned).Max(d => d.SignatureDateTime);

            return $"dovolujeme si Vás informovat, že žádost o poskytnutí úvěru ve výši {((decimal)Case.Data.TargetAmount).ToString("C0", CultureProvider.GetProvider())} " +
                   $"ze dne {(signedDate?.ToDateTime() ?? (DateTime)Case.Created.DateTime).ToString("d", CultureProvider.GetProvider())}, " +
                   $"vedená pod registračním číslem {Case.Data.ContractNumber} byla ukončena na základě Vaší žádosti.";

        }
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore();
    }

    public override async Task LoadAdditionalData(InputParameters parameters, CancellationToken cancellationToken)
    {
        var saValidationResult = await _salesArrangementService.ValidateSalesArrangementId(parameters.SalesArrangementId!.Value, true, cancellationToken);
        parameters.CaseId = saValidationResult.CaseId;

        await _caseServiceWrapper.LoadData(parameters, this, cancellationToken);
    }

    private GrpcAddress GetPermanentAddress() => Customer.Addresses.First(a => a.AddressTypeId == (int)AddressTypes.Permanent);
}