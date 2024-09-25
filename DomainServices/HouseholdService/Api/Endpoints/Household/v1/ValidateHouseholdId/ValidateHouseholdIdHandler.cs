using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.v1.ValidateHouseholdId;

internal sealed class ValidateHouseholdIdHandler(HouseholdServiceDbContext _dbContext)
    : IRequestHandler<ValidateHouseholdIdRequest, ValidateHouseholdIdResponse>
{
    public async Task<ValidateHouseholdIdResponse> Handle(ValidateHouseholdIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _dbContext.Households
            .Where(t => t.HouseholdId == request.HouseholdId)
            .Select(t => new { t.SalesArrangementId, t.CaseId })
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ThrowExceptionIfNotFound && instance is null)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.HouseholdNotFound, request.HouseholdId);
        }

        return new ValidateHouseholdIdResponse
        {
            Exists = instance is not null,
            SalesArrangementId = instance?.SalesArrangementId,
            CaseId = instance?.CaseId
        };
    }
}