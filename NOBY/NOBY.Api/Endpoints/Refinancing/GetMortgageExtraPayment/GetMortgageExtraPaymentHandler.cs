using DomainServices.OfferService.Clients.v1;
using NOBY.Services.MortgageRefinancing;
using NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPayment;

internal sealed class GetMortgageExtraPaymentHandler(
    IOfferServiceClient _offerService,
    MortgageRefinancingWorkflowService _refinancingWorkflowService)
    : IRequestHandler<GetMortgageExtraPaymentRequest, GetMortgageExtraPaymentResponse>
{
    public async Task<GetMortgageExtraPaymentResponse> Handle(GetMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        var extraPaymentData = await _refinancingWorkflowService.GetRefinancingData(request.CaseId, request.ProcessId, RefinancingTypes.MortgageExtraPayment, cancellationToken);

        var response = new GetMortgageExtraPaymentResponse
        {
            RefinancingStateId = (int)extraPaymentData.RefinancingState,
            SalesArrangementId = extraPaymentData.SalesArrangement?.SalesArrangementId,
            IsReadOnly = extraPaymentData.RefinancingState != RefinancingStates.RozpracovanoVNoby,
            Tasks = extraPaymentData.Tasks,
            IndividualPriceCommentLastVersion = extraPaymentData.SalesArrangement?.Retention?.IndividualPriceCommentLastVersion,
            ExtraPaymentAmount = (decimal?)extraPaymentData.Process!.MortgageExtraPayment?.ExtraPaymentAmount ?? 0M,
            ExtraPaymentDate = (DateTime?)extraPaymentData.Process.MortgageExtraPayment?.ExtraPaymentDate ?? DateTime.Now,
            IsExtraPaymentFullyRepaid = extraPaymentData.Process.MortgageExtraPayment?.IsFinalExtraPayment ?? false,
            PrincipalAmount = (decimal?)extraPaymentData.Process.MortgageExtraPayment?.ExtraPaymentAmount ?? 0M,
            IsPriceExceptionActive = extraPaymentData.ActivePriceException is not null
        };

        // handover
        if (extraPaymentData.SalesArrangement?.ExtraPayment?.HandoverTypeDetailId != null)
        {
            response.Handover = new GetMortgageExtraPaymentResponse.HandoverObject
            {
                FirstName = extraPaymentData.SalesArrangement.ExtraPayment.Client?.FirstName ?? "",
                LastName = extraPaymentData.SalesArrangement.ExtraPayment.Client?.LastName ?? "",
                HandoverTypeDetailId = extraPaymentData.SalesArrangement.ExtraPayment.HandoverTypeDetailId.Value
            };
        }

        if (!string.IsNullOrEmpty(extraPaymentData.Process.MortgageExtraPayment?.DocumentId))
        {
            response.Document = new()
            {
                DocumentId = extraPaymentData.Process.MortgageExtraPayment.DocumentId,
                DocumentEACode = extraPaymentData.Process.MortgageExtraPayment.DocumentEACode ?? 0
            };
        }

        if ((extraPaymentData.Process.MortgageExtraPayment?.ExtraPaymentAgreements?.Count ?? 0) > 0)
        {
            response.Agreements = extraPaymentData.Process.MortgageExtraPayment!.ExtraPaymentAgreements
                .Select(t => new Dto.Refinancing.RefinancingDocument
                {
                    DocumentEACode = t.AgreementEACode.GetValueOrDefault(),
                    DocumentId = t.AgreementDocumentId
                })
                .ToList();
        }

        // aktivni IC
        if ((extraPaymentData.ActivePriceException?.Fees?.Count ?? 0) != 0)
        {
            response.FeeAmountDiscount = extraPaymentData.ActivePriceException!.Fees![0].TariffSum - extraPaymentData.ActivePriceException.Fees[0].FinalSum;
        }

        // pokud existuje Offer
        if (extraPaymentData.SalesArrangement?.OfferId is not null)
        {
            // detail offer
            var offerInstance = await _offerService.GetOffer(extraPaymentData.SalesArrangement.OfferId.Value, cancellationToken);
            response.SimulationResults = offerInstance.MortgageExtraPayment.SimulationResults.ToDto(offerInstance.Data.Created.DateTime, response.FeeAmountDiscount);

            response.ContainsInconsistentIndividualPriceData = offerInstance.MortgageRetention.BasicParameters.FeeAmountDiscounted != response.FeeAmountDiscount;
        }

        return response;
    }
}
