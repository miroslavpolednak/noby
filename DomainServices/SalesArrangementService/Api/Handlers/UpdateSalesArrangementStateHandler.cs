using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class UpdateSalesArrangementStateHandler
    : IRequestHandler<Dto.UpdateSalesArrangementStateMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementStateMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Update state of #{id} to {state}", request.SalesArrangementId, request.State);

        var arrangement = await _repository.GetSalesArrangement(request.SalesArrangementId);
        if (arrangement == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "SalesArrangementId does not exist.", 13000);
        //TODO nejaka validace na case?

        await _repository.UpdateSalesArrangementState(request.SalesArrangementId, request.State);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<UpdateSalesArrangementStateHandler> _logger;

    public UpdateSalesArrangementStateHandler(
        Repositories.NobyDbRepository repository,
        ILogger<UpdateSalesArrangementStateHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
