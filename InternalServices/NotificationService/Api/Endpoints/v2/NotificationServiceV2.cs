using CIS.InternalServices.NotificationService.Contracts.v2;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2;

[Authorize]
internal sealed class NotificationServiceV2
    : Contracts.v2.NotificationService.NotificationServiceBase
{
    [Authorize(Roles = UserRoles.SendSms)]
    public override async Task<NotificationIdResponse> SendSms(SendSmsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;
    public NotificationServiceV2(IMediator mediator)
        => _mediator = mediator;
}
