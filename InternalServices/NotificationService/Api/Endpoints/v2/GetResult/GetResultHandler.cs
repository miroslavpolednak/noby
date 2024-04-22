using CIS.InternalServices.NotificationService.Contracts.v2;
using SharedComponents.DocumentDataStorage;
using CIS.InternalServices.NotificationService.Api.Database;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.GetResult;

internal sealed class GetResultHandler(
    IDocumentDataStorage _documentDataStorage, 
    Database.NotificationDbContext _dbContext)
        : IRequestHandler<GetResultRequest, ResultData>
{
    public async Task<ResultData> Handle(GetResultRequest request, CancellationToken cancellationToken)
    {
        var notificationId = Guid.Parse(request.NotificationId);

        var notificationInstance = await _dbContext
            .Notifications
            .AsNoTracking()
            .Where(t => t.Id == notificationId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new CIS.Core.Exceptions.CisNotFoundException(399, "Notification not found", request.NotificationId);

        var result = notificationInstance.MapToResultDataV2();

        // doplnit by type data notifikace
        await fillNotificationData(result, cancellationToken);

        return result;
    }

    private async Task fillNotificationData(ResultData result, CancellationToken cancellationToken)
    {
        switch (result.Channel)
        {
            case NotificationChannels.Sms:
                result.SmsData = (await _documentDataStorage
                    .FirstOrDefaultByEntityId<Database.DocumentDataEntities.SmsData, string>(result.NotificationId, cancellationToken))
                    .MapToSmsResult();
                break;
        }
    }
}
