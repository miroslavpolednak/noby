using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetMortgageOfferDetailHandler
    : BaseHandler, IRequestHandler<Dto.GetMortgageOfferDetailMediatrRequest, GetMortgageOfferDetailResponse>
{
    #region Construction

    public GetMortgageOfferDetailHandler(
        Repositories.OfferRepository repository,
        ICodebookServiceAbstraction codebookService) : base(repository, codebookService)
    {
    }

    #endregion

    public async Task<GetMortgageOfferDetailResponse> Handle(Dto.GetMortgageOfferDetailMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _repository.Get(request.OfferId, cancellation);

        var model = new GetMortgageOfferDetailResponse
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