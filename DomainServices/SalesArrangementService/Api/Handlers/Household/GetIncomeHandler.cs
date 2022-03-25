using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class GetIncomeHandler
    : IRequestHandler<Dto.GetIncomeMediatrRequest, Income>
{
    public async Task<Income> Handle(Dto.GetIncomeMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetIncomeHandler), request.IncomeId);

        var entity = await _dbContext.CustomersIncomes
            .Where(t => t.CustomerIncomeId == request.IncomeId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16029, $"Income ID {request.IncomeId} does not exist.");

        var model = new Income
        {
            IncomeId = entity.CustomerIncomeId,
            IncomeTypeId = (int)entity.IncomeTypeId,
            CustomerOnSAId = entity.CustomerOnSAId,
            BaseData = new IncomeBaseData
            {
                CurrencyCode = entity.CurrencyCode,
                Sum = entity.Sum
            }
        };

        if (!string.IsNullOrEmpty(entity.Data))
        {
            switch (entity.IncomeTypeId)
            {
                case CIS.Foms.Enums.CustomerIncomeTypes.Employement:
                    model.Employement = JsonSerializer.Deserialize<IncomeDataEmployement>(entity.Data!, GrpcHelpers.GrpcJsonSerializerOptions);
                    break;
                default:
                    throw new NotImplementedException("This customer income type deserializer is not implemented");
            }
        }
        
        return model;
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<GetIncomeHandler> _logger;

    public GetIncomeHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<GetIncomeHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
