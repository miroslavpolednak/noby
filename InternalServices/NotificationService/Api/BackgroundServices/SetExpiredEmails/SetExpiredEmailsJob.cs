
using CIS.Core;
using CIS.InternalServices.NotificationService.Api.Database;
using CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SetExpiredEmails;

public sealed class SetExpiredEmailsJob
    : Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    private readonly NotificationDbContext _dbContext;
    private readonly ILogger<SetExpiredEmailsJob> _logger;
    private readonly IDateTime _dateTime;
    private readonly SetExpiredEmailsJobConfiguration _configuration;

    public SetExpiredEmailsJob(
        NotificationDbContext dbContext,
        ILogger<SetExpiredEmailsJob> logger,
        IDateTime dateTime,
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
        var emails = await _dbContext.EmailResults
            .Where(t => t.State == NotificationState.InProgress && t.SenderType == LegacyContracts.Statistics.Dto.SenderType.MP)
            .Where(t => t.RequestTimestamp < _dateTime.Now.AddMinutes(_configuration.EmailSlaInMinutes * -1) && !t.Resend)
            .ToListAsync(cancellationToken);

        if (emails.Count != 0)
        {
            foreach (var email in emails)
            {
                email.State = NotificationState.Unsent;
                email.ResultTimestamp = _dateTime.Now;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Number of emails marked as Unsent from {nameof(SetExpiredEmailsJob)}: {emails.Count}, EmailSlaInMinutes: {_configuration.EmailSlaInMinutes}");
        }
    }
}
