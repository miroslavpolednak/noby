using _SA = DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using Google.Protobuf;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal sealed class UpdateIncomeHandler
    : IRequestHandler<Dto.UpdateIncomeMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateIncomeMediatrRequest request, CancellationToken cancellation)
    {
        var entity = (await _dbContext.CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.Request.IncomeId)
            .FirstOrDefaultAsync(cancellation)) ?? throw new CisNotFoundException(16029, $"Income ID {request.Request.IncomeId} does not exist.");

        var dataObject = getDataObject(entity.IncomeTypeId, request.Request);
        entity.Sum = request.Request.BaseData?.Sum;
        entity.CurrencyCode = request.Request.BaseData?.CurrencyCode;
        entity.Data = Newtonsoft.Json.JsonConvert.SerializeObject((object)dataObject);
        entity.DataBin = dataObject?.ToByteArray();

        await _dbContext.SaveChangesAsync(cancellation);
        
        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    static IMessage getDataObject(CIS.Foms.Enums.CustomerIncomeTypes incomeType, _SA.UpdateIncomeRequest request)
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
