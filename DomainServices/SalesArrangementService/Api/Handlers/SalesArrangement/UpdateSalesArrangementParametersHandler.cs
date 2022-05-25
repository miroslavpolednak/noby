using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement;

internal class UpdateSalesArrangementParametersHandler
    : IRequestHandler<Dto.UpdateSalesArrangementParametersMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementParametersMediatrRequest request, CancellationToken cancellation)
    {
        // existuje SA?
        if (!await _dbContext.SalesArrangements.AnyAsync(t => t.SalesArrangementId == request.Request.SalesArrangementId, cancellation))
            throw new CisNotFoundException(16000, $"Sales arrangement ID {request.Request.SalesArrangementId} does not exist.");

        // kontrolovat pokud je zmocnenec, tak zda existuje?
        if (request.Request.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage)
        {
            if (request.Request.Mortgage.Agent.HasValue)
            {
                if (!_dbContext.Customers.Any(t => t.SalesArrangementId == request.Request.SalesArrangementId && t.CustomerOnSAId == request.Request.Mortgage.Agent))
                    throw new CisNotFoundException(16000, $"Agent {request.Request.Mortgage.Agent} not found amoung customersOnSA for SAID {request.Request.SalesArrangementId}");
            }
        }

        // instance parametru, pokud existuje
        var entity = await _dbContext.SalesArrangementsParameters.FirstOrDefaultAsync(t => t.SalesArrangementId == request.Request.SalesArrangementId, cancellation);
        if (entity is null)
        {
            entity = new Repositories.Entities.SalesArrangementParameters
            {
                SalesArrangementId = request.Request.SalesArrangementId
            };
            _dbContext.SalesArrangementsParameters.Add(entity);
        }
        // naplnit parametry serializovanym objektem
        entity.Parameters = serializeParameters(request.Request);

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    static string? serializeParameters(Contracts.UpdateSalesArrangementParametersRequest request)
        => request.DataCase switch
        {
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage => request.Mortgage is null ? null : JsonSerializer.Serialize(request.Mortgage),
            _ => throw new NotImplementedException()
        };

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    
    public UpdateSalesArrangementParametersHandler(
        Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
