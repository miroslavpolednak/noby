using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Handlers;

internal class GetTaskListHandler
    : IRequestHandler<Dto.GetTaskListMediatrRequest, Contracts.GetTaskListResponse>
{
    public async Task<Contracts.GetTaskListResponse> Handle(Dto.GetTaskListMediatrRequest request, CancellationToken cancellation)
    {
        //MOCK
        var response = new GetTaskListResponse();
        response.Tasks.Add(new WorkflowTask
        {
            StateId = 1,
            CreatedOn = DateTime.Now,
            Name = "Testovaci task",
            TaskId = 1,
            TypeId = 1
        });
        return response;
    }
}
