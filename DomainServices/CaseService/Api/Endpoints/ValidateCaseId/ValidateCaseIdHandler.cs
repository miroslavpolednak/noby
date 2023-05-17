using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.ValidateCaseId;

internal sealed class ValidateCaseIdHandler
    : IRequestHandler<ValidateCaseIdRequest, ValidateCaseIdResponse>
{
    public async Task<ValidateCaseIdResponse> Handle(ValidateCaseIdRequest request, CancellationToken cancellationToken)
    {
        bool exists = await _dbContext.Cases.AnyAsync(t => t.CaseId ==  request.CaseId, cancellationToken);

        return new ValidateCaseIdResponse
        {
            Exists = exists
        };
    }

    private readonly CaseServiceDbContext _dbContext;

    public ValidateCaseIdHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
