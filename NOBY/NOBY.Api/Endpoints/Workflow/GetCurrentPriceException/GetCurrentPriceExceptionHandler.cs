using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Workflow.GetCurrentPriceException;

internal sealed class GetCurrentPriceExceptionHandler(
    Services.WorkflowTask.IWorkflowTaskService _workflowTaskService,
    ICodebookServiceClient _codebookService,
    ICaseServiceClient _caseService,
    ISalesArrangementServiceClient _salesArrangementService,
    IOfferServiceClient _offerService)
        : IRequestHandler<GetCurrentPriceExceptionRequest, WorkflowGetCurrentPriceExceptionResponse>
{
    public async Task<WorkflowGetCurrentPriceExceptionResponse> Handle(GetCurrentPriceExceptionRequest request, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(request.CaseId, cancellationToken);
        var priceException = taskList.FirstOrDefault(t => t.TaskTypeId == (int)WorkflowTaskTypes.PriceException && !t.Cancelled);

        if (priceException is not null)
        {
            var (taskDto, taskDetailDto, documents) = await _workflowTaskService.GetTaskDetail(request.CaseId, priceException.TaskIdSb, cancellationToken);

            return new WorkflowGetCurrentPriceExceptionResponse
            {
                TaskDetail = taskDetailDto,
                Task = taskDto,
                Documents = documents
            };
        }
        else
        {
            var productSA = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken)).First();
            var offer = await _offerService.GetMortgageDetail(productSA.OfferId.GetValueOrDefault(), cancellationToken);
            var process = (await _caseService.GetProcessList(request.CaseId, cancellationToken)).FirstOrDefault(t => t.ProcessTypeId == (int)WorkflowProcesses.Main);
            string? taskTypeName = (await _codebookService.WorkflowTaskTypes(cancellationToken)).FirstOrDefault(t => t.Id == (int)WorkflowTaskTypes.PriceException)?.Name;
            var loanInterestRateAnnouncedTypes = await _codebookService.LoanInterestRateAnnouncedTypes(cancellationToken);

            var amendment = new SharedTypesWorkflowAmendmentsPriceException
            {
                Expiration = DateOnly.FromDateTime(offer.BasicParameters.GuaranteeDateTo),
                LoanInterestRate = new()
                {
                    LoanInterestRate = offer.SimulationResults.LoanInterestRate,
                    LoanInterestRateProvided = offer.SimulationResults.LoanInterestRateProvided,
                    LoanInterestRateAnnouncedTypeName = loanInterestRateAnnouncedTypes
                                .FirstOrDefault(t => t.Id == offer.SimulationResults.LoanInterestRateAnnouncedType)?
                                .Name ?? string.Empty,
                    LoanInterestRateDiscount = offer.SimulationInputs.InterestRateDiscount
                },
                Fees = offer.AdditionalSimulationResults.Fees?.Select(t => new SharedTypesWorkflowAmendmentsPriceExceptionFee
                {
                    FeeName = t.Name.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    TariffSum = (decimal?)t.TariffSum ?? 0,
                    FinalSum = (decimal?)t.FinalSum ?? 0,
                    DiscountPercentage = t.DiscountPercentage
                })?.ToList()
            };

            var response = new WorkflowGetCurrentPriceExceptionResponse
            {
                TaskDetail = new()
                {
                    ProcessNameLong = process?.ProcessNameLong ?? "",
                    Amendments = SharedTypesWorkflowTaskDetailAmendmentsOneOf.Create(amendment)
                },
                Task = new()
                {
                    TaskTypeName = taskTypeName ?? "",
                    ProcessId = process?.ProcessId ?? 0,
                    TaskTypeId = (int)WorkflowTaskTypes.PriceException
                }
            };

            return response;
        }
    }
}
