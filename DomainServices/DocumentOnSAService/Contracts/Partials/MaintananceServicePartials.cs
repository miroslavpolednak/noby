namespace DomainServices.DocumentOnSAService.Contracts;

public partial class GetUpdateDocumentStatusIdsRequest
    : MediatR.IRequest<GetUpdateDocumentStatusIdsResponse>
{ }

public partial class GetCheckDocumentsArchivedIdsRequest
    : MediatR.IRequest<GetCheckDocumentsArchivedIdsResponse>
{ }