using NOBY.Api.Endpoints.Document.Shared;

namespace NOBY.Api.Endpoints.Document.GeneralDocument;

internal sealed class GetGeneralDocumentHandler : IRequestHandler<GetGeneralDocumentRequest, ReadOnlyMemory<byte>>
{
    private readonly DocumentGenerator _documentGenerator;

    public GetGeneralDocumentHandler(DocumentGenerator documentGenerator)
    {
        _documentGenerator = documentGenerator;
    }

    public async Task<ReadOnlyMemory<byte>> Handle(GetGeneralDocumentRequest request, CancellationToken cancellationToken)
    {
        var generateDocumentRequest = await _documentGenerator.CreateRequest(request, cancellationToken);

        return await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);
    }
}