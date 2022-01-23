using CIS.Core;
using DomainServices.DocumentService.Contracts;

namespace DomainServices.DocumentService.Api.Dto;
internal sealed class GetDocumentMediatrRequest : IRequest<GetDocumentResponse>, CIS.Core.Validation.IValidatableRequest
{
    public string DocumentId { get; init; }
    public IdentitySchemes Mandant { get; init; }

    //public GetDocumentMediatrRequest(string documentId, IdentitySchemes mandant)
    //{
    //    DocumentId = documentId;
    //    Mandant = mandant;
    //}

    public GetDocumentMediatrRequest(GetDocumentRequest request)
    {
        DocumentId = request.DocumentId;
        Mandant = IdentitySchemes.Unknown; //request.Mandant;
    }
}