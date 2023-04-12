using DomainServices.SalesArrangementService.Api.Database;
using Microsoft.EntityFrameworkCore;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangement;

internal sealed class GetSalesArrangementHandler
    : IRequestHandler<__SA.GetSalesArrangementRequest, __SA.SalesArrangement>
{
    public async Task<__SA.SalesArrangement> Handle(__SA.GetSalesArrangementRequest request, CancellationToken cancellation)
    {
        // detail SA
        var model = await _dbContext.SalesArrangements
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .AsNoTracking()
            .Select(DatabaseExpressions.SalesArrangementDetail())
            .FirstOrDefaultAsync(cancellation) 
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

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
                case SalesArrangementTypes.Mortgage:
                    model.Mortgage = __SA.SalesArrangementParametersMortgage.Parser.ParseFrom(parameters.Bin);
                    break;
                case SalesArrangementTypes.Drawing:
                    model.Drawing = __SA.SalesArrangementParametersDrawing.Parser.ParseFrom(parameters.Bin);
                    break;
                case SalesArrangementTypes.GeneralChange:
                    model.GeneralChange = __SA.SalesArrangementParametersGeneralChange.Parser.ParseFrom(parameters.Bin);
                    break;
                case SalesArrangementTypes.HUBN:
                    model.HUBN = __SA.SalesArrangementParametersHUBN.Parser.ParseFrom(parameters.Bin);
                    break;
                case SalesArrangementTypes.CustomerChange:
                    model.HUBN = __SA.SalesArrangementParametersHUBN.Parser.ParseFrom(parameters.Bin);
                    break;
                case SalesArrangementTypes.CustomerChange3602A:
                    model.CustomerChange3602A = __SA.SalesArrangementParametersCustomerChange3602.Parser.ParseFrom(parameters.Bin);
                    break;
                case SalesArrangementTypes.CustomerChange3602B:
                    model.CustomerChange3602B = __SA.SalesArrangementParametersCustomerChange3602.Parser.ParseFrom(parameters.Bin);
                    break;
                case SalesArrangementTypes.CustomerChange3602C:
                    model.CustomerChange3602C = __SA.SalesArrangementParametersCustomerChange3602.Parser.ParseFrom(parameters.Bin);
                    break;
                default:
                    throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SATypeNotSupported, parameters.ParameterType);
            }
        }

        return model;
    }

    private readonly SalesArrangementServiceDbContext _dbContext;

    public GetSalesArrangementHandler(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
