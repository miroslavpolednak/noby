using CIS.InternalServices.NotificationService.Api.Database;
using CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SetExpiredEmails;

internal sealed class SetExpiredEmailsJob
    : Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    private readonly NotificationDbContext _dbContext;
    private readonly ILogger<SetExpiredEmailsJob> _logger;
    private readonly TimeProvider _dateTime;
    private readonly SetExpiredEmailsJobConfiguration _configuration;

    public SetExpiredEmailsJob(
        NotificationDbContext dbContext,
        ILogger<SetExpiredEmailsJob> logger,
        TimeProvider dateTime,
        SetExpiredEmailsJobConfiguration configuration)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dateTime = dateTime;
        _configuration = configuration;
    }

    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        // nastavit unsent emailum ktere uz nejsou k odeslani
        var emails_v1 = await _dbContext.EmailResults
            .Where(t => t.State == NotificationState.InProgress && t.SenderType == LegacyContracts.Statistics.Dto.SenderType.MP)
            .Where(t => t.RequestTimestamp < _dateTime.GetLocalNow().AddMinutes(_configuration.EmailSlaInMinutes * -1) && !t.Resend)
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
            .Where(t => t.CreatedTime < _dateTime.GetLocalNow().AddMinutes(_configuration.EmailSlaInMinutes * -1) && !t.Resend)
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
            _logger.SetExpiredEmailsJob(count, _configuration.EmailSlaInMinutes);
    }
}
