using NOBY.Api.Endpoints.Document.Shared;

namespace NOBY.Api.Endpoints.Document.PaymentSchedule;

internal class GetPaymentScheduleHandler : IRequestHandler<GetPaymentScheduleRequest, ReadOnlyMemory<byte>>
{
    private readonly DocumentGenerator _documentGenerator;

    public GetPaymentScheduleHandler(DocumentGenerator documentGenerator)
    {
        _documentGenerator = documentGenerator;
    }

    public async Task<ReadOnlyMemory<byte>> Handle(GetPaymentScheduleRequest request, CancellationToken cancellationToken)
    {
        var generateDocumentRequest = await _documentGenerator.CreateRequest(request);

        return await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);
    }
}