using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentArchiveService.Contracts;

public partial class DeleteDataFromArchiveQueueRequest
    : MediatR.IRequest<Empty>
{ }
