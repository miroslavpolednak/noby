using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;
using DomainServices.UserService.Clients;

namespace DomainServices.CaseService.Api.Endpoints.CreateTask;

internal sealed class CreateTaskHandler
    : IRequestHandler<CreateTaskRequest, CreateTaskResponse>
{
    public async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        Dictionary<string, string> metadata = new();
        metadata.Add(getTaskTypeKey(), request.TaskRequest);

        // subtype
        if (request.TaskTypeId == 3)
        {
            metadata.Add("ukol_konzultace_oblast", $"{request.TaskSubtypeId}");
        }

        // ID dokumentu
        if (request.TaskDocumentsId?.Any() ?? false)
        {
            metadata.Add("wfl_refobj_dokumenty", string.Join(",", request.TaskDocumentsId) + ",");
        }

        // get current user's login
        var userInstance = await _userService.GetUser(_userAccessor.User!.Id, cancellationToken);

        var result = await _sbWebApi.CreateTask(new ExternalServices.SbWebApi.Dto.CreateTask.CreateTaskRequest
        {
            ProcessId = request.ProcessId,
            TaskTypeId = request.TaskTypeId,
            Metadata = metadata,
            Login = userInstance.UserIdentifiers.FirstOrDefault()?.Identity ?? "anonymous",
        }, cancellationToken);

        return new CreateTaskResponse
        {
            TaskIdSB = result.TaskIdSB,
            TaskId = result.TaskId
        };

        string getTaskTypeKey() => request.TaskTypeId switch
        {
            7 => "ukol_predanihs_pozadavek",
            3 => "ukol_konzultace_pozadavek",
            _ => throw new NotImplementedException($"TaskTypeId {request.TaskTypeId} is not supported")
        };
    }

    private readonly CaseServiceDbContext _dbContext;
    private readonly ISbWebApiClient _sbWebApi;
    private readonly IUserServiceClient _userService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public CreateTaskHandler(ISbWebApiClient sbWebApi, IUserServiceClient userService, CIS.Core.Security.ICurrentUserAccessor userAccessor, CaseServiceDbContext dbContext)
    {
        _userAccessor = userAccessor;
        _dbContext = dbContext;
        _userService = userService;
        _sbWebApi = sbWebApi;
    }
}
