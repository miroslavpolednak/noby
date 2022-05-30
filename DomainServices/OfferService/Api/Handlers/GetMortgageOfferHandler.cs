using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;
using System.Text.Json;

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

        var simulationInputs = entity.SimulationInputs.ToSimulationInputs();

        // kontrola ProductTypeId (zda je typu Mortgage)d
        await CheckProductTypeCategory(
            simulationInputs.ProductTypeId,
            CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory.Mortgage,
            cancellation
        );

        var model = new GetMortgageOfferResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
            BasicParameters = entity.BasicParameters.ToBasicParameters(),
            SimulationInputs = simulationInputs,
            SimulationResults = entity.SimulationResults.ToBaseSimulationResults(),
        };

        return model;
    }
  
}