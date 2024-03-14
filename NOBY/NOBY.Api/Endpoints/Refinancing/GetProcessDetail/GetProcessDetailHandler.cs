using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Refinancing.GetProcessDetail;

internal class GetProcessDetailHandler : IRequestHandler<GetProcessDetailRequest, GetProcessDetailResponse>
{
    private readonly ICaseServiceClient _caseService;
    private readonly IProductServiceClient _productService;
    private readonly ICodebookServiceClient _codebookService;

    public GetProcessDetailHandler(
        ICaseServiceClient caseService,
        IProductServiceClient productService,
        ICodebookServiceClient codebookService)
    {
        _caseService = caseService;
        _productService = productService;
        _codebookService = codebookService;
    }

    public async Task<GetProcessDetailResponse> Handle(GetProcessDetailRequest request, CancellationToken cancellationToken)
    {
        var process = (await _caseService.GetProcessList(request.caseId, cancellationToken))
            .SingleOrDefault(p => p.ProcessId == request.processId)
            ?? throw new NobyValidationException(90043, $"ProccesId not found in list {request.processId}");

        if (process.ProcessTypeId is not 3)
            throw new NobyValidationException(90032, "Only processTypeId == 3 is allowed");

        var eaCodesMain = await _codebookService.EaCodesMain(cancellationToken);
        var refinancingTypes = await _codebookService.RefinancingTypes(cancellationToken);
        var mortgage = (await _productService.GetMortgage(request.caseId, cancellationToken)).Mortgage;

        return new()
        {
            ProcessDetail = new()
            {
                ProcessId = process.ProcessId,
                RefinancingTypeId = RefinancingHelper.GetRefinancingType(process),
                RefinancingTypeText = RefinancingHelper.GetRefinancingTypeText(eaCodesMain, process, refinancingTypes),
                RefinancingStateId = RefinancingHelper.GetRefinancingState(null, process),
                CreatedTime = process.CreatedOn,
                LoanInterestRateProvided = process.RefinancingProcess?.LoanInterestRateProvided ?? process.RefinancingProcess?.LoanInterestRate,
                LoanInterestRateValidFrom = process.RefinancingProcess?.InterestRateValidFrom!,
                LoanInterestRateValidTo = mortgage.FixedRateValidTo, // This should be mapped from process in near feature (SB has to implement it first)
                EffectiveDate = process.RefinancingProcess?.EffectiveDate,
                DocumentId = process.RefinancingProcess?.RefinancingDocumentId
            },
            LoanInterestRate = process.RefinancingProcess?.LoanInterestRate,
            LoanPaymentAmount = process.RefinancingProcess?.LoanPaymentAmount,
            LoanPaymentAmountFinal = process.RefinancingProcess?.LoanPaymentAmountFinal,
            FeeSum = process.RefinancingProcess?.FeeSum,
            FeeFinalSum = process.RefinancingProcess?.FeeFinalSum,
        };
    }
}
