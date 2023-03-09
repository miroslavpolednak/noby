using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOffer;

internal sealed class GetMortgageOfferHandler
    : IRequestHandler<GetMortgageOfferRequest, GetMortgageOfferResponse>
{
    public async Task<GetMortgageOfferResponse> Handle(GetMortgageOfferRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
           .Offers
           .AsNoTracking()
           .Where(t => t.OfferId == request.OfferId)
           .Select(t => new
           {
               t.ResourceProcessId,
               t.CreatedUserId,
               t.CreatedUserName,
               t.CreatedTime,
               t.BasicParametersBin,
               t.SimulationInputsBin,
               t.SimulationResultsBin
           })
           .FirstOrDefaultAsync(cancellationToken) 
           ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        var model = new GetMortgageOfferResponse
        {
            OfferId = request.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity.CreatedUserId, entity.CreatedUserName, entity.CreatedTime),
            BasicParameters = BasicParameters.Parser.ParseFrom(entity.BasicParametersBin),
            SimulationInputs = MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin),
            SimulationResults = MortgageSimulationResults.Parser.ParseFrom(entity.SimulationResultsBin)
        };

        return model;
    }

    private readonly Database.OfferServiceDbContext _dbContext;

    public GetMortgageOfferHandler(Database.OfferServiceDbContext dbContext)
        => _dbContext = dbContext;
}