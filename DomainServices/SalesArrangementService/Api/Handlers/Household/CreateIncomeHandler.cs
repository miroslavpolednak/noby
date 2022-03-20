﻿using DomainServices.SalesArrangementService.Contracts;
using System.Text.Json;

namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class CreateIncomeHandler
    : IRequestHandler<Dto.CreateIncomeMediatrRequest, CreateIncomeResponse>
{
    public async Task<CreateIncomeResponse> Handle(Dto.CreateIncomeMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(CreateIncomeHandler));

        var entity = new Repositories.Entities.CustomerIncome
        {
            CustomerOnSAId = request.Request.CustomerOnSAId,
            Sum = request.Request.Sum,
            CurrencyId = request.Request.CurrencyId,
            IncomeTypeId = (CIS.Foms.Enums.CustomerIncomeTypes)request.Request.IncomeTypeId,
            Data = JsonSerializer.Serialize(getDataObject(request.Request))
        };

        _dbContext.CustomersIncomes.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.CustomerIncome), entity.CustomerIncomeId);

        return new CreateIncomeResponse
        {
            IncomeId = entity.CustomerIncomeId
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
