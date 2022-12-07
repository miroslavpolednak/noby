using DomainServices.DocumentArchiveService.Contracts;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocumentList
{
    public class GetDocumentListMediatrRequest : IRequest<GetDocumentListResponse>, CIS.Core.Validation.IValidatableRequest
    {
        public GetDocumentListMediatrRequest(GetDocumentListRequest request)
        {
            Request = request;
        }

        public GetDocumentListRequest Request { get; }
    }
}
