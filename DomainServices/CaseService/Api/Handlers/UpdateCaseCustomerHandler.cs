using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CaseService.Api.Handlers;


internal class UpdateCaseCustomerHandler
    : IRequestHandler<Dto.UpdateCaseCustomerMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseCustomerMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Update case customer for #{caseId}", request.Request.CaseId);

        var caseInstance = await _repository.GetCaseDetail(request.Request.CaseId);
        if (caseInstance == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "CaseId does not exist.", 13000);
        //TODO nejaka validace na case?

        await _repository.UpdateCaseCustomer(request.Request.CaseId, request.Request.Customer.IdentityId, request.Request.FirstNameNaturalPerson, request.Request.Name, request.Request.DateOfBirthNaturalPerson);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;

    public UpdateCaseCustomerHandler(
        Repositories.NobyDbRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
