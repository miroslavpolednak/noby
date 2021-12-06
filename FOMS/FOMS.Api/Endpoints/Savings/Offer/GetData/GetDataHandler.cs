using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class GetDataHandler
    : IRequestHandler<Dto.GetDataRequest, Dto.GetDataResponse>
{
    

    public async Task<Dto.GetDataResponse> Handle(Dto.GetDataRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Get data for ${id}", request.OfferInstanceId);

        var result = resolveResult(await _offerService.GetBuildingSavingsData(request.OfferInstanceId));
        
        var model = new Dto.GetDataResponse
        {
            SimulationType = result.SimulationType,
            BuildingSavings = result.BuildingSavings,
            InputData = (Dto.BuildingSavingsInput)result.InputData,
            InsertTime = result.InsertStamp.DateTime,
            InsertUserId = result.InsertStamp.UserId,
            OfferInstanceId = result.OfferInstanceId,
        };
        if (result.SimulationType == DomainServices.OfferService.Contracts.SimulationTypes.BuildingSavingsWithLoan)
            model.Loan = result.Loan;

        _logger.LogDebug("Data from {time} resolved", model.InsertTime);

        return model;
    }

    private DomainServices.OfferService.Contracts.GetBuildingSavingsDataResponse resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.OfferService.Contracts.GetBuildingSavingsDataResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<GetDataHandler> _logger;

    public GetDataHandler(IOfferServiceAbstraction offerService, ILogger<GetDataHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
