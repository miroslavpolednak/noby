namespace DomainServices.DocumentArchiveService.Api.Endpoints.UploadDocument
{
    public class UploadDocumentMediatrRequestHandler : IRequestHandler<UploadDocumentMediatrRequest>
    {
        public async Task<Unit> Handle(UploadDocumentMediatrRequest request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}
