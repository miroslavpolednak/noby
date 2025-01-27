﻿using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.SbWebApi.V1;
using NOBY.Services.MortgageRefinancing;
using _contract = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRetentionDocument;

public class GenerateRetentionDocumentHandler(
    ISalesArrangementServiceClient _salesArrangementService,
    IOfferServiceClient _offerService,
    ICodebookServiceClient _codebookService,
    ISbWebApiClient _sbWebApi,
    MortgageRefinancingDocumentService _refinancingDocumentService)
        : IRequestHandler<RefinancingGenerateRetentionDocumentRequest>
{
    public async Task Handle(RefinancingGenerateRetentionDocumentRequest request, CancellationToken cancellationToken)
    {
        await ValidateSignatureTypeDetailId(request, cancellationToken);

        var salesArrangement = await _refinancingDocumentService.LoadAndValidateSA(request.SalesArrangementId, SalesArrangementTypes.MortgageRetention, cancellationToken);
        var offer = await LoadAndValidateOffer(salesArrangement.OfferId!.Value, cancellationToken);

        var offerIndividualPrice = new MortgageRefinancingIndividualPrice(offer.MortgageRetention.SimulationInputs.InterestRateDiscount, GetFeeDiscount(offer.MortgageRetention.BasicParameters));

        if (offerIndividualPrice.HasIndividualPrice && !await _refinancingDocumentService.IsIndividualPriceValid(salesArrangement, offerIndividualPrice, cancellationToken))
            throw new NobyValidationException(90048);

        await UpdateSaParams(request, salesArrangement, cancellationToken);

        await GenerateRetentionDocument(salesArrangement, offer, offerIndividualPrice.HasIndividualPrice, cancellationToken);

        await _salesArrangementService.UpdateSalesArrangementState(salesArrangement.SalesArrangementId, EnumSalesArrangementStates.InSigning, cancellationToken);
    }

    private async Task ValidateSignatureTypeDetailId(RefinancingGenerateRetentionDocumentRequest request, CancellationToken cancellationToken)
    {
        var signatureTypeDetail = (await _codebookService.SignatureTypeDetails(cancellationToken)).SingleOrDefault(s => s.Id == request.SignatureTypeDetailId);

        if (signatureTypeDetail?.IsRetentionAvailable != true)
            throw new NobyValidationException(90032);
    }

    private async Task<GetOfferResponse> LoadAndValidateOffer(int offerId, CancellationToken cancellationToken)
    {
        var offer = await _offerService.GetOffer(offerId, cancellationToken);

        if (((DateTime)offer.MortgageRetention.SimulationInputs.InterestRateValidFrom).AddDays(-14).Date < DateTime.UtcNow.Date)
            throw new NobyValidationException(90051);

        return offer;
    }

    private async Task GenerateRetentionDocument(_contract.SalesArrangement sa, GetOfferResponse offer, bool hasIndividualPrice, CancellationToken cancellationToken)
    {
        var user = await _refinancingDocumentService.LoadUserInfo(cancellationToken);

        var simulationInputs = offer.MortgageRetention.SimulationInputs;
        var simulationResults = offer.MortgageRetention.SimulationResults;
        var basicParams = offer.MortgageRetention.BasicParameters;

        await _sbWebApi.GenerateRetentionDocument(new ExternalServices.SbWebApi.Dto.Refinancing.GenerateRetentionDocumentRequest
        {
            CaseId = sa.CaseId,
            InterestRate = simulationInputs.InterestRate - ((decimal?)simulationInputs.InterestRateDiscount).GetValueOrDefault(),
            DateFrom = offer.MortgageRetention.SimulationInputs.InterestRateValidFrom,
            PaymentAmount = simulationResults.LoanPaymentAmountDiscounted ?? simulationResults.LoanPaymentAmount,
            SignatureTypeDetailId = sa.Retention.SignatureTypeDetailId,
            Cpm = user.Cpm,
            Icp = user.Icp,
            SignatureDeadline = sa.Retention.SignatureDeadline,
            IndividualPricing = hasIndividualPrice,
            Fee = basicParams.FeeAmountDiscounted ?? basicParams.FeeAmount
        }, cancellationToken);
    }

    private async Task UpdateSaParams(RefinancingGenerateRetentionDocumentRequest request, _contract.SalesArrangement sa, CancellationToken cancellationToken)
    {
        sa.Retention.SignatureTypeDetailId = request.SignatureTypeDetailId;
        sa.Retention.SignatureDeadline = request.SignatureDeadline;

        var saRequest = new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = sa.SalesArrangementId,
            Retention = sa.Retention
        };

        await _salesArrangementService.UpdateSalesArrangementParameters(saRequest, cancellationToken);
    }

    private static decimal? GetFeeDiscount(MortgageRetentionBasicParameters retention)
    {
        if (retention.FeeAmountDiscounted is null || (decimal)retention.FeeAmount - (decimal?)retention.FeeAmountDiscounted < 0)
            return default;

        return (decimal)retention.FeeAmount - (decimal?)retention.FeeAmountDiscounted;
    }
}
