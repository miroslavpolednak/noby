using CIS.Foms.Enums;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.ValidateCaseId;

internal sealed class ValidateCaseIdHandler
    : IRequestHandler<ValidateCaseIdRequest, ValidateCaseIdResponse>
{
    public async Task<ValidateCaseIdResponse> Handle(ValidateCaseIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _dbContext.Cases
            .Where(t => t.CaseId ==  request.CaseId)
            .Select(t => new { t.State, t.OwnerUserId })
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ThrowExceptionIfNotFound && instance is null)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        }
        else if (instance is not null && _disallowedStates.Contains(instance!.State))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CaseCancelled);
        }
        else
        {
            return new ValidateCaseIdResponse
            {
                Exists = instance is not null,
                OwnerUserId = instance?.OwnerUserId
            };
        }
    }

    private static int[] _disallowedStates = new[]
    {
        (int)CaseStates.ToBeCancelledConfirmed
    };

    private readonly CaseServiceDbContext _dbContext;

    public ValidateCaseIdHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
