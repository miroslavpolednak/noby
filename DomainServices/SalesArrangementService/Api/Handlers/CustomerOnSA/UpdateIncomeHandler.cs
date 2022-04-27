using _SA = DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class UpdateIncomeHandler
    : IRequestHandler<Dto.UpdateIncomeMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateIncomeMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateIncomeHandler), request.Request.IncomeId);

        var entity = (await _dbContext.CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.Request.IncomeId)
            .FirstOrDefaultAsync(cancellation)) ?? throw new CisNotFoundException(16029, $"Income ID {request.Request.IncomeId} does not exist.");
        
        entity.Sum = request.Request.BaseData?.Sum;
        entity.CurrencyCode = request.Request.BaseData?.CurrencyCode;
        entity.Data = JsonSerializer.Serialize(getDataObject(entity.IncomeTypeId, request.Request));

        await _dbContext.SaveChangesAsync(cancellation);
        
        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    static object getDataObject(CIS.Foms.Enums.CustomerIncomeTypes incomeType, _SA.UpdateIncomeRequest request)
        => incomeType switch
        {
            CIS.Foms.Enums.CustomerIncomeTypes.Employement => request.Employement,
            CIS.Foms.Enums.CustomerIncomeTypes.Other => request.Other,
            _ => throw new NotImplementedException("This customer income type serializer is not implemented")
        };

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<UpdateIncomeHandler> _logger;

    public UpdateIncomeHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<UpdateIncomeHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
