using CIS.Core;
using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients.Authorization;
using NOBY.Infrastructure.ErrorHandling;
using NOBY.Infrastructure.Security;
using NOBY.Services.WorkflowTask;
using WFL = DomainServices.CaseService.Contracts.WorkflowTask;

namespace NOBY.Services.MortgageRefinancing;

[ScopedService, SelfService]
public sealed class MortgageRefinancingWorkflowService(
    ICaseServiceClient _caseService, 
    ICurrentUserAccessor _currentUserAccessor, 
    ISalesArrangementServiceClient _salesArrangementService, 
    WorkflowMapper.IWorkflowMapperService _workflowMapper)
{
    public async Task<GetRefinancingDataResult> GetRefinancingData(long caseId, long? processId, RefinancingTypes refinancingType, CancellationToken cancellationToken)
    {
        GetRefinancingDataResult result = new();

        if (processId.HasValue)
        {
            // detail procesu
            var process = (await _caseService.GetProcessList(caseId, cancellationToken))
                .FirstOrDefault(p => p.ProcessId == processId)
                ?? throw new NobyValidationException(90043, $"ProccesId {processId} not found in list");

            // validace typu procesu
            int refType = refinancingType == RefinancingTypes.MortgageRetention ? 1 : 2;
            if (process.ProcessTypeId != 3 || process.RefinancingProcess?.RefinancingType != refType)
            {
                throw new NobyValidationException(90032, $"ProcessTypeId!=3 or RefinancingType!={refType}");
            }

            // zjistit refinancingState
            var (salesArrangement, refinancingState) = await getRefinancingStateId(caseId, process, cancellationToken);

            if (refinancingState is (RefinancingStates.Zruseno or RefinancingStates.Dokonceno))
            {
                throw new NobyValidationException(90032, $"RefinancingState is not allowed: {refinancingState}");
            }

            result.Process = process;
            result.SalesArrangement = salesArrangement;
            result.RefinancingState = refinancingState;
        }
        else
        {
            result.RefinancingState = RefinancingStates.Unknown;
        }

        // vsechny tasky z WF, potom vyfiltrovat jen na konkretni processId
        var tasks = (await _caseService.GetTaskList(caseId, cancellationToken))
            .Where(t => t.ProcessId == processId)
            .ToList();

        result.Tasks = (await tasks
                .SelectAsync(t => _workflowMapper.MapTask(t, cancellationToken)))
                .ToList();

        // toto je aktivni task!
        result.ActivePriceExceptionTaskIdSb = tasks
            .FirstOrDefault(t => t.TaskTypeId == (int)WorkflowTaskTypes.PriceException && !t.Cancelled && t.DecisionId != 2 && t.PhaseTypeId == 2)
            ?.TaskIdSb;

        return result;
    }

    public Task<WorkflowTaskByTaskId.WorkflowProcessByProcessIdResult> GetProcessInfoByProcessId(long caseId, long processId, CancellationToken cancellationToken) => 
        _caseService.GetProcessByProcessId(caseId, processId, cancellationToken);

    public async Task<MortgageRefinancingIndividualPrice> GetIndividualPrices(long caseId, long taskProcessId, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(caseId, cancellationToken);

        var priceExceptionTasks = GetPriceExceptionTasks(taskList, taskProcessId).ToList();

        if (priceExceptionTasks.Count == 0)
            throw new NobyValidationException(90032, "Empty collection");

        var priceExceptionTask = priceExceptionTasks.SingleOrDefault() ?? throw new NobyValidationException(90050);

        if (priceExceptionTask.StateIdSb != 30)
            throw new NobyValidationException(90049);

        if (priceExceptionTask.DecisionId != 1)
            throw new NobyValidationException(90032, "Not exist DecisionId == 1");

        var priceExceptionDetail = await _caseService.GetTaskDetail(priceExceptionTask.TaskIdSb, cancellationToken);

        return new MortgageRefinancingIndividualPrice(priceExceptionDetail.TaskDetail.PriceException);
    }

    public async Task CreateIndividualPriceWorkflowTask(MortgageRefinancingWorkflowParameters mortgageParameters, string? taskRequest, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(mortgageParameters.CaseId, cancellationToken);

        if (!await CancelExistingPriceExceptions(taskList, mortgageParameters, cancellationToken)
            || (mortgageParameters.LoanInterestRateDiscount is (null or 0) && (mortgageParameters.Fee?.FeeFinalSum ?? 0) == 0))
        {
            return;
        }

        ValidatePermission();

        var createTaskRequest = new CreateTaskRequest
        {
            CaseId = mortgageParameters.CaseId,
            TaskTypeId = (int)WorkflowTaskTypes.PriceException,
            ProcessId = mortgageParameters.TaskProcessId,
            TaskRequest = taskRequest,
            PriceException = new TaskPriceException
            {
                LoanInterestRate = new PriceExceptionLoanInterestRateItem
                {
                    LoanInterestRate = mortgageParameters.LoanInterestRate,
                    LoanInterestRateProvided = mortgageParameters.LoanInterestRate.HasValue ? mortgageParameters.LoanInterestRate - mortgageParameters.LoanInterestRateDiscount.GetValueOrDefault() : null,
                    LoanInterestRateDiscount = mortgageParameters.LoanInterestRateDiscount
                }
            }
        };

        if (mortgageParameters.Fee is not null)
        {
            createTaskRequest.PriceException.Fees.Add(new PriceExceptionFeesItem
            {
                TariffSum = mortgageParameters.Fee.FeeSum,
                FinalSum = mortgageParameters.Fee.FeeFinalSum,
                DiscountPercentage = 100 * (mortgageParameters.Fee.FeeSum - mortgageParameters.Fee.FeeFinalSum) / mortgageParameters.Fee.FeeSum
            });
        }

        await _caseService.CreateTask(createTaskRequest, cancellationToken);
    }

    private async Task<bool> CancelExistingPriceExceptions(IEnumerable<WFL> taskList, MortgageRefinancingWorkflowParameters mortgageParameters, CancellationToken cancellationToken)
    {
        var priceExceptionWasCancelled = true;

        foreach (var task in GetPriceExceptionTasks(taskList, mortgageParameters.TaskProcessId))
        {
            var priceExceptionTaskDetail = await _caseService.GetTaskDetail(task.TaskIdSb, cancellationToken);

            var taskIndividualPrice = new MortgageRefinancingIndividualPrice(priceExceptionTaskDetail.TaskDetail.PriceException);

            if (new MortgageRefinancingIndividualPrice(mortgageParameters.LoanInterestRateDiscount, mortgageParameters.Fee?.FeeFinalSum).Equals(taskIndividualPrice))
            {
                priceExceptionWasCancelled = false;

                continue;
            }

            ValidatePermission();

            await _caseService.CancelTask(mortgageParameters.CaseId, task.TaskIdSb, cancellationToken);
        }

        return priceExceptionWasCancelled;
    }

    private static IEnumerable<WFL> GetPriceExceptionTasks(IEnumerable<WFL> taskList, long taskProcessId) => 
        taskList.Where(t => t.ProcessId == taskProcessId && t is { TaskTypeId: (int)WorkflowTaskTypes.PriceException, Cancelled: false });

    private async Task<(DomainServices.SalesArrangementService.Contracts.SalesArrangement? salesArrangement, RefinancingStates RefinancingState)> getRefinancingStateId(long caseId, ProcessTask process, CancellationToken cancellationToken)
    {
        DomainServices.SalesArrangementService.Contracts.SalesArrangement? currentProcessSADetail = null;
        var allSalesArrangements = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        var currentProcessSA = allSalesArrangements.SalesArrangements.FirstOrDefault(t => t.TaskProcessId == process.ProcessId);
        if (currentProcessSA is not null)
        {
            currentProcessSADetail = await _salesArrangementService.GetSalesArrangement(currentProcessSA.SalesArrangementId, cancellationToken);
            if (currentProcessSA.Retention?.ManagedByRC2 ?? false)
            {
                // ref.state staci vzit pouze z SA
                return (currentProcessSADetail, RefinancingHelper.GetRefinancingState((SalesArrangementStates)currentProcessSA.State));
            }
        }

        return (currentProcessSADetail, RefinancingHelper.GetRefinancingState(false, currentProcessSA?.TaskProcessId, process));
    }

    private void ValidatePermission()
    {
        // TODO: projit zda je potreba s Klarou a Davidem
        if (_currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage))
            return;

        throw new CisAuthorizationException($"User does not have permission {UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage} ({(int)UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage})");
    }
}