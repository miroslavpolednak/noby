using DomainServices.CodebookService.Abstraction;
using DomainServices.CaseService.Contracts;
using Grpc.Core;
using CIS.Infrastructure.gRPC;

namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateActiveTasksHandler
    : IRequestHandler<Dto.UpdateActiveTasksMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{

    #region Construction

    private readonly ILogger<UpdateActiveTasksHandler> _logger;
    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly UserService.Clients.IUserServiceAbstraction _userService;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public UpdateActiveTasksHandler(
        ILogger<UpdateActiveTasksHandler> logger,
        Repositories.CaseServiceRepository repository,
        ICodebookServiceAbstraction codebookService,
        UserService.Clients.IUserServiceAbstraction userService,
        EasSimulationHT.IEasSimulationHTClient easSimulationHTClient,
        CIS.Core.Security.ICurrentUserAccessor userAccessor
        )
    {
        _logger = logger;
        _repository = repository;
        _codebookService = codebookService;
        _userService = userService;
        _easSimulationHTClient = easSimulationHTClient;
        _userAccessor = userAccessor;
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateActiveTasksMediatrRequest request, CancellationToken cancellation)
    {
        // check if case exists
        await _repository.EnsureExistingCase(request.CaseId, cancellation);

        // load WorkflowTaskTypes
        var taskTypes = await _codebookService.WorkflowTaskTypes(cancellation);
        var taskTypeIds = taskTypes.Select(i => i.Id).ToArray();

        // CheckTasks
        CheckTasks(request.Tasks, taskTypeIds);

        await _repository.ReplaceActiveTasks(request.CaseId, request.Tasks, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static void CheckTasks(ActiveTask[] tasks, int[] taskTypeIds)
    {
        var tasksWithInvalidTypeId = tasks.Where(t => !taskTypeIds.Contains(t.TypeId));

        if (!tasksWithInvalidTypeId.Any())
            return;

        var invalidTypeIds = tasksWithInvalidTypeId.Select(t => t.TypeId).Distinct();
        var taskIds = tasksWithInvalidTypeId.Select(t => t.TaskProcessId);

        throw new CisValidationException(13007, $"Found tasks [{String.Join(",", taskIds)}] with invalid TypeId [{String.Join(",", invalidTypeIds)}].");
    }

}
