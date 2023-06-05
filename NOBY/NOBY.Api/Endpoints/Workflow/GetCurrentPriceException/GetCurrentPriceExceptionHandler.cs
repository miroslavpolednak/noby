using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Workflow.GetCurrentPriceException;

internal sealed class GetCurrentPriceExceptionHandler
    : IRequestHandler<GetCurrentPriceExceptionRequest, GetCurrentPriceExceptionResponse>
{
    public async Task<GetCurrentPriceExceptionResponse> Handle(GetCurrentPriceExceptionRequest request, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(request.CaseId, cancellationToken);
        var priceException = taskList.FirstOrDefault(t => t.TaskTypeId == 2 && !t.Cancelled);

        if (priceException is not null)
        {
            var (taskDto, taskDetailDto, documents) = await _workflowTaskService.GetTaskDetail(request.CaseId, priceException.TaskIdSb, cancellationToken);

            return new GetCurrentPriceExceptionResponse
            {
                TaskDetail = taskDetailDto,
                Task = taskDto,
                Documents = documents
            };
        }
        else
        {
            var productSAId = await _salesArrangementService.GetProductSalesArrangementId(request.CaseId, cancellationToken);
            var offer = await _offerService.GetMortgageOfferDetail(productSAId, cancellationToken);
            var process = (await _caseService.GetProcessList(request.CaseId, cancellationToken)).FirstOrDefault(t => t.ProcessTypeId == 1);
            string? taskTypeName = (await _codebookService.WorkflowTaskTypes(cancellationToken)).FirstOrDefault(t => t.Id == 2)?.Name;
            var loanInterestRateAnnouncedTypes = await _codebookService.LoanInterestRateAnnouncedTypes(cancellationToken);
            
            var response = new GetCurrentPriceExceptionResponse
            {
                TaskDetail = new()
                {
                    ProcessNameLong = process?.ProcessNameLong ?? "",
                    Amendments = new NOBY.Dto.Workflow.AmendmentsPriceException
                    {
                        Expiration = offer.BasicParameters.GuaranteeDateTo,
                        LoanInterestRate = new()
                        {
                            LoanInterestRate = offer.SimulationResults.LoanInterestRate?.ToString() ?? "",
                            LoanInterestRateProvided = offer.SimulationResults.LoanInterestRateProvided?.ToString() ?? "",
                            LoanInterestRateAnnouncedTypeName = loanInterestRateAnnouncedTypes
                                .FirstOrDefault(t => t.Id == offer.SimulationResults.LoanInterestRateAnnouncedType)?
                                .Name ?? string.Empty,
                            LoanInterestRateDiscount = offer.SimulationInputs.InterestRateDiscount?.ToString() ?? ""
                        },
                        Fees = offer.AdditionalSimulationResults.Fees?.Select(t => new Dto.Workflow.Fee
                        {
                            FeeId = t.FeeId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                            //TariffSum = t.TariffSum,
                            //FinalSum = t.FinalSum,
                            //DiscountPercentage = t.DiscountPercentage
                        })?.ToList()
                    }
                },
                Task = new()
                {
                    TaskTypeName = taskTypeName ?? "",
                    ProcessId = process?.ProcessId ?? 0,
                    TaskTypeId = 2
                }
            };

            return response;
        }
    }

    private readonly ICodebookServiceClient _codebookService;
    private readonly Infrastructure.Services.WorkflowTask.IWorkflowTaskService _workflowTaskService;
    private readonly ICaseServiceClient _caseService;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public GetCurrentPriceExceptionHandler(
        Infrastructure.Services.WorkflowTask.IWorkflowTaskService workflowTaskService,
        ICodebookServiceClient codebookService,
        ICaseServiceClient caseService, 
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService)
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _workflowTaskService = workflowTaskService;
    }
}
