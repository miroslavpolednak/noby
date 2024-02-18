﻿using DomainServices.CaseService.Clients;
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
            var productSA = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken)).First();
            var offer = await _offerService.GetOfferDetail(productSA.OfferId.GetValueOrDefault(), cancellationToken);
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
                        Expiration = DateOnly.FromDateTime(offer.MortgageOffer.BasicParameters.GuaranteeDateTo),
                        LoanInterestRate = new()
                        {
                            LoanInterestRate = offer.MortgageOffer.SimulationResults.LoanInterestRate,
                            LoanInterestRateProvided = offer.MortgageOffer.SimulationResults.LoanInterestRateProvided,
                            LoanInterestRateAnnouncedTypeName = loanInterestRateAnnouncedTypes
                                .FirstOrDefault(t => t.Id == offer.MortgageOffer.SimulationResults.LoanInterestRateAnnouncedType)?
                                .Name ?? string.Empty,
                            LoanInterestRateDiscount = offer.MortgageOffer.SimulationInputs.InterestRateDiscount
                        },
                        Fees = offer.MortgageOffer.AdditionalSimulationResults.Fees?.Select(t => new Dto.Workflow.Fee
                        {
                            FeeName = t.Name.ToString(System.Globalization.CultureInfo.InvariantCulture),
                            TariffSum = (decimal?)t.TariffSum ?? 0,
                            FinalSum = (decimal?)t.FinalSum ?? 0,
                            DiscountPercentage = t.DiscountPercentage
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
    private readonly Services.WorkflowTask.IWorkflowTaskService _workflowTaskService;
    private readonly ICaseServiceClient _caseService;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public GetCurrentPriceExceptionHandler(
        Services.WorkflowTask.IWorkflowTaskService workflowTaskService,
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
