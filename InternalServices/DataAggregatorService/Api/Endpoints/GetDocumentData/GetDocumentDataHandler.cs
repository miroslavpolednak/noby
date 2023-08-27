﻿using CIS.InternalServices.DataAggregatorService.Api.Configuration;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

namespace CIS.InternalServices.DataAggregatorService.Api.Endpoints.GetDocumentData;

internal class GetDocumentDataHandler : IRequestHandler<GetDocumentDataRequest, GetDocumentDataResponse>
{
    private readonly IConfigurationManager _configurationManager;
    private readonly DataServicesLoader _dataServicesLoader;
    private readonly DocumentDataFactory _documentDataFactory;

    public GetDocumentDataHandler(IConfigurationManager configurationManager, DataServicesLoader dataServicesLoader, DocumentDataFactory documentDataFactory)
    {
        _configurationManager = configurationManager;
        _dataServicesLoader = dataServicesLoader;
        _documentDataFactory = documentDataFactory;
    }

    public async Task<GetDocumentDataResponse> Handle(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        var versionData = await _documentDataFactory.CreateVersionData((DocumentTypes)request.DocumentTypeId).GetDocumentVersionData(request, cancellationToken);

        var documentKey = new DocumentKey(request.DocumentTypeId, versionData);
        var config = await _configurationManager.LoadDocumentConfiguration(documentKey, cancellationToken);

        var documentMapper = await LoadDocumentData((DocumentTypes)request.DocumentTypeId, request.InputParameters, config, cancellationToken);

        return new GetDocumentDataResponse
        {
            DocumentTemplateVersionId = config.DocumentTemplateVersionId,
            DocumentTemplateVariantId = config.DocumentTemplateVariantId,
            DocumentData = { documentMapper.MapDocumentFieldData() },
            InputParameters = request.InputParameters
        };
    }

    private async Task<DocumentMapper> LoadDocumentData(DocumentTypes documentType, InputParameters inputParameters, DocumentConfiguration config, CancellationToken cancellationToken)
    {
        var documentData = _documentDataFactory.CreateData(documentType);

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, documentData, cancellationToken);

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (documentData.SalesArrangement is not null)
        {
            inputParameters.CaseId = documentData.SalesArrangement.CaseId;
        }

        return new DocumentMapper(config, documentData);
    }
}