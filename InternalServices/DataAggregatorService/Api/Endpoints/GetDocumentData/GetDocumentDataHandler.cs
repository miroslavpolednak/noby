using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents;

namespace CIS.InternalServices.DataAggregatorService.Api.Endpoints.GetDocumentData;

internal class GetDocumentDataHandler : IRequestHandler<GetDocumentDataRequest, GetDocumentDataResponse>
{
    private readonly Configuration.ConfigurationManager _configurationManager;
    private readonly DataServicesLoader _dataServicesLoader;
    private readonly DocumentDataFactory _documentDataFactory;

    public GetDocumentDataHandler(Configuration.ConfigurationManager configurationManager, DataServicesLoader dataServicesLoader, DocumentDataFactory documentDataFactory)
    {
        _configurationManager = configurationManager;
        _dataServicesLoader = dataServicesLoader;
        _documentDataFactory = documentDataFactory;
    }

    public async Task<GetDocumentDataResponse> Handle(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        var config = await _configurationManager.LoadDocumentConfiguration(request.DocumentTypeId, request.DocumentTemplateVersionId, cancellationToken);

        var documentMapper = await LoadDocumentData((DocumentType)request.DocumentTypeId, request.InputParameters, config);

        return new GetDocumentDataResponse
        {
            DocumentTemplateVersionId = config.DocumentTemplateVersionId,
            DocumentTemplateVersion = config.DocumentTemplateVersion,
            DocumentData = { documentMapper.MapDocumentFieldData() },
            InputParameters = request.InputParameters
        };
    }

    private async Task<DocumentMapper> LoadDocumentData(DocumentType documentType, InputParameters inputParameters, DocumentConfiguration config)
    {
        var documentData = _documentDataFactory.Create(documentType);

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, documentData);

        return new DocumentMapper(config, documentData);
    }
}