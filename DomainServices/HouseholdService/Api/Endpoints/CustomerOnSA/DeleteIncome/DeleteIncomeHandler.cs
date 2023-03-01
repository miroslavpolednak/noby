﻿using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteIncome;

internal sealed class DeleteIncomeHandler
    : IRequestHandler<DeleteIncomeRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .CustomersIncomes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.CustomerOnSAIncomeId == request.IncomeId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.IncomeNotFound, request.IncomeId);
        
        _dbContext.CustomersIncomes.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;
    
    public DeleteIncomeHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
