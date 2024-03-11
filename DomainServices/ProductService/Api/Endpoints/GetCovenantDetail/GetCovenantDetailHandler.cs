using DomainServices.CaseService.Clients.v1;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantDetail;

internal sealed class GetCovenantDetailHandler : IRequestHandler<GetCovenantDetailRequest, GetCovenantDetailResponse>
{
    private readonly ICaseServiceClient _caseService;
    private readonly LoanRepository _repository;

    public GetCovenantDetailHandler(
        ICaseServiceClient caseService,
        LoanRepository repository)
    {
        _caseService = caseService;
        _repository = repository;
    }

    public async Task<GetCovenantDetailResponse> Handle(GetCovenantDetailRequest request, CancellationToken cancellationToken)
    {
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);
    
        // check if loan exists (against KonsDB)
        if (!await _repository.LoanExists(request.CaseId, cancellationToken))
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.CaseId);

        var covenant = await _repository.GetCovenant(request.CaseId, request.Order, cancellationToken)
                       ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12024);

        return new GetCovenantDetailResponse
        {
            Description = covenant.Description ?? string.Empty,
            Name = covenant.Name ?? string.Empty,
            Text = covenant.Text ?? string.Empty,
            FulfillDate = covenant.FulfillDate,
            IsFulfilled = (covenant.IsFulFilled ?? 0 ) != 0
        };
    }
}