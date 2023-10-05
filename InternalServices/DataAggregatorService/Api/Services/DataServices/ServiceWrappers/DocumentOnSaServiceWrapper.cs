using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using DomainServices.DocumentOnSAService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class DocumentOnSaServiceWrapper : IServiceWrapper
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;

    public DocumentOnSaServiceWrapper(IDocumentOnSAServiceClient documentOnSAService)
    {
        _documentOnSAService = documentOnSAService;
    }

    public DataService DataService => DataService.DocumentOnSa;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        var response = await _documentOnSAService.GetDocumentsOnSAList(input.SalesArrangementId!.Value, cancellationToken);

        data.Custom.DocumentOnSa = new DocumentOnSaInfo(response.DocumentsOnSA);
    }
}