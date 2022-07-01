using _OS = DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetMortgageOfferDetailHandler
    : BaseHandler, IRequestHandler<Dto.GetMortgageOfferDetailMediatrRequest, _OS.GetMortgageOfferDetailResponse>
{
    #region Construction

    public GetMortgageOfferDetailHandler(
        Repositories.OfferRepository repository,
        ICodebookServiceAbstraction codebookService) : base(repository, codebookService)
    {
    }

    #endregion

    public async Task<_OS.GetMortgageOfferDetailResponse> Handle(Dto.GetMortgageOfferDetailMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _repository.Get(request.OfferId, cancellation);

        var model = new _OS.GetMortgageOfferDetailResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
            BasicParameters = _OS.BasicParameters.Parser.ParseFrom(entity.BasicParametersBin),
            SimulationInputs = _OS.MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin),
            SimulationResults = _OS.MortgageSimulationResults.Parser.ParseFrom(entity.SimulationResultsBin),
            AdditionalSimulationResults = _OS.AdditionalMortgageSimulationResults.Parser.ParseFrom(entity.AdditionalSimulationResultsBin)
        };

        return model;
    }
  
}