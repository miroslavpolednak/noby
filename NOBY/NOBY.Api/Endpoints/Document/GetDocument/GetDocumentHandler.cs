using System.Runtime.InteropServices;
using CIS.InternalServices.DataAggregator;
using CIS.InternalServices.DataAggregator.Documents;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using __Document = CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace NOBY.Api.Endpoints.Document.GetDocument;

internal class GetDocumentHandler : IRequestHandler<GetDocumentRequest, GetDocumentResponse>
{
    private readonly IDocumentGeneratorServiceClient _documentGenerator;
    private readonly IDataAggregator _dataAggregator;

    public GetDocumentHandler(IDocumentGeneratorServiceClient documentGenerator, IDataAggregator dataAggregator)
    {
        _documentGenerator = documentGenerator;
        _dataAggregator = dataAggregator;
    }

    public async Task<GetDocumentResponse> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        var result = await GenerateDocument(request, cancellationToken);

        if (!MemoryMarshal.TryGetArray(result.Data.Memory, out var arraySegment))
            throw new InvalidOperationException("Failed to get memory of document buffer");

        return new GetDocumentResponse
        {
            Buffer = arraySegment.Array!,
            CaseId = request.InputParameters.CaseId
        };
    }

    private async Task<__Document.Document> GenerateDocument(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        var documentGeneratorRequest = await CreateDocumentGeneratorRequest(request);

        return await _documentGenerator.GenerateDocument(documentGeneratorRequest, cancellationToken);
    }

    private async Task<__Document.GenerateDocumentRequest> CreateDocumentGeneratorRequest(GetDocumentRequest request)
    {
        var documentData = await _dataAggregator.GetDocumentData(request.TemplateType, request.TemplateVersion, request.InputParameters);

        return new __Document.GenerateDocumentRequest
        {
            TemplateTypeId = request.TemplateTypeId,
            TemplateVersion = request.TemplateVersion,
            OutputType = __Document.OutputFileType.OpenForm,
            Parts =
            {
                new __Document.GenerateDocumentPart
                {
                    TemplateTypeId = request.TemplateTypeId,
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
}