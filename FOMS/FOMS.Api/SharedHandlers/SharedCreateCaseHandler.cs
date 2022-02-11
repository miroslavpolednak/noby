namespace FOMS.Api.SharedHandlers;

internal sealed class SharedCreateCaseHandler
    : IRequestHandler<Requests.SharedCreateCaseRequest, long>
{
    public async Task<long> Handle(Requests.SharedCreateCaseRequest request, CancellationToken cancellationToken)
    {
        _logger.SharedCreateCaseStarted(request);
        
        var model = new DomainServices.CaseService.Contracts.CreateCaseRequest()
        {
            CaseOwnerUserId = _userAccessor.User.Id,
            Customer = new DomainServices.CaseService.Contracts.CustomerData
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName,
                Name = request.LastName,
                Identity = request.Customer is null ? null : new CIS.Infrastructure.gRPC.CisTypes.Identity(request.Customer)
            },
            Data = new DomainServices.CaseService.Contracts.CaseData
            {
                ProductTypeId = request.ProductTypeId,
                TargetAmount = request.TargetAmount
            }
        };

        long caseId = resolveResult(await _caseService.CreateCase(model, cancellationToken));
        
        _logger.EntityCreated("Case", caseId);

        return caseId;
    }

    private long resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r => r.Model,
            ErrorServiceCallResult e2 => throw new CisValidationException(e2.Errors),
            _ => throw new NotImplementedException()
        };

    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly ILogger<SharedCreateCaseHandler> _logger;

    public SharedCreateCaseHandler(
        ILogger<SharedCreateCaseHandler> logger,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _logger = logger;
        _userAccessor = userAccessor;
        _caseService = caseService;
    }
}
