using DomainServices.CaseService.Clients;
using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantDetail;

internal sealed class GetCovenantDetailHandler : IRequestHandler<GetCovenantDetailRequest, GetCovenantDetailResponse>
{
    public async Task<GetCovenantDetailResponse> Handle(GetCovenantDetailRequest request, CancellationToken cancellationToken)
    {
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);

        // check if loan exists (against KonsDB)
        if (!await _repository.ExistsLoan(request.CaseId, cancellationToken))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.CaseId);
        }

        var covenant = await _dbContext.Covenants
            .Where(c => c.CaseId == request.CaseId && c.Order == request.Order)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12024);
        
        return new()
        {
            Description = covenant.Description,
            Name = covenant.Name,
            Text = covenant.Text,
            FulfillDate = covenant.FulfillDate,
            IsFulfilled = covenant.IsFulFilled != 0

        };
    }

    private readonly ProductServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseService;
    private readonly LoanRepository _repository;

    public GetCovenantDetailHandler(
        ProductServiceDbContext dbContext,
        ICaseServiceClient caseService,
        LoanRepository repository)
    {
        _dbContext = dbContext;
        _caseService = caseService;
        _repository = repository;
    }
}