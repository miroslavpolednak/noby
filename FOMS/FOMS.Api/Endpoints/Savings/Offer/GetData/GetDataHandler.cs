using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class GetDataHandler
    : IRequestHandler<Dto.GetDataRequest, Dto.GetDataResponse>
{
    private readonly IOfferServiceAbstraction _offerService;

    public GetDataHandler(IOfferServiceAbstraction offerService)
    {
        _offerService = offerService;
    }

    public async Task<Dto.GetDataResponse> Handle(Dto.GetDataRequest request, CancellationToken cancellationToken)
    {
        var result = resolveResult(await _offerService.GetBuildingSavingsData(request.OfferInstanceId));
        
        var model = new Dto.GetDataResponse
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
