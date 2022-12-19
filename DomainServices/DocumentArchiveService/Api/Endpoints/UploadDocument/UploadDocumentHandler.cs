using DomainServices.DocumentArchiveService.Contracts;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.UploadDocument;

public class UploadDocumentHandler : IRequestHandler<UploadDocumentRequest>
{
    public async Task<Unit> Handle(UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        // decimal test = request.Request.Metadata.TestNumber;
        DateTime testtime = request.Metadata.Testtime.ToDateTime();
        return Unit.Value;
    }
}
