using DomainServices.OfferService.Clients.v1;
using NOBY.Services.MortgageRefinancing;
using NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPayment;

internal sealed class GetMortgageExtraPaymentHandler(
    IOfferServiceClient _offerService,
    MortgageRefinancingDataService _refinancingDataService)
    : IRequestHandler<GetMortgageExtraPaymentRequest, GetMortgageExtraPaymentResponse>
{
    public async Task<GetMortgageExtraPaymentResponse> Handle(GetMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        // sesbirat vsechna potrebna data
        var data = await _refinancingDataService.GetRefinancingData(request.CaseId, request.ProcessId, RefinancingTypes.MortgageExtraPayment, cancellationToken);

        // vytvorit a naplnit zaklad response modelu
        var response = data.UpdateBaseResponseModel(new GetMortgageExtraPaymentResponse());

        // extr payment specific data
        response.Document = await _refinancingDataService.CreateSigningDocument(data, RefinancingTypes.MortgageExtraPayment, data.Process?.MortgageExtraPayment?.DocumentEACode, data.Process?.MortgageExtraPayment?.DocumentId);
        response.IndividualPriceCommentLastVersion = data.SalesArrangement?.ExtraPayment?.IndividualPriceCommentLastVersion;
        response.ExtraPaymentAmount = (decimal?)data.Process!.MortgageExtraPayment?.ExtraPaymentAmount ?? 0M;
		response.ExtraPaymentAmountIncludingFee = (decimal?)data.Process!.MortgageExtraPayment?.ExtraPaymentAmountIncludingFee ?? 0M;
		response.ExtraPaymentDate = DateOnly.FromDateTime(data.Process.MortgageExtraPayment?.ExtraPaymentDate ?? DateTime.MinValue);
        response.IsExtraPaymentFullyRepaid = data.Process.MortgageExtraPayment?.IsFinalExtraPayment ?? false;
        response.PrincipalAmount = (decimal?)data.Process.MortgageExtraPayment?.ExtraPaymentAmount ?? 0M;
    
        // handover
        if (data.SalesArrangement?.ExtraPayment?.HandoverTypeDetailId != null)
        {
            response.Handover = new GetMortgageExtraPaymentResponse.HandoverObject
            {
                FirstName = data.SalesArrangement.ExtraPayment.Client?.FirstName ?? "",
                LastName = data.SalesArrangement.ExtraPayment.Client?.LastName ?? "",
                HandoverTypeDetailId = data.SalesArrangement.ExtraPayment.HandoverTypeDetailId.Value
            };
        }

        if ((data.Process.MortgageExtraPayment?.ExtraPaymentAgreements?.Count ?? 0) > 0)
        {
            response.Agreements = data.Process.MortgageExtraPayment!.ExtraPaymentAgreements
                .Select(t => new GetMortgageExtraPaymentResponse.AgreementDocument
                {
                    DocumentEACode = t.AgreementEACode.GetValueOrDefault(),
                    DocumentId = t.AgreementDocumentId
                })
                .ToList();
        }

        // aktivni IC
        if ((data.ActivePriceException?.Fees?.Count ?? 0) > 0)
        {
            response.FeeAmountDiscount = data.ActivePriceException!.Fees![0].TariffSum - data.ActivePriceException.Fees[0].FinalSum;
        }

        // pokud existuje Offer
        if (data.SalesArrangement?.OfferId is not null)
        {
            // detail offer
            var offerInstance = await _offerService.GetOffer(data.SalesArrangement.OfferId.Value, cancellationToken);
            response.SimulationResults = offerInstance.MortgageExtraPayment.SimulationResults.ToDto(offerInstance.Data.Created.DateTime, response.FeeAmountDiscount);

            response.ContainsInconsistentIndividualPriceData = offerInstance.MortgageExtraPayment.BasicParameters.FeeAmountDiscount != response.FeeAmountDiscount;
        }

        return response;
    }
}
