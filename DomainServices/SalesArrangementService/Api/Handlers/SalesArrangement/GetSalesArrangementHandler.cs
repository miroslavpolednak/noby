using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementHandler
    : IRequestHandler<Dto.GetSalesArrangementMediatrRequest, _SA.SalesArrangement>
{
    public async Task<_SA.SalesArrangement> Handle(Dto.GetSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        // detail SA
        var model = await _repository.GetSalesArrangement(request.SalesArrangementId, cancellation);

        // parametry
        string? parameters = await _dbContext.SalesArrangementsParameters
            .AsNoTracking()
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .Select(t => t.Parameters)
            .FirstOrDefaultAsync(cancellation);

        //TODO udelat rozdeleni podle typu produkt. Bude tady vubec rozdil mezi produkty?
        if (!string.IsNullOrEmpty(parameters))
            model.Mortgage = Google.Protobuf.JsonParser.Default.Parse<_SA.SalesArrangementParametersMortgage>(parameters!);

        return model;
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementHandler> _logger;
    
    public GetSalesArrangementHandler(
        Repositories.SalesArrangementServiceRepository repository,
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<GetSalesArrangementHandler> logger)
    {
        _dbContext = dbContext;
        _repository = repository;
        _logger = logger;
    }
}
