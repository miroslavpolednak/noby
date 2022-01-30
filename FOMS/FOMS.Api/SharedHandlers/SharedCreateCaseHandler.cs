using CIS.Core.Results;

namespace FOMS.Api.SharedHandlers;

internal sealed class SharedCreateCaseHandler
    : IRequestHandler<Requests.SharedCreateCaseRequest, long>
{
    public async Task<long> Handle(Requests.SharedCreateCaseRequest request, CancellationToken cancellationToken)
    {
        var model = new DomainServices.CaseService.Contracts.CreateCaseRequest()
        {
            CaseOwnerUserId = _userAccessor.User.Id,
            Customer = new DomainServices.CaseService.Contracts.CustomerData
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName,
                Name = request.LastName,
            },
            Data = new DomainServices.CaseService.Contracts.CaseData
            {
                ProductInstanceTypeId = request.ProductInstanceTypeId,
                TargetAmount = request.TargetAmount
            }
        };
        if (request.Customer is not null)
            model.Customer.Identity = new CIS.Infrastructure.gRPC.CisTypes.Identity(request.Customer);

        _logger.LogDebug("Attempt to create case {data}", model);
        long caseId = resolveResult(await _caseService.CreateCase(model, cancellationToken));
        _logger.LogDebug("Case #{caseId} created", caseId);

        return caseId;
    }

    private long resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r => r.Model,
            ErrorServiceCallResult e2 => throw new CIS.Core.Exceptions.CisValidationException(e2.Errors),
            _ => throw new NotImplementedException()
        };

    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private ILogger<SharedCreateCaseHandler> _logger;

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
