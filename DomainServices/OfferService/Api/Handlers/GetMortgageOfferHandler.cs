using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetMortgageOfferHandler
    : BaseHandler, IRequestHandler<Dto.GetMortgageOfferMediatrRequest, GetMortgageOfferResponse>
{
    #region Construction

    private readonly ILogger<GetMortgageOfferHandler> _logger;

    public GetMortgageOfferHandler(
        Repositories.OfferRepository repository,
        ILogger<GetMortgageOfferHandler> logger,
        ICodebookServiceAbstraction codebookService) : base(repository, codebookService)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetMortgageOfferResponse> Handle(Dto.GetMortgageOfferMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _repository.Get(request.OfferId, cancellation);

        var model = new GetMortgageOfferResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
            BasicParameters = BasicParameters.Parser.ParseFrom(entity.BasicParametersBin),
            SimulationInputs = SimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin),
            SimulationResults = SimulationResults.Parser.ParseFrom(entity.SimulationResultsBin)
        };

        return model;
    }
  
}