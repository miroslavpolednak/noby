using NOBY.Api.Endpoints.Document.Shared;

namespace NOBY.Api.Endpoints.Document.Calculation;

internal class GetCalculationHandler : IRequestHandler<GetCalculationRequest, ReadOnlyMemory<byte>>
{
    private readonly DocumentGenerator _documentGenerator;

    public GetCalculationHandler(DocumentGenerator documentGenerator)
    {
        _documentGenerator = documentGenerator;
    }

    public async Task<ReadOnlyMemory<byte>> Handle(GetCalculationRequest request, CancellationToken cancellationToken)
    {
        var generateDocumentRequest = await _documentGenerator.CreateRequest(request);

        return await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);
    }
}