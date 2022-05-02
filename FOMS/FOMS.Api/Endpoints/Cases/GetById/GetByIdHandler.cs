namespace FOMS.Api.Endpoints.Cases.GetById;

internal class GetByIdHandler
    : IRequestHandler<GetByIdRequest, Dto.CaseModel>
{
    public async Task<Dto.CaseModel> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetByIdHandler), request.CaseId);

        var result = ServiceCallResult.ResolveAndThrowIfError<DomainServices.CaseService.Contracts.Case>(await _caseService.GetCaseDetail(request.CaseId, cancellationToken));
        
        return await _converter.FromContract(result);
    }

    private readonly ILogger<GetByIdHandler> _logger;
    private readonly CasesModelConverter _converter;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public GetByIdHandler(
        CasesModelConverter converter,
        ILogger<GetByIdHandler> logger,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _converter = converter;
        _logger = logger;
        _caseService = caseService;
    }
}
