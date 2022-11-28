namespace DomainServices.DocumentArchiveService.Api.Endpoints.UploadDocument
{
    public class UploadDocumentMediatrRequestHandler : IRequestHandler<UploadDocumentMediatrRequest>
    {
        public async Task<Unit> Handle(UploadDocumentMediatrRequest request, CancellationToken cancellationToken)
        {
            // decimal test = request.Request.Metadata.TestNumber;
            DateTime testtime = request.Request.Metadata.Testtime.ToDateTime();
            return Unit.Value;
        }
    }
}
