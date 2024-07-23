using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Contracts.v2;
using Google.Protobuf.WellKnownTypes;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.Resend;

internal sealed class ResendHandler(
    Database.NotificationDbContext _dbContext)
    : IRequestHandler<ResendRequest, Empty>
{
    public async Task<Empty> Handle(ResendRequest request, CancellationToken cancellationToken)
    {
        var notificationId = Guid.Parse(request.NotificationId);

        // resend jen na MPSS emaily
        var entity = await _dbContext.Notifications
            .Where(t => t.Id == notificationId && t.Channel == NotificationChannels.Email && t.Mandant == Mandants.Mp)
            .Where(t => t.State == NotificationStates.Invalid || t.State == NotificationStates.Unsent || t.State == NotificationStates.Error)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new CisNotFoundException(ErrorCodeMapper.ResultNotFound, $"Notification '{request.NotificationId}' not found.");

        entity.State = NotificationStates.InProgress;
        entity.Resend = true;

        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"Delete From SentNotification Where Id = {entity.Id}", cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}
