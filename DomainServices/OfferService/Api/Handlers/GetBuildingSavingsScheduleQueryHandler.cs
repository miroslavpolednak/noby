using DomainServices.OfferService.Contracts;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetBuildingSavingsScheduleQueryHandler : IRequestHandler<Dto.GetBuildingSavingsScheduleRequest, GetBuildingSavingsScheduleResponse>
{
    public async Task<GetBuildingSavingsScheduleResponse> Handle(Dto.GetBuildingSavingsScheduleRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get {schedule} schedule for offer ID #{id}", request.ScheduleType, request.OfferInstanceId);

        var model = await _repository.GetScheduleItems(request.OfferInstanceId);

        if (model == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Offer instance #{request.OfferInstanceId} not found", 10007);

        if (model.ScheduleItems == null ||
            (model.SimulationType != SimulationTypes.BuildingSavingsWithLoan && request.ScheduleType == ScheduleItemTypes.PaymentSchedule))
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Schedule Items not found for OfferInstanceId #{request.OfferInstanceId}", 10010);

        // vybrat jen ty, pro ktere plati vybrany scheduled
        model.ScheduleItems.AddRange(model.ScheduleItems.Where(t => t.Type == request.ScheduleType).ToList());

        if (!model.ScheduleItems.Any())
            throw new ArgumentNullException("ScheduleItems");

        return model;
    }

    private readonly Repositories.SimulateBuildingSavingsRepository _repository;
    private readonly ILogger<SimulateBuildingSavingsCommandHandler> _logger;

    public GetBuildingSavingsScheduleQueryHandler(
        Repositories.SimulateBuildingSavingsRepository repository,
        ILogger<SimulateBuildingSavingsCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
