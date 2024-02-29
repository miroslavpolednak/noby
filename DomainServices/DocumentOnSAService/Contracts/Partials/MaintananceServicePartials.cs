using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Contracts;

public partial class GetUpdateDocumentStatusIdsRequest
    : MediatR.IRequest<GetUpdateDocumentStatusIdsResponse>
{ }

public partial class GetCheckDocumentsArchivedIdsRequest
    : MediatR.IRequest<GetCheckDocumentsArchivedIdsResponse>
{ }

public partial class UpdateDocumentsIsArchivedRequest
    : MediatR.IRequest<Empty>
{ }