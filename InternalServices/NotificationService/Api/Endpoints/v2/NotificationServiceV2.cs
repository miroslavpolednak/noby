using CIS.InternalServices.NotificationService.Contracts.v2;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2;

internal sealed class NotificationService(IMediator _mediator)
        : Contracts.v2.NotificationService.NotificationServiceBase
{
    [Authorize(Roles = UserRoles.SendSms)]
    public override async Task<NotificationIdResponse> SendSms(SendSmsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    [Authorize(Roles = UserRoles.SendEmail)]
    public override async Task<NotificationIdResponse> SendEmail(SendEmailRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    [Authorize(Roles = UserRoles.ReadResult)]
    public override async Task<ResultData> GetResult(GetResultRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    [EnableRateLimiting("fixed")]
    [Authorize(Roles = UserRoles.ReadResult)]
    public override async Task<SearchResultsResponse> SearchResults(SearchResultsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    [Authorize(Roles = UserRoles.ReceiveStatistics)]
    public override async Task<GetStatisticsResponse> GetStatistics(GetStatisticsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    [Authorize(Roles = UserRoles.ReceiveStatistics)]
    public override async Task<GetDetailedStatisticsResponse> GetDetailedStatistics(GetDetailedStatisticsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    [Authorize(Roles = UserRoles.ResendNotifications)]
    public override async Task<Empty> Resend(ResendRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);
}
