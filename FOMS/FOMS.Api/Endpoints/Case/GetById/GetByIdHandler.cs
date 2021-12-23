using CIS.Core.Results;

namespace FOMS.Api.Endpoints.Case.Handlers;

internal class GetByIdHandler
    : IRequestHandler<Dto.GetByIdRequest, Dto.GetByIdResponse>
{
    public async Task<Dto.GetByIdResponse> Handle(Dto.GetByIdRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Get #{caseId}", request.CaseId);

        var result = resolveResult(await _caseService.GetCaseDetail(request.CaseId));

        return new Dto.GetByIdResponse
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

    private readonly ILogger<GetByIdHandler> _logger;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public GetByIdHandler(
        ILogger<GetByIdHandler> logger,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _logger = logger;
        _caseService = caseService;
    }
}
