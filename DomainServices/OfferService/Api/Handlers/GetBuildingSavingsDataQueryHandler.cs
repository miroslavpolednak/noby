using DomainServices.OfferService.Contracts;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetBuildingSavingsDataQueryHandler 
    : IRequestHandler<Dto.GetBuildingSavingsDataMediatrRequest, GetBuildingSavingsDataResponse>
{
    public async Task<GetBuildingSavingsDataResponse> Handle(Dto.GetBuildingSavingsDataMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get offer instance ID #{id}", request.OfferInstanceId);

        var model = await _repository.Get(request.OfferInstanceId);

        if (model == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Offer instance #{request.OfferInstanceId} not found", 10007);

        return model;
    }

    private readonly Repositories.SimulateBuildingSavingsRepository _repository;
    private readonly ILogger<SimulateBuildingSavingsCommandHandler> _logger;

    public GetBuildingSavingsDataQueryHandler(
        Repositories.SimulateBuildingSavingsRepository repository,
        ILogger<SimulateBuildingSavingsCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
