using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class CreateIncomeHandler
    : IRequestHandler<Dto.CreateIncomeMediatrRequest, CreateIncomeResponse>
{
    public async Task<CreateIncomeResponse> Handle(Dto.CreateIncomeMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(CreateIncomeHandler));

        // check customer existence
        if (!await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId, cancellation))
            throw new CisNotFoundException(16020, "CustomerOnSA", request.Request.CustomerOnSAId);

        var entity = new Repositories.Entities.CustomerOnSAIncome
        {
            CustomerOnSAId = request.Request.CustomerOnSAId,
            Sum = request.Request.BaseData?.Sum,
            CurrencyCode = request.Request.BaseData?.CurrencyCode,
            IncomeTypeId = (CIS.Foms.Enums.CustomerIncomeTypes)request.Request.IncomeTypeId,
            Data = JsonSerializer.Serialize(getDataObject(request.Request), GrpcHelpers.GrpcJsonSerializerOptions)
        };

        _dbContext.CustomersIncomes.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.CustomerOnSAIncome), entity.CustomerOnSAIncomeId);

        return new CreateIncomeResponse
        {
            IncomeId = entity.CustomerOnSAIncomeId
        };
    }

    static object getDataObject(CreateIncomeRequest request)
        => (CIS.Foms.Enums.CustomerIncomeTypes)request.IncomeTypeId switch
        {
            CIS.Foms.Enums.CustomerIncomeTypes.Employement => request.Employement,
            _ => throw new NotImplementedException("This customer income type serializer is not implemented")
        };

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<CreateIncomeHandler> _logger;

    public CreateIncomeHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<CreateIncomeHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
