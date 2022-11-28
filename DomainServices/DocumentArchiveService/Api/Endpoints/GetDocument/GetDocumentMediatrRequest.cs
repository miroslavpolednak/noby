using DomainServices.DocumentArchiveService.Contracts;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocument
{
    public class GetDocumentMediatrRequest : IRequest<GetDocumentResponse>
    {
        public GetDocumentMediatrRequest(GetDocumentRequest request)
        {
            Request = request;
        }

        public GetDocumentRequest Request { get; }
    }
}
