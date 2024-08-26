using CIS.Core;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.ApiContracts;
using NOBY.Infrastructure.ErrorHandling;

namespace NOBY.Services.MortgageRefinancing;

[ScopedService, SelfService]
public sealed class MortgageRefinancingDataService(
    ICodebookServiceClient _codebookService,
    ICaseServiceClient _caseService,
    ISalesArrangementServiceClient _salesArrangementService,
    WorkflowMapper.IWorkflowMapperService _workflowMapper)
{
    public async Task<RefinancingSharedDocument> CreateSigningDocument(GetRefinancingDataResult result, EnumRefinancingTypes refinancingType, int? eaCode, string? documentId)
    {
        // aktivni podepisovaci task
        var activeSigningTask = result.Tasks?.FirstOrDefault(t => t.TaskTypeId == (int)WorkflowTaskTypes.Signing && !t.Cancelled);

        var response = new RefinancingSharedDocument
        {
            DocumentId = documentId,
            IsContinueEnabled = refinancingType == EnumRefinancingTypes.MortgageRetention || (refinancingType == EnumRefinancingTypes.MortgageRefixation && activeSigningTask is not null),
            DocumentName = await getSigningDocumentName(refinancingType, eaCode),
            SignatureTypeDetailId = result.SalesArrangement?.Retention?.SignatureTypeDetailId ?? result.SalesArrangement?.Refixation?.SignatureTypeDetailId,
            IsGenerateDocumentEnabled = (refinancingType == EnumRefinancingTypes.MortgageRefixation || result.SalesArrangement?.OfferId is not null)
                                        && result.RefinancingState == EnumRefinancingStates.RozpracovanoVNoby
                                        && (result.ActivePriceException is null || result.IsActivePriceExceptionCompleted)
        };

        // pokud existuje aktivni task na podepisovani
        if (activeSigningTask is not null)
        {
            response.TaskId = activeSigningTask.TaskId;
            response.StateIndicator = activeSigningTask.StateIndicator;
            response.StateName = activeSigningTask.StateName;
            response.StateFilter = activeSigningTask.StateFilter;

            // odstranit task z kolekce
            result.Tasks!.Remove(activeSigningTask);
        }

        return response;
    }

    public async Task<GetRefinancingDataResult> GetRefinancingData(long caseId, long? processId, EnumRefinancingTypes refinancingType, CancellationToken cancellationToken)
    {
        GetRefinancingDataResult result = new()
        {
            RefinancingState = EnumRefinancingStates.Unknown
        };

        var processes = await _caseService.GetProcessList(caseId, cancellationToken);

        if (processes?.Any(t => 
            t.ProcessId != processId 
            && (t.StateIdSB is not 30) 
            && t.ProcessTypeId == (int)WorkflowProcesses.Refinancing 
            && t.RefinancingType == (int)refinancingType) ?? false)
        {
            throw new NobyValidationException(90061);
        }

        if (processId.HasValue)
        {
            // detail procesu
            result.Process = processes
                ?.FirstOrDefault(p => p.ProcessId == processId)
                ?? throw new NobyValidationException(90043, $"ProccesId {processId} not found in list");

            // validace typu procesu
            if (result.Process.AmendmentsCase != getRequiredAmendmentCase(refinancingType))
            {
                throw new NobyValidationException(90032, $"ProcessTypeId!=3 or RefinancingType!={refinancingType}");
            }

            // zjistit refinancingState
            var (salesArrangement, refinancingState) = await getRefinancingStateId(caseId, result.Process, cancellationToken);

            // validace stavu refinancovani
            if (refinancingType == EnumRefinancingTypes.MortgageExtraPayment && refinancingState is not (EnumRefinancingStates.RozpracovanoVNoby or EnumRefinancingStates.Dokonceno or EnumRefinancingStates.Zruseno))
            {
                throw new NobyValidationException(90070);
            }
            else if (refinancingType != EnumRefinancingTypes.MortgageExtraPayment && refinancingState is (EnumRefinancingStates.Zruseno or EnumRefinancingStates.Dokonceno))
            {
                throw new NobyValidationException(90070);
            }

            result.SalesArrangement = salesArrangement;
            result.RefinancingState = refinancingState;
        }
        
        // vsechny tasky z WF, potom vyfiltrovat jen na konkretni processId
        var tasks = (await _caseService.GetTaskList(caseId, cancellationToken))
            .Where(t => t.ProcessId == processId)
            .ToList();

        result.Tasks = (await tasks
            .SelectAsync(t => _workflowMapper.MapTask(t, cancellationToken)))
            .ToList();

        // toto je aktivni IC task
        var priceExceptionData = await getPriceExceptionData(tasks, cancellationToken);

        result.ActivePriceException = priceExceptionData.priceException;
        result.IsActivePriceExceptionCompleted = priceExceptionData.completed;

        return result;
    }

    public async Task<AmendmentPriceException?> GetActivePriceException(long caseId, long processId, CancellationToken cancellationToken)   
    {
        var tasks = (await _caseService.GetTaskList(caseId, cancellationToken))
            .Where(t => t.ProcessId == processId)
            .ToList();

        var result = await getPriceExceptionData(tasks, cancellationToken);

        return result.priceException;
    }

    private async Task<string> getSigningDocumentName(EnumRefinancingTypes refinancingType, int? eaCode)
    {
        var code = (await _codebookService.EaCodesMain()).FirstOrDefault(t => t.Id == eaCode);
        if (code is not null)
        {
            return code.Name;
        }
        else
        {
            return (await _codebookService.RefinancingTypes()).First(t => t.Id == (int)refinancingType).Name;
        }
    }

    private async Task<(bool completed, AmendmentPriceException? priceException)> getPriceExceptionData(List<DomainServices.CaseService.Contracts.WorkflowTask> tasks, CancellationToken cancellationToken)
    {
        // toto je aktivni IC task!
        var activePriceExceptionTask = tasks.FirstOrDefault(t => t is { TaskTypeId: (int)WorkflowTaskTypes.PriceException, Cancelled: false });

        if (activePriceExceptionTask is null || activePriceExceptionTask.DecisionId == 2) 
            return default;

        // detail IC tasku
        var taskDetail = await _caseService.GetTaskDetail(activePriceExceptionTask.TaskIdSb, cancellationToken);
        var isCompleted = taskDetail.TaskObject.PhaseTypeId == 2 && taskDetail.TaskObject.DecisionId == 1;

        return (isCompleted, taskDetail.TaskDetail?.PriceException);

    }

    private static ProcessTask.AmendmentsOneofCase getRequiredAmendmentCase(EnumRefinancingTypes refinancingType)
        => refinancingType switch
        {
            EnumRefinancingTypes.MortgageRetention => ProcessTask.AmendmentsOneofCase.MortgageRetention,
            EnumRefinancingTypes.MortgageRefixation => ProcessTask.AmendmentsOneofCase.MortgageRefixation,
            EnumRefinancingTypes.MortgageExtraPayment => ProcessTask.AmendmentsOneofCase.MortgageExtraPayment,
            _ => throw new NotImplementedException()
        };

    private async Task<(DomainServices.SalesArrangementService.Contracts.SalesArrangement? salesArrangement, EnumRefinancingStates RefinancingState)> getRefinancingStateId(long caseId, ProcessTask process, CancellationToken cancellationToken)
    {
        DomainServices.SalesArrangementService.Contracts.SalesArrangement? currentProcessSADetail = null;
        var allSalesArrangements = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        EnumSalesArrangementStates helperState = EnumSalesArrangementStates.Unknown;
        bool helperManagedByRC2 = false;

        var currentProcessSA = allSalesArrangements.SalesArrangements.FirstOrDefault(t => t.ProcessId == process.ProcessId);
        if (currentProcessSA is not null)
        {
            currentProcessSADetail = await _salesArrangementService.GetSalesArrangement(currentProcessSA.SalesArrangementId, cancellationToken);

            helperState = (EnumSalesArrangementStates)currentProcessSA.State;
            helperManagedByRC2 = currentProcessSADetail.Retention?.ManagedByRC2 ?? currentProcessSADetail.Refixation?.ManagedByRC2 ?? false;
        }

        return (currentProcessSADetail, RefinancingHelper.GetRefinancingState(helperState, helperManagedByRC2, process));
    }
}
