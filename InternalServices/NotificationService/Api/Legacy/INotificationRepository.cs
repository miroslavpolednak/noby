using CIS.InternalServices.NotificationService.Api.Database.Entities;

namespace CIS.InternalServices.NotificationService.Api.Legacy;

public interface INotificationRepository
{
    EmailResult NewEmailResult();

    SmsResult NewSmsResult();

    Task AddResult(Result result, CancellationToken token = default);

    Task<Result> GetResult(Guid id, CancellationToken token = default);

    Task<IEnumerable<Result>> SearchResultsBy(string? identity, string? identityScheme, long? caseId, string? customId, string? documentId);

    void DeleteResult(Result result);

    Task<int> SaveChanges(CancellationToken token = default);
}