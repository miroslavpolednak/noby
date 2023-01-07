using Microsoft.EntityFrameworkCore;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal sealed class GetSalesArrangementHandler
    : IRequestHandler<Dto.GetSalesArrangementMediatrRequest, _SA.SalesArrangement>
{
    public async Task<_SA.SalesArrangement> Handle(Dto.GetSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        // detail SA
        var model = await _repository.GetSalesArrangement(request.SalesArrangementId, cancellation);

        // parametry
        var parameters = await _dbContext.SalesArrangementsParameters
            .AsNoTracking()
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .Select(t => new { ParameterType = t.SalesArrangementParametersType, Bin = t.ParametersBin })
            .FirstOrDefaultAsync(cancellation);

        //TODO udelat rozdeleni podle typu produkt. Bude tady vubec rozdil mezi produkty?
        if (parameters?.Bin is not null && parameters.Bin.Length > 0)
        {
            switch (parameters.ParameterType)
            {
                case Repositories.Entities.SalesArrangementParametersTypes.Mortgage:
                    model.Mortgage = _SA.SalesArrangementParametersMortgage.Parser.ParseFrom(parameters.Bin);
                    break;
                case Repositories.Entities.SalesArrangementParametersTypes.Drawing:
                    model.Drawing = _SA.SalesArrangementParametersDrawing.Parser.ParseFrom(parameters.Bin);
                    break;
                case Repositories.Entities.SalesArrangementParametersTypes.GeneralChange:
                    model.GeneralChange = _SA.SalesArrangementParametersGeneralChange.Parser.ParseFrom(parameters.Bin);
                    break;
                default:
                    throw new NotImplementedException($"SalesArrangementParametersType {parameters.ParameterType} is not implemented");
            }
        }
        
        return model;
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    
    public GetSalesArrangementHandler(
        Repositories.SalesArrangementServiceRepository repository,
        Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
        _repository = repository;
    }
}
