using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.CreateTask;

internal sealed class CreateTaskHandler
    : IRequestHandler<CreateTaskRequest, CreateTaskResponse>
{
    public async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellation)
    {
        return null;
    }

    public CreateTaskHandler()
    {

    }
}
