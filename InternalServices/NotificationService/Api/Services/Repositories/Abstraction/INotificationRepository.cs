using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;

public interface INotificationRepository
{
    Entities.EmailResult NewEmailResult();

    Entities.SmsResult NewSmsResult();

    Task AddResult(Result result, CancellationToken token = default);

    Task<Result> GetResult(Guid id, CancellationToken token = default);

    Task<IEnumerable<Result>> SearchResultsBy(string? identity, string? identityScheme, string? customId, string? documentId);

    void DeleteResult(Result result);

    Task<int> SaveChanges(CancellationToken token = default);
}