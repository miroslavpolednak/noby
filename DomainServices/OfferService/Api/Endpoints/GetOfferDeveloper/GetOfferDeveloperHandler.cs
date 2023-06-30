using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.GetOfferDeveloper;

internal sealed class GetOfferDeveloperHandler
    : IRequestHandler<GetOfferDeveloperRequest, GetOfferDeveloperResponse>
{
    public async Task<GetOfferDeveloperResponse> Handle(GetOfferDeveloperRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .Offers
            .AsNoTracking()
            .Where(t => t.OfferId == request.OfferId)
            .Select(t => new
            {
                t.SimulationInputsBin
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        var simulationInputs = MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin);
        var response = new GetOfferDeveloperResponse
        {
            DeveloperId = simulationInputs.Developer?.DeveloperId,
            ProjectId = simulationInputs.Developer?.ProjectId
        };

        if (simulationInputs.Developer?.DeveloperId != null && simulationInputs.Developer?.ProjectId != null)
        {
            response.IsDeveloperSet = true;

            var project = await _codebookService.GetDeveloperProject(simulationInputs.Developer.DeveloperId.Value, simulationInputs.Developer.ProjectId.Value, cancellationToken);

            response.IsDeveloperAllowed = _allowedMassValuations.Contains(project.MassEvaluation);
        }

        return response;
    }

    private static int[] _allowedMassValuations = new int[] { -1, 1 };

    private readonly ICodebookServiceClient _codebookService;
    private readonly Database.OfferServiceDbContext _dbContext;

    public GetOfferDeveloperHandler(Database.OfferServiceDbContext dbContext, ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
    }
}
