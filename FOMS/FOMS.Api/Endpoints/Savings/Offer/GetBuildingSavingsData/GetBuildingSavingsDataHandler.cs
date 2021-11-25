using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class GetBuildingSavingsDataHandler
    : IRequestHandler<Dto.GetBuildingSavingsDataRequest, Dto.GetBuildingSavingsDataResponse>
{
    private readonly IOfferServiceAbstraction _offerService;

    public GetBuildingSavingsDataHandler(IOfferServiceAbstraction offerService)
    {
        _offerService = offerService;
    }

    public async Task<Dto.GetBuildingSavingsDataResponse> Handle(Dto.GetBuildingSavingsDataRequest request, CancellationToken cancellationToken)
    {
        var result = resolveResult(await _offerService.GetBuildingSavingsData(request.OfferInstanceId));
        
        var model = new Dto.GetBuildingSavingsDataResponse
        {
            SimulationType = result.SimulationType,
            BuildingSavings = result.BuildingSavings,
            InputData = result.InputData,
            InsertTime = result.InsertStamp.DateTime,
            InsertUserId = result.InsertStamp.UserId,
            OfferInstanceId = result.OfferInstanceId,
        };
        if (result.SimulationType == DomainServices.OfferService.Contracts.SimulationTypes.BuildingSavingsWithLoan)
            model.Loan = result.Loan;

        return model;
    }

    private DomainServices.OfferService.Contracts.GetBuildingSavingsDataResponse resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.OfferService.Contracts.GetBuildingSavingsDataResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };
}
