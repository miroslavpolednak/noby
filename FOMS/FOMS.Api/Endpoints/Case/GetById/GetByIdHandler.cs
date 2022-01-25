using CIS.Core.Results;

namespace FOMS.Api.Endpoints.Case.Handlers;

internal class GetByIdHandler
    : IRequestHandler<Dto.GetByIdRequest, Dto.CaseModel>
{
    public async Task<Dto.CaseModel> Handle(Dto.GetByIdRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Get #{caseId}", request.CaseId);

        var result = resolveResult(await _caseService.GetCaseDetail(request.CaseId, cancellationToken));

        return await _converter.FromContract(result);
    }

    private DomainServices.CaseService.Contracts.Case resolveResult(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<DomainServices.CaseService.Contracts.Case> r => r.Model,
           _ => throw new NotImplementedException()
       };

    private readonly ILogger<GetByIdHandler> _logger;
    private readonly CaseModelConverter _converter;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public GetByIdHandler(
        CaseModelConverter converter,
        ILogger<GetByIdHandler> logger,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _converter = converter;
        _logger = logger;
        _caseService = caseService;
    }
}
