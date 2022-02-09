using CIS.Core.Enums;
using DomainServices.DocumentService.Contracts;

namespace DomainServices.DocumentService.Api.Dto;
internal sealed class GetDocumentStatusMediatrRequest : IRequest<GetDocumentStatusResponse>, CIS.Core.Validation.IValidatableRequest
{
    public string DocumentId { get; init; }
    public IdentitySchemes Mandant { get; init; }


    public GetDocumentStatusMediatrRequest(GetDocumentStatusRequest request)
    {
        DocumentId = request.DocumentId;

        if (!Enum.TryParse<IdentitySchemes>(request.Mandant.ToString(), out IdentitySchemes parsedScheme))
            throw new CIS.Core.Exceptions.CisArgumentException(1, "Mandant is not in valid format", "Mandant");

        Mandant = parsedScheme;

        //Mandant = IdentitySchemes.Unknown; //request.Mandant;
    }
}
            