namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateCaseCustomerHandler
    : IRequestHandler<Dto.UpdateCaseCustomerMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseCustomerMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Update case customer for #{caseId}", request.Request.CaseId);

        var caseInstance = await _repository.GetCaseDetail(request.Request.CaseId);
        //TODO nejaka validace na case?

        CIS.Core.IdentitySchemes? scheme = request.Request.Customer is null ? null : (CIS.Core.IdentitySchemes)Convert.ToInt32(request.Request.Customer?.IdentityScheme);
        await _repository.UpdateCaseCustomer(request.Request.CaseId, request.Request.Customer?.IdentityId, scheme, request.Request.FirstNameNaturalPerson, request.Request.Name, request.Request.DateOfBirthNaturalPerson);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;

    public UpdateCaseCustomerHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
