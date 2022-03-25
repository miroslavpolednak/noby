using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class UpdateIncomeBaseDataHander
    : IRequestHandler<Dto.UpdateIncomeBaseDataMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateIncomeBaseDataMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateIncomeBaseDataHander), request.Request.IncomeId);

        var entity = (await _dbContext.CustomersIncomes
            .Where(t => t.CustomerIncomeId == request.Request.IncomeId)
            .FirstOrDefaultAsync(cancellation)) ?? throw new CisNotFoundException(16029, $"Income ID {request.Request.IncomeId} does not exist.");

        // base data
        entity.Sum = request.Request.BaseData?.Sum;
        entity.CurrencyCode = request.Request.BaseData?.CurrencyCode;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<UpdateIncomeBaseDataHander> _logger;

    public UpdateIncomeBaseDataHander(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<UpdateIncomeBaseDataHander> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
