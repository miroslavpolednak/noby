using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetMortgageOfferDetailHandler
    : BaseHandler, IRequestHandler<Dto.GetMortgageOfferDetailMediatrRequest, GetMortgageOfferDetailResponse>
{
    #region Construction

    private readonly ILogger<GetMortgageOfferDetailHandler> _logger;

    public GetMortgageOfferDetailHandler(
        Repositories.OfferRepository repository,
        ILogger<GetMortgageOfferDetailHandler> logger,
        ICodebookServiceAbstraction codebookService) : base(repository, codebookService)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetMortgageOfferDetailResponse> Handle(Dto.GetMortgageOfferDetailMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _repository.Get(request.OfferId, cancellation);

        var simulationInputs = entity.SimulationInputs.ToSimulationInputs();

        var model = new GetMortgageOfferDetailResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
            BasicParameters = entity.BasicParameters.ToBasicParameters(),
            SimulationInputs = simulationInputs,
            SimulationResults = entity.SimulationResults.ToSimulationResults(),            
        };

        return model;
    }
  
}