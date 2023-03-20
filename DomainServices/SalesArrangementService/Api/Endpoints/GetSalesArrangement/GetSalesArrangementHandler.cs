using Microsoft.EntityFrameworkCore;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangement;

internal sealed class GetSalesArrangementHandler
    : IRequestHandler<__SA.GetSalesArrangementRequest, __SA.SalesArrangement>
{
    public async Task<__SA.SalesArrangement> Handle(__SA.GetSalesArrangementRequest request, CancellationToken cancellation)
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
                case CIS.Foms.Types.Enums.SalesArrangementTypes.Mortgage:
                    model.Mortgage = __SA.SalesArrangementParametersMortgage.Parser.ParseFrom(parameters.Bin);
                    break;
                case CIS.Foms.Types.Enums.SalesArrangementTypes.Drawing:
                    model.Drawing = __SA.SalesArrangementParametersDrawing.Parser.ParseFrom(parameters.Bin);
                    break;
                case CIS.Foms.Types.Enums.SalesArrangementTypes.GeneralChange:
                    model.GeneralChange = __SA.SalesArrangementParametersGeneralChange.Parser.ParseFrom(parameters.Bin);
                    break;
                case CIS.Foms.Types.Enums.SalesArrangementTypes.HUBN:
                    model.HUBN = __SA.SalesArrangementParametersHUBN.Parser.ParseFrom(parameters.Bin);
                    break;
                case CIS.Foms.Types.Enums.SalesArrangementTypes.CustomerChange:
                    model.HUBN = __SA.SalesArrangementParametersHUBN.Parser.ParseFrom(parameters.Bin);
                    break;
                default:
                    throw new NotImplementedException($"SalesArrangementParametersType {parameters.ParameterType} is not implemented");
            }
        }

        return model;
    }

    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly Database.SalesArrangementServiceRepository _repository;

    public GetSalesArrangementHandler(
        Database.SalesArrangementServiceRepository repository,
        Database.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
        _repository = repository;
    }
}
