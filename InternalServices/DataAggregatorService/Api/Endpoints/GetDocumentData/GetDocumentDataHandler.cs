using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents;

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
        //TODO: mock
        if (request.DocumentTypeId is 4 or 5 && request.DocumentTemplateVariantId is null)
        {
            request.DocumentTemplateVariantId = request.DocumentTypeId switch
            {
                4 => 2,
                5 => 6
            };
        }

        var documentKey = new DocumentKey(request.DocumentTypeId, request.DocumentTemplateVersionId, request.DocumentTemplateVariantId);
        var config = await _configurationManager.LoadDocumentConfiguration(documentKey, cancellationToken);

        var documentMapper = await LoadDocumentData((DocumentType)request.DocumentTypeId, request.InputParameters, config, cancellationToken);

        return new GetDocumentDataResponse
        {
            DocumentTemplateVersionId = config.DocumentTemplateVersionId,
            DocumentData = { documentMapper.MapDocumentFieldData() },
            InputParameters = request.InputParameters
        };
    }

    private async Task<DocumentMapper> LoadDocumentData(DocumentType documentType, InputParameters inputParameters, DocumentConfiguration config, CancellationToken cancellationToken)
    {
        var documentData = _documentDataFactory.Create(documentType);

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, documentData, cancellationToken);

        return new DocumentMapper(config, documentData);
    }
}