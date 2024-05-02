using CIS.Core;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Dto.Workflow;
using NOBY.Infrastructure.ErrorHandling;
using System.Threading.Tasks;

namespace NOBY.Services.MortgageRefinancing;

[ScopedService, SelfService]
public sealed class MortgageRefinancingDataService(
    ICodebookServiceClient _codebookService,
    ICaseServiceClient _caseService,
    ISalesArrangementServiceClient _salesArrangementService,
    WorkflowMapper.IWorkflowMapperService _workflowMapper)
{
    public async Task<Dto.Refinancing.RefinancingDocument> CreateSigningDocument(GetRefinancingDataResult result, RefinancingTypes refinancingType, int? eaCode, string? documentId)
    {
        // aktivni podepisovaci task
        var activeSigningTask = result.Tasks?.FirstOrDefault(t => t.TaskTypeId == (int)WorkflowTaskTypes.Signing && !t.Cancelled);

        var response = new Dto.Refinancing.RefinancingDocument
        {
            DocumentId = documentId,
            IsContinueEnabled = refinancingType == RefinancingTypes.MortgageRetention || (refinancingType == RefinancingTypes.MortgageRefixation && eaCode is (605353 or 604587)),
            DocumentName = await getSigningDocumentName(refinancingType, eaCode),
            SignatureTypeDetailId = result.SalesArrangement?.Retention?.SignatureTypeDetailId, // pouze pro retence
            IsGenerateDocumentEnabled = result.SalesArrangement?.OfferId is not null
                && result.RefinancingState == RefinancingStates.RozpracovanoVNoby
                && result.ActivePriceException is null
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

    public async Task<GetRefinancingDataResult> GetRefinancingData(long caseId, long? processId, RefinancingTypes refinancingType, CancellationToken cancellationToken)
    {
        GetRefinancingDataResult result = new()
        {
            RefinancingState = RefinancingStates.Unknown
        };

        if (processId.HasValue)
        {
            // detail procesu
            result.Process = (await _caseService.GetProcessList(caseId, cancellationToken))
                .FirstOrDefault(p => p.ProcessId == processId)
                ?? throw new NobyValidationException(90043, $"ProccesId {processId} not found in list");

            // validace typu procesu
            if (result.Process.AmendmentsCase != getRequiredAmendmentCase(refinancingType))
            {
                throw new NobyValidationException(90032, $"ProcessTypeId!=3 or RefinancingType!={refinancingType}");
            }

            // zjistit refinancingState
            var (salesArrangement, refinancingState) = await getRefinancingStateId(caseId, result.Process, cancellationToken);

            if (refinancingState is (RefinancingStates.Zruseno or RefinancingStates.Dokonceno))
            {
                throw new NobyValidationException(90032, $"RefinancingState is not allowed: {refinancingState}");
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
        result.ActivePriceException = await getPriceExceptionData(tasks, cancellationToken);

        return result;
    }

    private async Task<string> getSigningDocumentName(RefinancingTypes refinancingType, int? eaCode)
    {
        var code = (await _codebookService.EaCodesMain()).FirstOrDefault(t => t.Id == eaCode);
        if (code is not null)
        {
            return code.Name;
        }
        else
        {
            return refinancingType switch
            {
                RefinancingTypes.MortgageRetention => "RefinancingType",//??? co sem?
                RefinancingTypes.MortgageRefixation => "Dodatek - Refixace",
                _ => "Unknown"
            };
        }
    }

    private async Task<AmendmentPriceException?> getPriceExceptionData(List<DomainServices.CaseService.Contracts.WorkflowTask> tasks, CancellationToken cancellationToken)
    {
        // toto je aktivni IC task!
        var activePriceExceptionTaskIdSb = tasks
            .FirstOrDefault(t => t.TaskTypeId == (int)WorkflowTaskTypes.PriceException && !t.Cancelled && t.DecisionId != 2 && t.PhaseTypeId == 2)
            ?.TaskIdSb;

        // detail IC tasku
        if (activePriceExceptionTaskIdSb.HasValue)
        {
            return (await _caseService.GetTaskDetail(activePriceExceptionTaskIdSb.Value, cancellationToken))?.TaskDetail?.PriceException;
        }

        return null;
    }

    private static ProcessTask.AmendmentsOneofCase getRequiredAmendmentCase(RefinancingTypes refinancingType)
        => refinancingType switch
        {
            RefinancingTypes.MortgageRetention => ProcessTask.AmendmentsOneofCase.MortgageRetention,
            RefinancingTypes.MortgageRefixation => ProcessTask.AmendmentsOneofCase.MortgageRefixation,
            RefinancingTypes.MortgageExtraPayment => ProcessTask.AmendmentsOneofCase.MortgageExtraPayment,
            _ => throw new NotImplementedException()
        };

    private async Task<(DomainServices.SalesArrangementService.Contracts.SalesArrangement? salesArrangement, RefinancingStates RefinancingState)> getRefinancingStateId(long caseId, ProcessTask process, CancellationToken cancellationToken)
    {
        DomainServices.SalesArrangementService.Contracts.SalesArrangement? currentProcessSADetail = null;
        var allSalesArrangements = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        var currentProcessSA = allSalesArrangements.SalesArrangements.FirstOrDefault(t => t.ProcessId == process.ProcessId);
        if (currentProcessSA is not null)
        {
            currentProcessSADetail = await _salesArrangementService.GetSalesArrangement(currentProcessSA.SalesArrangementId, cancellationToken);
            if (currentProcessSA.Retention?.ManagedByRC2 ?? currentProcessSA.Refixation?.ManagedByRC2 ?? false)
            {
                // ref.state staci vzit pouze z SA
                return (currentProcessSADetail, RefinancingHelper.GetRefinancingState((SalesArrangementStates)currentProcessSA.State));
            }
        }

        return (currentProcessSADetail, RefinancingHelper.GetRefinancingState(false, currentProcessSA?.ProcessId, process));
    }
}
