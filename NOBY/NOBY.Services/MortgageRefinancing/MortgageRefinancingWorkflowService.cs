using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.UserService.Clients.Authorization;
using NOBY.Infrastructure.ErrorHandling;
using NOBY.Infrastructure.Security;
using NOBY.Services.WorkflowTask;
using WFL = DomainServices.CaseService.Contracts.WorkflowTask;

namespace NOBY.Services.MortgageRefinancing;

[ScopedService, SelfService]
public sealed class MortgageRefinancingWorkflowService(
    ICaseServiceClient _caseService, 
    ICurrentUserAccessor _currentUserAccessor)
{
    public Task<WorkflowTaskByTaskId.WorkflowProcessByProcessIdResult> GetProcessInfoByProcessId(long caseId, long processId, CancellationToken cancellationToken) => 
        _caseService.GetProcessByProcessId(caseId, processId, cancellationToken);

    public void ValidateIndividualPriceExceptionComment(string? individualPriceExceptionComment, decimal? interestRateDiscount, decimal? feeAmountDiscount)
    {
        if (interestRateDiscount is null or 0 && feeAmountDiscount is null or 0)
            return;

        if (string.IsNullOrWhiteSpace(individualPriceExceptionComment))
            throw new NobyValidationException(90032, "Unable to update discount without comment");
    }

    public async Task<MortgageRefinancingIndividualPrice> GetIndividualPrices(long caseId, long processId, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(caseId, cancellationToken);

        var priceExceptionTasks = GetPriceExceptionTasks(taskList, processId).ToList();

        var priceExceptionTask = priceExceptionTasks.Count == 1 ? priceExceptionTasks.Single() : throw new NobyValidationException(90050);

        if (priceExceptionTask.StateIdSb != 30 && priceExceptionTask.PhaseTypeId != 2)
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
            || (mortgageParameters.LoanInterestRateDiscount is (null or 0) && (mortgageParameters.Fee?.DiscountPercentage ?? 0) == 0))
        {
            return;
        }

        ValidatePermission();

        var createTaskRequest = new CreateTaskRequest
        {
            CaseId = mortgageParameters.CaseId,
            TaskTypeId = (int)WorkflowTaskTypes.PriceException,
            ProcessId = mortgageParameters.ProcessId,
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
                FeeId = mortgageParameters.Fee.FeeId,
                TariffSum = mortgageParameters.Fee.FeeSum,
                FinalSum = mortgageParameters.Fee.FeeFinalSum,
                DiscountPercentage = mortgageParameters.Fee.DiscountPercentage
            });
        }

        await _caseService.CreateTask(createTaskRequest, cancellationToken);
    }

    private async Task<bool> CancelExistingPriceExceptions(IEnumerable<WFL> taskList, MortgageRefinancingWorkflowParameters mortgageParameters, CancellationToken cancellationToken)
    {
        var priceExceptionWasCancelled = true;

        foreach (var task in GetPriceExceptionTasks(taskList, mortgageParameters.ProcessId))
        {
            var priceExceptionTaskDetail = await _caseService.GetTaskDetail(task.TaskIdSb, cancellationToken);

            var taskIndividualPrice = new MortgageRefinancingIndividualPrice(priceExceptionTaskDetail.TaskDetail.PriceException);

            if (new MortgageRefinancingIndividualPrice(mortgageParameters.LoanInterestRateDiscount, mortgageParameters.Fee?.FeeDiscount).Equals(taskIndividualPrice))
            {
                priceExceptionWasCancelled = false;

                continue;
            }

            ValidatePermission();

            await _caseService.CancelTask(mortgageParameters.CaseId, task.TaskIdSb, cancellationToken);
        }

        return priceExceptionWasCancelled;
    }

    private static IEnumerable<WFL> GetPriceExceptionTasks(IEnumerable<WFL> taskList, long processId) => 
        taskList.Where(t => t.ProcessId == processId && t is { TaskTypeId: (int)WorkflowTaskTypes.PriceException, Cancelled: false });

    private void ValidatePermission()
    {
        // TODO: projit zda je potreba s Klarou a Davidem
        if (_currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage))
            return;

        throw new CisAuthorizationException($"User does not have permission {UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage} ({(int)UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage})");
    }
}