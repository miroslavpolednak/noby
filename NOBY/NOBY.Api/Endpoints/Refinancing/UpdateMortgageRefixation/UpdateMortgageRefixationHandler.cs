﻿using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Services.MortgageRefinancing;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Refinancing.UpdateMortgageRefixation;

internal sealed class UpdateMortgageRefixationHandler(
    IOfferServiceClient _offerService,
    ISalesArrangementServiceClient _salesArrangementService,
    ApiServices.MortgageRefinancingSalesArrangementCreateService _salesArrangementCreateService,
    MortgageRefinancingWorkflowService _retentionWorkflowService)
        : IRequestHandler<RefinancingUpdateMortgageRefixationRequest, RefinancingSharedOfferLinkResult>
{
    public async Task<RefinancingSharedOfferLinkResult> Handle(RefinancingUpdateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        decimal? interestRateDiscount = request.InterestRateDiscount == 0 ? null : request.InterestRateDiscount;

        _retentionWorkflowService.ValidateIndividualPriceExceptionComment(request.IndividualPriceCommentLastVersion, interestRateDiscount, default);

        // ziskat existujici nebo zalozit novy SA
        var salesArrangement = await _salesArrangementCreateService.GetOrCreateSalesArrangement(request.CaseId, SalesArrangementTypes.MortgageRefixation, cancellationToken);

        var mortgageParameters = new MortgageRefinancingWorkflowParameters
        {
            CaseId = salesArrangement.CaseId,
            ProcessId = salesArrangement.ProcessId!.Value,
            LoanInterestRateDiscount = interestRateDiscount
        };

        // vytvorit / updatovat IC task pokud je treba
        await _retentionWorkflowService.CreateIndividualPriceWorkflowTask(mortgageParameters, request.IndividualPriceCommentLastVersion, cancellationToken);

        // ulozit SA params refixace (poznamky)
        await updateSalesArrangementParameters(request, salesArrangement, cancellationToken);

        // presimulovat modelace
        await updateOffers(request, interestRateDiscount, cancellationToken);

        return new()
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            ProcessId = salesArrangement.ProcessId!.Value
        };
    }

    private async Task updateOffers(RefinancingUpdateMortgageRefixationRequest request, decimal? interestRateDiscount, CancellationToken cancellationToken)
    {
        var offers = await _offerService.GetOfferList(request.CaseId, OfferTypes.MortgageRefixation, false, cancellationToken: cancellationToken);

        foreach (var offer in offers)
        {
            // mame ulozenou jinou slevu ze sazby nez je v requestu
            if (!((EnumOfferFlagTypes)offer.Data.Flags).HasFlag(EnumOfferFlagTypes.LegalNotice)
                && offer.MortgageRefixation.SimulationInputs.InterestRateDiscount != interestRateDiscount)
            {
                var simulationRequest = new SimulateMortgageRefixationRequest
                {
                    CaseId = request.CaseId,
                    OfferId = offer.Data.OfferId,
                    BasicParameters = offer.MortgageRefixation.BasicParameters,
                    SimulationInputs = offer.MortgageRefixation.SimulationInputs
                };
                simulationRequest.SimulationInputs.InterestRateDiscount = interestRateDiscount;

                // presimulovat
                await _offerService.SimulateMortgageRefixation(simulationRequest, cancellationToken);
            }
        }
    }

    private Task updateSalesArrangementParameters(RefinancingUpdateMortgageRefixationRequest request, _SA salesArrangement, CancellationToken cancellationToken)
    {
        salesArrangement.Refixation.IndividualPriceCommentLastVersion = request.IndividualPriceCommentLastVersion;
        salesArrangement.Refixation.Comment = request.Comment;

        return _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Refixation = salesArrangement.Refixation
        }, cancellationToken);
    }
}
