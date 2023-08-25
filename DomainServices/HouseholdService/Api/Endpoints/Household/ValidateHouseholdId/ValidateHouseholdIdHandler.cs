using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.ValidateHouseholdId;

internal sealed class ValidateHouseholdIdHandler
    : IRequestHandler<ValidateHouseholdIdRequest, ValidateHouseholdIdResponse>
{
    public async Task<ValidateHouseholdIdResponse> Handle(ValidateHouseholdIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _dbContext.Households
            .Where(t => t.HouseholdId ==  request.HouseholdId)
            .Select(t => new { t.SalesArrangementId, t.CaseId })
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ThrowExceptionIfNotFound && instance is null)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.HouseholdNotFound, request.HouseholdId);
        }
        
        return new ValidateHouseholdIdResponse
        {
            Exists = instance is not null,
            SalesArrangementId = instance?.SalesArrangementId,
            CaseId = instance?.CaseId
        };
    }

    private readonly HouseholdServiceDbContext _dbContext;
    
    public ValidateHouseholdIdHandler(HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}