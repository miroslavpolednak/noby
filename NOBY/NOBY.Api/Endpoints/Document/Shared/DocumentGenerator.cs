using CIS.Core.Attributes;
using CIS.InternalServices.DataAggregator;
using CIS.InternalServices.DataAggregator.Documents;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using __Document = CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace NOBY.Api.Endpoints.Document.Shared;

[TransientService, SelfService]
internal class DocumentGenerator
{
    private readonly IDataAggregator _dataAggregator;
    private readonly IDocumentGeneratorServiceClient _documentGenerator;

    public DocumentGenerator(IDataAggregator dataAggregator, IDocumentGeneratorServiceClient documentGenerator)
    {
        _dataAggregator = dataAggregator;
        _documentGenerator = documentGenerator;
    }

    public async Task<__Document.GenerateDocumentRequest> CreateRequest(GetDocumentBaseRequest request)
    {
        var documentData = await _dataAggregator.GetDocumentData(request.TemplateType, request.TemplateVersion, request.InputParameters);

        return Create(request, documentData);
    }

    public async Task<ReadOnlyMemory<byte>> GenerateDocument(__Document.GenerateDocumentRequest generateDocumentRequest, CancellationToken cancellationToken)
    {
        var document = await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);

        return document.Data.Memory;
    }

    private static __Document.GenerateDocumentRequest Create(GetDocumentBaseRequest request, IEnumerable<DocumentFieldData> documentData) =>
        new()
        {
            TemplateTypeId = (int)request.TemplateType,
            TemplateVersion = request.TemplateVersion,
            OutputType = __Document.OutputFileType.OpenForm,
            Parts =
            {
                new __Document.GenerateDocumentPart
                {
                    TemplateTypeId = (int)request.TemplateType,
                    TemplateVersion = request.TemplateVersion
                }.FillDocumentPart(documentData)
            },
            DocumentFooter = new __Document.DocumentFooter
            {
                CaseId = request.InputParameters.CaseId,
                OfferId = request.InputParameters.OfferId
            }
        };
}