using DomainServices.CaseService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.CaseService.Api.Endpoints.UpdateTask;

public class UpdateTaskHandler : IRequestHandler<UpdateTaskRequest, Empty>
{
    public Task<Empty> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
