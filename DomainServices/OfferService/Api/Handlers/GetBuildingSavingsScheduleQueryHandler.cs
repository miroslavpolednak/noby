using DomainServices.OfferService.Contracts;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using System.Text.Json;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetBuildingSavingsScheduleQueryHandler : IRequestHandler<Dto.GetBuildingSavingsScheduleRequest, GetBuildingSavingsScheduleResponse>
{
    public async Task<GetBuildingSavingsScheduleResponse> Handle(Dto.GetBuildingSavingsScheduleRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get {schedule} schedule for offer ID #{id}", request.ScheduleType, request.OfferInstanceId);

        var scheduleData = await _repository.GetScheduleItems(request.OfferInstanceId);
        if (string.IsNullOrEmpty(scheduleData))
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Offer instance #{request.OfferInstanceId} not found", 10007);
        var scheduleItems = JsonSerializer.Deserialize<List<ScheduleItem>>(scheduleData);

        var model = new GetBuildingSavingsScheduleResponse
        {
            ScheduleType = request.ScheduleType,
            OfferInstanceId = request.OfferInstanceId
        };

        // vybrat jen ty, pro ktere plati vybrany scheduled
        model.ScheduleItems.AddRange(scheduleItems?.Where(t => t.Type == request.ScheduleType));

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
