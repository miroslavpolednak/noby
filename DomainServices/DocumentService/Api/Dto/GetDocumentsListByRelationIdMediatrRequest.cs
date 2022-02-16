using CIS.Foms.Enums;
using DomainServices.DocumentService.Contracts;

namespace DomainServices.DocumentService.Api.Dto;
internal sealed class GetDocumentsListByRelationIdMediatrRequest : IRequest<GetDocumentsListResponse>, CIS.Core.Validation.IValidatableRequest
{
    public string RelationId { get; init; }
    public IdentitySchemes Mandant { get; init; }

    public GetDocumentsListByRelationIdMediatrRequest(GetDocumentsListByRelationIdRequest request)
    {
        RelationId = request.RelationId;
        Mandant = IdentitySchemes.Unknown; //request.Mandant;
    }
}