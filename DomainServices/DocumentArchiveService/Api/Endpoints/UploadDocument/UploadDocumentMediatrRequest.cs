using DomainServices.DocumentArchiveService.Contracts;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.UploadDocument
{
    public class UploadDocumentMediatrRequest:IRequest, CIS.Core.Validation.IValidatableRequest
    {
        public UploadDocumentMediatrRequest(UploadDocumentRequest request)
        {
            Request = request;
        }

        public UploadDocumentRequest Request { get; }
    }
}
