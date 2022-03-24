using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement;

internal class UpdateSalesArrangementParametersHandler
    : IRequestHandler<Dto.UpdateSalesArrangementParametersMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementParametersMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateSalesArrangementParametersHandler), request.Request.SalesArrangementId);

        // existuje SA?
        if (!await _dbContext.SalesArrangements.AnyAsync(t => t.SalesArrangementId == request.Request.SalesArrangementId, cancellation))
            throw new CisNotFoundException(16000, $"Sales arrangement ID {request.Request.SalesArrangementId} does not exist.");

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
        entity.Parameteres = serializeParameters(request.Request);

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    static string? serializeParameters(Contracts.UpdateSalesArrangementParametersRequest request)
        => request.DataCase switch
        {
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage => request.Mortgage is null ? null : System.Text.Json.JsonSerializer.Serialize(request.Mortgage),
            _ => throw new NotImplementedException()
        };

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<UpdateSalesArrangementParametersHandler> _logger;

    public UpdateSalesArrangementParametersHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<UpdateSalesArrangementParametersHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
