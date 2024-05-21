using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangementId;

internal sealed class ValidateSalesArrangementIdHandler(SalesArrangementServiceDbContext _dbContext)
		: IRequestHandler<ValidateSalesArrangementIdRequest, ValidateSalesArrangementIdResponse>
{
    public async Task<ValidateSalesArrangementIdResponse> Handle(ValidateSalesArrangementIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _dbContext.SalesArrangements
            .Where(t => t.SalesArrangementId ==  request.SalesArrangementId)
            .Select(t => new { t.State, t.CaseId, t.SalesArrangementTypeId })
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ThrowExceptionIfNotFound && instance is null)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);
        }
        
        return new ValidateSalesArrangementIdResponse
        {
            Exists = instance is not null,
            CaseId = instance?.CaseId,
            State = instance?.State,
            SalesArrangementTypeId = instance?.SalesArrangementTypeId,
        };
    }
}