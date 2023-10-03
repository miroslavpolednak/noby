using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Document.Shared;

namespace NOBY.Api.Endpoints.Document.GeneralDocument;

internal sealed class GetGeneralDocumentHandler : IRequestHandler<GetGeneralDocumentRequest, ReadOnlyMemory<byte>>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly DocumentGenerator _documentGenerator;

    public GetGeneralDocumentHandler(ISalesArrangementServiceClient salesArrangementService, DocumentGenerator documentGenerator)
    {
        _salesArrangementService = salesArrangementService;
        _documentGenerator = documentGenerator;
    }

    public async Task<ReadOnlyMemory<byte>> Handle(GetGeneralDocumentRequest request, CancellationToken cancellationToken)
    {
        if (request.InputParameters.SalesArrangementId.HasValue)
        {
            var saValidationResult = await _salesArrangementService.ValidateSalesArrangementId(request.InputParameters.SalesArrangementId.Value, false, cancellationToken);

            request.InputParameters.CaseId = saValidationResult.CaseId;
        }

        var generateDocumentRequest = await _documentGenerator.CreateRequest(request, cancellationToken);

        return await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);
    }
}