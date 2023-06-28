using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.VersionData;

[TransientService, SelfService]
internal class CustomerChangeVersionDataProvider : DocumentVersionDataProviderBase
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public CustomerChangeVersionDataProvider(IDocumentVersionDataProvider documentVersionDataProvider, ISalesArrangementServiceClient salesArrangementService, ICodebookServiceClient codebookService) 
        : base(documentVersionDataProvider, codebookService)
    {
        _salesArrangementService = salesArrangementService;
    }

    protected override async Task<string> LoadVariantName(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        request.InputParameters.ValidateSalesArrangementId();

        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.InputParameters.SalesArrangementId!.Value, cancellationToken);

        return salesArrangement.CustomerChange.Applicants.Count switch
        {
            1 => "A",
            2 => "B",
            3 => "C",
            4 => "D",
            _ => throw new NotImplementedException()
        };
    }
}