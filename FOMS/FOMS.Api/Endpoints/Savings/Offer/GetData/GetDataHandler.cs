using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class GetDataHandler
    : IRequestHandler<Dto.GetDataRequest, Dto.GetDataResponse>
{
    public async Task<Dto.GetDataResponse> Handle(Dto.GetDataRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Get data for ${id}", request.OfferInstanceId);

        var result = resolveResult(await _offerService.GetBuildingSavingsData(request.OfferInstanceId, cancellationToken));
        
        var model = new Dto.GetDataResponse
        {
            BuildingSavings = result.BuildingSavings,
            Loan = result.Loan,
            InputData = (Dto.BuildingSavingsInput)result.InputData,
            OfferInstanceId = result.OfferInstanceId,
            CreatedTime = result.Created.DateTime,
            CreatedUserId = result.Created.UserId,
            CreatedUserName = result.Created.UserName,
        };
        
        _logger.LogDebug("Data from {time} resolved", model.CreatedTime);

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
