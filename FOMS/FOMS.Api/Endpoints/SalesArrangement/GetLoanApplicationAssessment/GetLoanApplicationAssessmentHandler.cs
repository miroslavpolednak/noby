using DomainServices.OfferService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Offer = DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal class GetLoanApplicationAssessmentHandler
    : IRequestHandler<GetLoanApplicationAssessmentRequest, GetLoanApplicationAssessmentResponse>
{
    public async Task<GetLoanApplicationAssessmentResponse> Handle(GetLoanApplicationAssessmentRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));
        if (!saInstance.OfferId.HasValue)
            throw new CisNotFoundException(0, "Offer not found for SA");
        // instance Offer
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<_Offer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId!.Value, cancellationToken));

        int[] codes = new[] { 501, 502 };
        Random random = new Random();
        int randomNumber1 = random.Next(0, codes.Length);

        GetLoanApplicationAssessmentResponse model = new()
        {
            Application = new()
            {
                LoanApplicationLimit = 9862532,
                LoanAmount = offerInstance.SimulationResults.LoanAmount ?? 0,
                LoanApplicationInstallmentLimit = 26825,
                LoanPaymentAmount = offerInstance.SimulationResults.LoanPaymentAmount ?? 0,
                RemainingAnnuityLivingAmount = 8652,
                MonthlyIncomeAmount = 56200,
                MonthlyCostsWithoutInstAmount = 13200,
                MonthlyInstallmentsInKBAmount = 5200,
                MonthlyEntrepreneurInstallmentsInKBAmount = 0,
                MonthlyInstallmentsInMPSSAmount = 0,
                MonthlyInstallmentsInOFIAmount = 6526,
                MonthlyInstallmentsInCBCBAmount = 11487,
                CIR = 2,
                DTI = 4,
                DSTI = 52,
                LTC = 12,
                LFTV = 52,
                LTV = 79
            },
            Households = new()
            {
                new Dto.Household
                {
                    HouseholdId = 2,
                    MonthlyIncomeAmount = 25852,
                    MonthlyCostsWithoutInstAmount = 3652,
                    MonthlyInstallmentsInMPSSAmount = 2400,
                    MonthlyInstallmentsInOFIAmount = 7825,
                    MonthlyInstallmentsInCBCBAmount = 12935,
                    CIR = 2,
                    DTI = 4,
                    DSTI = 48
                }
            },
            AssessmentResult = codes[randomNumber1],
            Reasons = new()
            {
                new()
                {
                    Code = "SOLUS",
                    Level = "ERROR",
                    Weight = 100,
                    Target = "R",
                    Desc = "Negativní záznam v SOLUSu",
                    Result = "Black"
                },
                new()
                {
                    Code = "FRAUD",
                    Level = "WARNING",
                    Weight = 11,
                    Target = "R",
                    Desc = "3x Usvědčený podvodník",
                    Result = "Black"
                },
                new()
                {
                    Code = "SOLUS",
                    Level = "ERROR",
                    Weight = 200,
                    Target = "R",
                    Desc = "Nesplacené závazky",
                    Result = "Black"
                },
                new()
                {
                    Code = "FRAUD",
                    Level = "WARNING",
                    Weight = 1,
                    Target = "R",
                    Desc = "Podvody u mobilních operátorů",
                    Result = "Black"
                },
                new()
                {
                    Code = "ID",
                    Level = "PROBLEM",
                    Weight = 1,
                    Target = "R",
                    Desc = "Prošlé doklady",
                    Result = "Black"
                },
                new()
                {
                    Code = "SOLUS",
                    Level = "INFO",
                    Weight = 100,
                    Target = "R",
                    Desc = "Negativní záznam v SOLUSu",
                    Result = "Black"
                }
            }
        };

        return model;
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;

    public GetLoanApplicationAssessmentHandler(
        IOfferServiceAbstraction offerService,
        ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
    }
}
