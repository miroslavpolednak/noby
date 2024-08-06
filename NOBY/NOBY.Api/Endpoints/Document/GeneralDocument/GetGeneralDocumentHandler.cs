using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Document.SharedDto;

namespace NOBY.Api.Endpoints.Document.GeneralDocument;

internal sealed class GetGeneralDocumentHandler(
    ISalesArrangementServiceClient _salesArrangementService, 
    DocumentGenerator _documentGenerator) 
    : IRequestHandler<GetGeneralDocumentRequest, ReadOnlyMemory<byte>>
{
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