using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CaseService.Api.Handlers.CaseService;

internal class LinkOwnerToCaseHandler
    : IRequestHandler<Dto.CaseService.LinkOwnerToCaseMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.CaseService.LinkOwnerToCaseMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Link owner #{partyId} to case #{caseId}", request.PartyId, request.CaseId);

        var caseInstance = await _repository.GetCaseDetail(request.CaseId);
        if (caseInstance == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "CaseId does not exist.", 13000);
        //TODO nejaka validace na case?

        // update majitele v databazi
        await _repository.LinkOwnerToCase(request.CaseId, request.PartyId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<LinkOwnerToCaseHandler> _logger;

    public LinkOwnerToCaseHandler(
        Repositories.NobyDbRepository repository,
        ILogger<LinkOwnerToCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}