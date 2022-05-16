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
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<_Offer.GetMortgageDataResponse>(await _offerService.GetMortgageData(saInstance.OfferId!.Value, cancellationToken));

        GetLoanApplicationAssessmentResponse model = new()
        {
            Application = new()
            {
                LoanApplicationLimit = 9862532,
                LoanAmount = offerInstance.Outputs.LoanAmount,
                LoanApplicationInstallmentLimit = 26825,
                LoanPaymentAmount = offerInstance.Outputs.LoanPaymentAmount,
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
            AssessmentResult = 504,
            Reasons = new()
            {
                new Dto.AssessmentReason
                {
                    Desc = "Úvěrový obchod nelze samoschválit, postupujte dle standardních schvalovacích pravomocí"
                },
                new Dto.AssessmentReason
                {
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
