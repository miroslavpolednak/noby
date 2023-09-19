using DomainServices.CaseService.Clients;
using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Contracts;

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

        var covenant = await _repository.GetCovenant(request.CaseId, request.Order, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12024);

        return new()
        {
            Description = covenant.Description ?? string.Empty,
            Name = covenant.Name ?? string.Empty,
            Text = covenant.Text ?? string.Empty,
            FulfillDate = covenant.FulfillDate,
            IsFulfilled = (covenant.IsFulFilled ?? 0 ) != 0

        };
    }

    private readonly ICaseServiceClient _caseService;
    private readonly LoanRepository _repository;

    public GetCovenantDetailHandler(
        ICaseServiceClient caseService,
        LoanRepository repository)
    {
        _caseService = caseService;
        _repository = repository;
    }
}