using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Dto.Refinancing;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters;

internal sealed class GetRefinancingParametersHandler(
    ICaseServiceClient _caseService,
    IProductServiceClient _productService,
    ISalesArrangementServiceClient _salesArrangementService,
    ICodebookServiceClient _codebookService)
        : IRequestHandler<GetRefinancingParametersRequest, GetRefinancingParametersResponse>
{
    public async Task<GetRefinancingParametersResponse> Handle(GetRefinancingParametersRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.ValidateCaseId(request.CaseId, false, cancellationToken);

        if (caseInstance.State is not (int)CaseStates.InDisbursement and (int)CaseStates.InAdministration)
            throw new NobyValidationException(90032);

        var mortgage = (await _productService.GetMortgage(request.CaseId, cancellationToken)).Mortgage;

        var saList = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);

        var caseDetail = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        var saFiltered = saList.SalesArrangements.Where(s => s.SalesArrangementTypeId is (int)SalesArrangementTypes.MortgageRetention or (int)SalesArrangementTypes.MortgageRefixation);

        var saFilteredWithDetail = await Task.WhenAll(saFiltered.Select(d => _salesArrangementService.GetSalesArrangement(d.SalesArrangementId)));

        var refinancingProcessList = (await _caseService.GetProcessList(request.CaseId, cancellationToken))
                                     .Where(p => p.ProcessTypeId is (int)WorkflowProcesses.Refinancing).ToList();

        var mergeOfSaAndProcess = refinancingProcessList.Select(pr => new
        {
            Process = pr,
            Sa = Array.Find(saFilteredWithDetail, sa => sa.ProcessId == pr.ProcessId)
        });

        var eaCodesMain = await _codebookService.EaCodesMain(cancellationToken);
        var refinancingTypes = await _codebookService.RefinancingTypes(cancellationToken);

        return new()
        {
            IsAnotherSalesArrangementInProgress = RefinancingHelper.IsAnotherSalesArrangementInProgress(saList),
            CustomerPriceSensitivity = caseDetail.Customer.CustomerPriceSensitivity,
            CustomerChurnRisk = caseDetail.Customer.CustomerChurnRisk,
            RefinancingParametersCurrent = new()
            {
                LoanAmount = mortgage.LoanAmount,
                Principal = mortgage.Principal,
                LoanDueDate = mortgage.LoanDueDate,
                LoanInterestRate = mortgage.LoanInterestRate,
                FixedRatePeriod = mortgage.FixedRatePeriod,
                LoanPaymentAmount = mortgage.LoanPaymentAmount,
                FixedRateValidFrom = RefinancingHelper.GetFixedRateValidFrom(mortgage.FixedRateValidTo, mortgage.FixedRatePeriod),
                FixedRateValidTo = mortgage.FixedRateValidTo
            },
            RefinancingParametersFuture = new()
            {
                LoanInterestRate = mortgage.LoanInterestRateRefinancing,
                FixedRatePeriod = mortgage.FixedRatePeriodRefinancing,
                LoanPaymentAmount = mortgage.LoanPaymentAmountRefinancing,
                FixedRateValidFrom = RefinancingHelper.GetFixedRateValidFrom(mortgage.LoanInterestRateValidToRefinancing, mortgage.FixedRatePeriodRefinancing),
                FixedRateValidTo = mortgage.LoanInterestRateValidToRefinancing
            },
            RefinancingProcesses = mergeOfSaAndProcess.Select(s => new RefinancingProcess
            {
                SalesArrangementId = s.Sa?.SalesArrangementId,
                ProcessDetail = getProcessDetail(s.Process, s.Sa, mortgage.FixedRateValidTo, eaCodesMain, refinancingTypes)
            }).ToList()
        };
    }

    private static ProcessDetail getProcessDetail(
        ProcessTask process, 
        DomainServices.SalesArrangementService.Contracts.SalesArrangement? sa,
        DateTime? fixedRateValidTo,
        List<DomainServices.CodebookService.Contracts.v1.EaCodesMainResponse.Types.EaCodesMainItem> eaCodesMain,
        List<DomainServices.CodebookService.Contracts.v1.GenericCodebookResponse.Types.GenericCodebookItem> refinancingTypes)
    {
        ProcessDetail detail = new()
        {
            ProcessId = process.ProcessId,
            RefinancingTypeId = RefinancingHelper.GetRefinancingType(process),
            RefinancingTypeText = RefinancingHelper.GetRefinancingTypeText(eaCodesMain, process, refinancingTypes),
            RefinancingStateId = (int)RefinancingHelper.GetRefinancingState(sa?.Refixation?.ManagedByRC2 ?? sa?.Retention?.ManagedByRC2 ?? false, sa?.ProcessId, process),
            CreatedTime = process.CreatedOn,
            LoanInterestRateValidTo = fixedRateValidTo
        };

        switch (process.AmendmentsCase)
        {
            case ProcessTask.AmendmentsOneofCase.MortgageRetention:
                detail.LoanInterestRateProvided = process.MortgageRetention.LoanInterestRateProvided ?? process.MortgageRetention.LoanInterestRate;
                detail.LoanInterestRateValidFrom = process.MortgageRetention.InterestRateValidFrom!;
                detail.EffectiveDate = process.MortgageRetention.EffectiveDate;
                detail.DocumentId = process.MortgageRetention.DocumentId;
                break;

            case ProcessTask.AmendmentsOneofCase.MortgageRefixation:
                detail.LoanInterestRateProvided = process.MortgageRefixation.LoanInterestRateProvided ?? process.MortgageRetention.LoanInterestRate;
                detail.EffectiveDate = process.MortgageRefixation.EffectiveDate;
                detail.DocumentId = process.MortgageRefixation.DocumentId;
                break;
        }

        return detail;
    }
}
