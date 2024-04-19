using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.ValidateCustomerOnSAId;

internal sealed class ValidateCustomerOnSAIdHandler(HouseholdServiceDbContext _dbContext)
        : IRequestHandler<ValidateCustomerOnSAIdRequest, ValidateCustomerOnSAIdResponse>
{
    public async Task<ValidateCustomerOnSAIdResponse> Handle(ValidateCustomerOnSAIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _dbContext.Customers
            .Where(t => t.CustomerOnSAId ==  request.CustomerOnSAId)
            .Select(t => new { t.SalesArrangementId, t.CaseId })
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ThrowExceptionIfNotFound && instance is null)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId);
        }
        
        return new ValidateCustomerOnSAIdResponse
        {
            Exists = instance is not null,
            SalesArrangementId = instance?.SalesArrangementId,
            CaseId = instance?.CaseId
        };
    }
}