using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Helpers;
using CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using DomainServices.CodebookService.Clients;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Sms;

public class SendSmsHandler : IRequestHandler<SendSmsRequest, SendSmsResponse>
{
    private readonly IDateTime _dateTime;
    private readonly IMcsSmsProducer _mcsSmsProducer;
    private readonly IUserAdapterService _userAdapterService;
    private readonly INotificationRepository _repository;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISmsAuditLogger _auditLogger;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        IDateTime dateTime,
        IMcsSmsProducer mcsSmsProducer,
        IUserAdapterService userAdapterService,
        INotificationRepository repository,
        ICodebookServiceClient codebookService,
        ISmsAuditLogger auditLogger,
        ILogger<SendSmsHandler> logger)
    {
        _dateTime = dateTime;
        _mcsSmsProducer = mcsSmsProducer;
        _userAdapterService = userAdapterService;
        _repository = repository;
        _codebookService = codebookService;
        _auditLogger = auditLogger;
        _logger = logger;
    }
    
    public async Task<SendSmsResponse> Handle(SendSmsRequest request, CancellationToken cancellationToken)
    {
        var username = _userAdapterService
            .CheckSendSmsAccess()
            .GetUsername();
        
        var smsTypes = await _codebookService.SmsNotificationTypes(cancellationToken);
        var smsType = smsTypes.FirstOrDefault(s => s.Code == request.Type) ??
        throw new CisValidationException($"Invalid Type = '{request.Type}'. Allowed Types: {string.Join(',', smsTypes.Select(s => s.Code))}");
        
        var result = _repository.NewSmsResult();
        var phone = request.PhoneNumber.ParsePhone();
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.RequestTimestamp = _dateTime.Now;

        result.Type = request.Type;
        result.CountryCode = phone.CountryCode;
        result.PhoneNumber = phone.NationalNumber;

        result.CreatedBy = username;
        
        try
        {
            await _repository.AddResult(result, cancellationToken);
            await _repository.SaveChanges(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not create SmsResult.");
            throw new CisServiceServerErrorException(ErrorHandling.ErrorCodeMapper.CreateSmsResultFailed, nameof(SendSmsHandler), "SendSms request failed due to internal server error.");
        }
        
        var consumerId = _userAdapterService.GetConsumerId();
        
        var sendSms = new McsSendApi.v4.sms.SendSMS
        {
            id = result.Id.ToString(),
            phone = phone.Map(),
            type = smsType.McsCode,
            text = request.Text,
            processingPriority = request.ProcessingPriority,
            notificationConsumer = McsSmsMappers.MapToMcs(consumerId)
        };
        
        try
        {
            _auditLogger.LogKafkaProducing(smsType, username);
            await _mcsSmsProducer.SendSms(sendSms, cancellationToken);
            _auditLogger.LogKafkaProduced(smsType, result.Id, username);
        }
        catch (Exception e)
        {
            _auditLogger.LogKafkaError(smsType, username);
            _logger.LogError(e, "Could not produce message SendSMS to KAFKA.");
            _repository.DeleteResult(result);
            await _repository.SaveChanges(cancellationToken);
            throw new CisServiceServerErrorException(ErrorHandling.ErrorCodeMapper.ProduceSendSmsError, nameof(SendSmsHandler), "SendSms request failed due to internal server error.");
        }

        return new SendSmsResponse { NotificationId = result.Id };
    }
}