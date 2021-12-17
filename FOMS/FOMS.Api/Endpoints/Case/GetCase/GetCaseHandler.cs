using CIS.Core.Results;

namespace FOMS.Api.Endpoints.Case.Handlers;

internal class GetCaseHandler
    : IRequestHandler<Dto.GetCaseRequest, Dto.GetCaseResponse>
{
    public async Task<Dto.GetCaseResponse> Handle(Dto.GetCaseRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Get #{caseId}", request.CaseId);

        var result = resolveResult(await _caseService.GetCaseDetail(request.CaseId));

        return new Dto.GetCaseResponse
        {
            CaseId = result.CaseId,
            State = result.State,
            ContractNumber = result.ContractNumber,
            DateOfBirth = result.DateOfBirthNaturalPerson,
            FirstName = result.FirstNameNaturalPerson,
            LastName = result.Name
        };
    }

    private DomainServices.CaseService.Contracts.GetCaseDetailResponse resolveResult(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<DomainServices.CaseService.Contracts.GetCaseDetailResponse> r => r.Model,
           _ => throw new NotImplementedException()
       };

    private readonly ILogger<GetCaseHandler> _logger;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public GetCaseHandler(
        ILogger<GetCaseHandler> logger,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _logger = logger;
        _caseService = caseService;
    }
}
