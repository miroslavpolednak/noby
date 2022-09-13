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
        if (!request.NewAssessmentRequired && string.IsNullOrEmpty(saInstance.LoanApplicationAssessmentId))
            throw new CisValidationException($"LoanApplicationAssessmentId is missing for SA #{saInstance.SalesArrangementId}");

        // instance Offer
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<_Offer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId!.Value, cancellationToken));

        /*if (request.NewAssessmentRequired) // vytvorit novy assessment
        {
            return null;
        }
        else // stary assessment
        {
            var result = ServiceCallResult.ResolveAndThrowIfError<LoanApplicationAssessmentResponse>(await _ripClient.GetLoanApplication(saInstance.LoanApplicationAssessmentId, new List<string>
            {
                "assessmentDetail",
                "householdAssessmentDetail",
                "counterpartyAssessmentDetail",
                "collateralRiskCharacteristics"
            }));

            var model = result.ToApiResponse();
            model.Application!.LoanAmount = offerInstance.SimulationResults.LoanAmount ?? 0;
            model.Application!.LoanPaymentAmount = offerInstance.SimulationResults.LoanPaymentAmount ?? 0;

            return model;
        }*/
        return null;
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
