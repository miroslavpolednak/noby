using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.UserService.Clients.Authorization;
using NOBY.Infrastructure.Security;
using NOBY.Services.WorkflowTask;
using WFL = DomainServices.CaseService.Contracts.WorkflowTask;

namespace NOBY.Services.OfferLink;

[ScopedService, SelfService]
public class MortgageRetentionWorkflowService
{
    private readonly ICaseServiceClient _caseService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public MortgageRetentionWorkflowService(ICaseServiceClient caseService, ICurrentUserAccessor currentUserAccessor)
    {
        _caseService = caseService;
        _currentUserAccessor = currentUserAccessor;
    }

    public Task<WorkflowTaskByTaskId.WorkflowTaskByTaskIdResult> GetTaskInfoByTaskId(long caseId, long taskId, CancellationToken cancellationToken) => 
        _caseService.GetTaskByTaskId(caseId, taskId, cancellationToken);

    public async Task UpdateRetentionWorkflowProcess(IMortgageParameters mortgageParameters, int taskIdSb, CancellationToken cancellationToken)
    {
        var updateRequest = new UpdateTaskRequest
        {
            CaseId = mortgageParameters.CaseId,
            TaskIdSb = taskIdSb,
            Retention = new Retention
            {
                InterestRateValidFrom = mortgageParameters.InterestRateValidFrom,
                LoanInterestRate = mortgageParameters.LoanInterestRate,
                LoanInterestRateProvided = mortgageParameters.LoanInterestRateProvided,
                LoanPaymentAmount = mortgageParameters.LoanPaymentAmount,
                LoanPaymentAmountFinal = mortgageParameters.LoanPaymentAmountFinal,
                FeeSum = mortgageParameters.FeeSum,
                FeeFinalSum = mortgageParameters.FeeFinalSum
            }
        };

        await _caseService.UpdateTask(updateRequest, cancellationToken);
    }

    public async Task CreateIndividualPriceWorkflowTask(List<WFL> taskList, IMortgageParameters mortgageParameters, string? taskRequest, CancellationToken cancellationToken)
    {
        if (!await CancelExistingPriceExceptions(taskList, mortgageParameters, cancellationToken) || mortgageParameters.LoanInterestRateDiscount is null or 0 || mortgageParameters.FeeFinalSum == 0)
            return;

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
                    LoanInterestRateDiscount = mortgageParameters.LoanInterestRateDiscount,
                    LoanInterestRateProvided = mortgageParameters.LoanInterestRate - mortgageParameters.LoanInterestRateDiscount
                },
                Fees =
                {
                    new PriceExceptionFeesItem
                    {
                        TariffSum = mortgageParameters.FeeSum,
                        FinalSum = mortgageParameters.FeeFinalSum,
                        DiscountPercentage = 100 * (mortgageParameters.FeeSum - mortgageParameters.FeeFinalSum) / mortgageParameters.FeeSum
                    }
                }
            }
        };

        await _caseService.CreateTask(createTaskRequest, cancellationToken);
    }

    private async Task<bool> CancelExistingPriceExceptions(IEnumerable<WFL> taskList, IMortgageParameters mortgageParameters, CancellationToken cancellationToken)
    {
        var priceExceptionTasks = taskList.Where(t => t.ProcessId == mortgageParameters.TaskProcessId && t is { TaskTypeId: (int)WorkflowTaskTypes.PriceException, Cancelled: false })
                                          .ToList();

        var priceExceptionWasCancelled = true;

        if (priceExceptionTasks.Count == 0) 
            return priceExceptionWasCancelled;

        foreach (var task in priceExceptionTasks)
        {
            var priceExceptionTaskDetail = await _caseService.GetTaskDetail(task.TaskIdSb, cancellationToken);

            var taskLoanInterestRateDiscount = ((decimal?)priceExceptionTaskDetail.TaskDetail.PriceException.LoanInterestRate.LoanInterestRateDiscount).GetValueOrDefault();
            var taskFeeFinalSum = ((decimal?)priceExceptionTaskDetail.TaskDetail.PriceException.Fees.FirstOrDefault()?.FinalSum).GetValueOrDefault();

            if (taskLoanInterestRateDiscount != mortgageParameters.LoanInterestRateDiscount.GetValueOrDefault() && taskFeeFinalSum != mortgageParameters.FeeFinalSum)
            {
                priceExceptionWasCancelled = false;

                continue;
            }

            ValidatePermission();

            await _caseService.CancelTask(mortgageParameters.CaseId, task.TaskIdSb, cancellationToken);
        }

        return priceExceptionWasCancelled;
    }


    private void ValidatePermission()
    {
        if (_currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage))
            return;

        throw new CisAuthorizationException($"User does not have permission {UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage} ({(int)UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage})");
    }
}