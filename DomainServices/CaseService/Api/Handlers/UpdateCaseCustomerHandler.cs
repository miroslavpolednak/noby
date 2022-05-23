namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateCaseCustomerHandler
    : IRequestHandler<Dto.UpdateCaseCustomerMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseCustomerMediatrRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        await _repository.EnsureExistingCase(request.Request.CaseId, cancellation);
        //TODO zkontrolovat existenci klienta?

        // ulozit do DB
        await _repository.UpdateCaseCustomer(request.Request.CaseId, request.Request.Customer, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.CaseServiceRepository _repository;
    
    public UpdateCaseCustomerHandler(
        Repositories.CaseServiceRepository repository)
    {
        _repository = repository;
    }
}
