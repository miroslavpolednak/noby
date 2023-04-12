using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.GetTaskDetail;

internal class GetTaskDetailHandler : IRequestHandler<GetTaskDetailRequest, GetTaskDetailResponse>
{
    private readonly SbWebApiCommonDataProvider _commonDataProvider;
    private readonly ISbWebApiClient _sbWebApiClient;

    public GetTaskDetailHandler(SbWebApiCommonDataProvider commonDataProvider, ISbWebApiClient sbWebApiClient)
    {
        _commonDataProvider = commonDataProvider;
        _sbWebApiClient = sbWebApiClient;
    }

    public async Task<GetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var login = await _commonDataProvider.GetCurrentLogin(cancellationToken);

        var sbRequest = new FindByTaskIdRequest
        {
            HeaderLogin = login,
            TaskIdSb = request.TaskIdSb,
            TaskStates = await _commonDataProvider.GetValidTaskStateIds(cancellationToken)
        };

        var response = await _sbWebApiClient.FindTasksByTaskId(sbRequest, cancellationToken);

        if (response.ItemsFound == 0)
            throw new CisValidationException(13026, "Task ID not found in SB");

        return new GetTaskDetailResponse
        {
            TaskDetails =
            {
                response.Tasks.Select(taskData => new TaskDetailResponse
                {
                    TaskObject = taskData.ToWorkflowTask()
                })
            }
        };
    }
}