using CIS.InternalServices.NotificationService.Api.Database;
using CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.Cleaning;

internal sealed class CleaningJob(
    NotificationDbContext _dbContext,
    ILogger<CleaningJob> _logger,
    TimeProvider _dateTime,
    CleaningJobConfiguration _configuration)
    : Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        await SetExpiredEmails(cancellationToken);

        await CleanInProgress(cancellationToken);
    }

    internal async Task CleanInProgress(CancellationToken cancellationToken)
    {
        var date = _dateTime.GetLocalNow().AddMinutes(_configuration.CleanInProgressInMinutes * -1);

        // nastavit error notifikacim ktere jsou zaseknute v InProgress
        var emails_v1 = await _dbContext.Results
            .Where(t => t.State == NotificationState.InProgress)
            .Where(t => t.RequestTimestamp < date)
            .ToListAsync(cancellationToken);

        if (emails_v1.Count != 0)
        {
            foreach (var email in emails_v1)
            {
                email.State = NotificationState.Error;
                email.ResultTimestamp = _dateTime.GetLocalNow().DateTime;
                email.ErrorSet = new HashSet<ResultError>() { new ResultError { Code = "IN_PROGRESS_ISSUE", Message = "The notification remained in state IN PROGRESS for longer time than configured threshold." } };
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // nastavit error notifikacim ktere jsou zaseknute v InProgress
        var emails_v2 = await _dbContext.Notifications
            .Where(t => t.State == Contracts.v2.NotificationStates.InProgress)
            .Where(t => t.CreatedTime < date)
            .ToListAsync(cancellationToken);

        if (emails_v2.Count != 0)
        {
            foreach (var email in emails_v2)
            {
                email.State = Contracts.v2.NotificationStates.Unsent;
                email.ResultTime = _dateTime.GetLocalNow().DateTime;
                email.Errors = new List<Database.Entities.NotificationError> { new Database.Entities.NotificationError { Code = "IN_PROGRESS_ISSUE", Message = "The notification remained in state IN PROGRESS for longer time than configured threshold." } };
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        int count = emails_v1.Count + emails_v2.Count;

        if (count > 0)
            _logger.CleanInProgress(count, _configuration.CleanInProgressInMinutes);
    }

    internal async Task SetExpiredEmails(CancellationToken cancellationToken)
    {
        var date = _dateTime.GetLocalNow().AddMinutes(_configuration.EmailSlaInMinutes * -1);

        // nastavit unsent emailum ktere uz nejsou k odeslani
        var emails_v1 = await _dbContext.EmailResults
            .Where(t => t.State == NotificationState.InProgress && t.SenderType == LegacyContracts.Statistics.Dto.SenderType.MP)
            .Where(t => t.RequestTimestamp < date && !t.Resend)
            .ToListAsync(cancellationToken);

        if (emails_v1.Count != 0)
        {
            foreach (var email in emails_v1)
            {
                email.State = NotificationState.Unsent;
                email.ResultTimestamp = _dateTime.GetLocalNow().DateTime;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // nastavit unsent emailum ktere uz nejsou k odeslani
        var emails_v2 = await _dbContext.Notifications
            .Where(t => t.Channel == Contracts.v2.NotificationChannels.Email && t.State == Contracts.v2.NotificationStates.InProgress && t.Mandant == Mandants.Mp)
            .Where(t => t.CreatedTime < date && !t.Resend)
            .ToListAsync(cancellationToken);

        if (emails_v2.Count != 0)
        {
            foreach (var email in emails_v2)
            {
                email.State = Contracts.v2.NotificationStates.Unsent;
                email.ResultTime = _dateTime.GetLocalNow().DateTime;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        int count = emails_v1.Count + emails_v2.Count;

        if (count > 0)
            _logger.SetExpiredEmails(count, _configuration.EmailSlaInMinutes);
    }
}
