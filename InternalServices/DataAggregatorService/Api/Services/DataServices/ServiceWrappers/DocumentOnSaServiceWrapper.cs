﻿using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class DocumentOnSaServiceWrapper : IServiceWrapper
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ICodebookServiceClient _codebookService;

    public DocumentOnSaServiceWrapper(IDocumentOnSAServiceClient documentOnSAService, ICodebookServiceClient codebookService)
    {
        _documentOnSAService = documentOnSAService;
        _codebookService = codebookService;
    }

    public DataSource DataSource => DataSource.DocumentOnSa;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        var response = await _documentOnSAService.GetDocumentsOnSAList(input.SalesArrangementId!.Value, cancellationToken);

        data.Custom.DocumentOnSa = new DocumentOnSaInfo(response.DocumentsOnSA);
    }
}