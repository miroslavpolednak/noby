using CIS.InternalServices.NotificationService.Api.Database.Entities;
using CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Result;
using DomainServices.CodebookService.Clients;
using KafkaFlow;
using SharedAudit;
using SharedComponents.DocumentDataStorage;

namespace CIS.InternalServices.NotificationService.Api.Messaging.NotificationReport;

internal sealed class NotificationReportHandler
    : IMessageHandler<cz.kb.osbs.mcs.notificationreport.eventapi.v3.report.NotificationReport>
{
    public async Task Handle(IMessageContext context, cz.kb.osbs.mcs.notificationreport.eventapi.v3.report.NotificationReport message)
    {
        if (!message.id.StartsWith(Configuration.KafkaTopics.McsIdPrefix, StringComparison.InvariantCultureIgnoreCase) || !Guid.TryParse(message.id.AsSpan(Configuration.KafkaTopics.McsIdPrefix.Length), out var notificationId))
        {
            _logger.KafkaNotificationResultIdEmpty(message.id);
            return;
        }

        var notificationInstance = await _dbContext
            .Notifications
            .Where(t => t.Id == notificationId)
            .FirstOrDefaultAsync();

        /*TODO az se odstrani stara cesta!
        if (notificationInstance is null)
        {
            _logger.KafkaNotificationResultNotificationNotFound(notificationId);
            return;
        }*/

        // stara cesta?
        #region legacy code
        if (notificationInstance is null)
        {
            await _mediator.Send(new ConsumeResultRequest { NotificationReport = message, Id = notificationId });
        }
        #endregion legacy code
        else
        {
            // audit log
            if (notificationInstance.Channel == Contracts.v2.NotificationChannels.Sms)
            {
                await auditLogSmsResult(message, notificationId.ToString());
            } else
            {
                auditLogEmailResult(message, notificationId.ToString());
            }

            if (_statusesMap.TryGetValue(message.state, out Contracts.v2.NotificationStates state))
            {
                notificationInstance.ResultTime = _timeProvider.GetLocalNow().DateTime;
                notificationInstance.State = state;
                notificationInstance.Errors = message.notificationErrors?.Select(t => new Database.Entities.NotificationError
                {
                    Code = t.code,
                    Message = t.message
                }).ToList();

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.KafkaNotificationResultUnknownState(message.state, notificationId);
            }
        }
    }

    /// <summary>
    /// Auditni log
    /// </summary>
    private async Task auditLogSmsResult(cz.kb.osbs.mcs.notificationreport.eventapi.v3.report.NotificationReport message, string notificationId)
    {
        var smsData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.SmsData, string>(notificationId);
        var smsType = (await _codebookService.SmsNotificationTypes()).First(s => s.Code == smsData?.Data?.SmsType);

        var bodyBefore = new Dictionary<string, string>
            {
                { "smsType", smsType.Code },
                { "notificationId", notificationId },
                { "state", message.state }
            };

        if (message.notificationErrors is not null && message.notificationErrors.Count > 0)
        {
            bodyBefore.Add("errors", System.Text.Json.JsonSerializer.Serialize(message.notificationErrors));
        }

        _auditLogger.Log(
            AuditEventTypes.Noby014,
            "Received notification report for sms",
            bodyBefore: bodyBefore
        );
    }

    private void auditLogEmailResult(cz.kb.osbs.mcs.notificationreport.eventapi.v3.report.NotificationReport message, string notificationId)
    {
        var bodyBefore = new Dictionary<string, string>
            {
                { "notificationId", notificationId },
                { "state", message.state }
            };

        if (message.notificationErrors is not null && message.notificationErrors.Count > 0)
        {
            bodyBefore.Add("errors", System.Text.Json.JsonSerializer.Serialize(message.notificationErrors));
        }

        _auditLogger.Log(
            AuditEventTypes.Noby014,
            "Received notification report for email",
            bodyBefore: bodyBefore
        );
    }

    private static readonly Dictionary<string, Contracts.v2.NotificationStates> _statusesMap = new()
    {
        { "INVALID", Contracts.v2.NotificationStates.Invalid },
        { "UNSENT", Contracts.v2.NotificationStates.Unsent },
        { "DELIVERED", Contracts.v2.NotificationStates.Delivered },
        { "SENT", Contracts.v2.NotificationStates.Sent }
    };

    private readonly IAuditLogger _auditLogger;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ICodebookServiceClient _codebookService;
    private readonly IMediator _mediator;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<NotificationReportHandler> _logger;
    private readonly Database.NotificationDbContext _dbContext;

    public NotificationReportHandler(IMediator mediator, ILogger<NotificationReportHandler> logger, Database.NotificationDbContext dbContext, TimeProvider timeProvider, ICodebookServiceClient codebookService, IDocumentDataStorage documentDataStorage, IAuditLogger auditLogger)
    {
        _mediator = mediator;
        _logger = logger;
        _dbContext = dbContext;
        _timeProvider = timeProvider;
        _codebookService = codebookService;
        _documentDataStorage = documentDataStorage;
        _auditLogger = auditLogger;
    }
}
