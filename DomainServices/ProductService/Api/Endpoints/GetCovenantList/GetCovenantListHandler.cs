using DomainServices.CaseService.Clients;
using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantList;

internal sealed class GetCovenantListHandler : IRequestHandler<GetCovenantListRequest, GetCovenantListResponse>
{
    public async Task<GetCovenantListResponse> Handle(GetCovenantListRequest request, CancellationToken cancellationToken)
    {
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);

        // check if loan exists (against KonsDB)
        if (!await _repository.ExistsLoan(request.CaseId, cancellationToken))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.CaseId);
        }

        var covenants = await _repository.GetCovenants(request.CaseId, cancellationToken);
        var covenantPhases = await _repository.GetCovenantPhases(request.CaseId, cancellationToken);
        
        var response = new GetCovenantListResponse();
        response.Covenants.AddRange(covenants.Select(Map));
        response.Phases.AddRange(covenantPhases.Select(Map));
        
        return response;
    }

    private static CovenantListItem Map(Database.Models.Covenant covenant) => new()
    {
        Name = covenant.Name ?? string.Empty,
        FulfillDate = covenant.FulfillDate,
        IsFulfilled = (covenant.IsFulFilled ?? 0 ) != 0,
        Order = covenant.Order,
        OrderLetter = covenant.OrderLetter ?? string.Empty,
        PhaseOrder = covenant.PhaseOrder ?? 0,
        CovenantTypeId = covenant.CovenantTypeId ?? 0,
    };

    private static PhaseListItem Map(Database.Models.CovenantPhase covenantPhase) => new()
    {
        Name = covenantPhase.Name ?? string.Empty,
        Order = covenantPhase.Order,
        OrderLetter = covenantPhase.OrderLetter ?? string.Empty
    };
    
    private readonly ICaseServiceClient _caseService;
    private readonly LoanRepository _repository;

    public GetCovenantListHandler(
        ICaseServiceClient caseService,
        LoanRepository repository)
    {
        _caseService = caseService;
        _repository = repository;
    }
}