﻿using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Legacy;
using CIS.InternalServices.NotificationService.Api.Legacy.AuditLog.Abstraction;
using CIS.InternalServices.NotificationService.Api.Legacy.Helpers;
using CIS.InternalServices.NotificationService.Api.Legacy.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.LegacyContracts.Sms;
using DomainServices.CodebookService.Clients;
using KafkaFlow;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Sms;

internal class SendSmsHandler : IRequestHandler<SendSmsRequest, SendSmsResponse>
{
    private readonly TimeProvider _dateTime;
    private readonly IMessageProducer<cz.kb.osbs.mcs.sender.sendapi.v4.sms.SendSMS> _mcsSmsProducer;
    private readonly IUserAdapterService _userAdapterService;
    private readonly INotificationRepository _repository;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISmsAuditLogger _smsAuditLogger;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        TimeProvider dateTime,
        IMessageProducer<cz.kb.osbs.mcs.sender.sendapi.v4.sms.SendSMS> mcsSmsProducer,
        IUserAdapterService userAdapterService,
        INotificationRepository repository,
        ICodebookServiceClient codebookService,
        ISmsAuditLogger smsAuditLogger,
        ILogger<SendSmsHandler> logger)
    {
        _dateTime = dateTime;
        _mcsSmsProducer = mcsSmsProducer;
        _userAdapterService = userAdapterService;
        _repository = repository;
        _codebookService = codebookService;
        _smsAuditLogger = smsAuditLogger;
        _logger = logger;
    }
    
    public async Task<SendSmsResponse> Handle(SendSmsRequest request, CancellationToken cancellationToken)
    {
        var username = _userAdapterService
            .CheckSendSmsAccess()
            .GetUsername();
        
        var smsTypes = await _codebookService.SmsNotificationTypes(cancellationToken);
        var smsTypeCodes = string.Join(", ", smsTypes.Select(s => s.Code));
        var smsType = smsTypes.FirstOrDefault(s => s.Code == request.Type) ??
        throw new CisValidationException($"Invalid Type = '{request.Type}'. Allowed Types: {smsTypeCodes}");

        if (!string.IsNullOrEmpty(request.DocumentHash?.HashAlgorithm) && !HashAlgorithms.Algorithms.Contains(request.DocumentHash.HashAlgorithm))
        {
            throw new CisValidationException($"Invalid HashAlgorithm = '{request.DocumentHash?.HashAlgorithm}'.");
        }

        // zmenit text podle spec GSM 03.38
        request.Text = request.Text.ToGSMString();

        var result = _repository.NewSmsResult();
        var phone = request.PhoneNumber.ParsePhone()!;
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CaseId = request.CaseId;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.DocumentHash = request.DocumentHash?.Hash;
        result.HashAlgorithm = request.DocumentHash?.HashAlgorithm;
        result.RequestTimestamp = _dateTime.GetLocalNow().DateTime;

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
            throw new CisServiceServerErrorException(ErrorCodeMapper.CreateSmsResultFailed, nameof(SendSmsHandler), "SendSms request failed due to internal server error.");
        }
        
        var consumerId = _userAdapterService.GetConsumerId();
        
        var sendSms = new McsSendApi.v4.sms.SendSMS
        {
            id = Configuration.KafkaTopics.McsIdPrefix + result.Id.ToString(),
            phone = phone.Map(),
            type = smsType.McsCode,
            text = request.Text,
            processingPriority = request.ProcessingPriority,
            notificationConsumer = McsSmsMappers.MapToMcs(consumerId)
        };
        
        try
        {
            await _mcsSmsProducer.ProduceAsync(sendSms.id, sendSms);
            _smsAuditLogger.LogKafkaProduced(smsType, result.Id, username,
                request.Identifier?.Identity,
                request.Identifier?.IdentityScheme,
                request.CaseId,
                request.CustomId,
                request.DocumentId,
                request.DocumentHash?.Hash,
                request.DocumentHash?.HashAlgorithm);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not produce message SendSMS to KAFKA.");
            _smsAuditLogger.LogKafkaProduceError(smsType, username,
                request.Identifier?.Identity,
                request.Identifier?.IdentityScheme,
                request.CaseId,
                request.CustomId,
                request.DocumentId,
                request.DocumentHash?.Hash,
                request.DocumentHash?.HashAlgorithm);
            _repository.DeleteResult(result);
            await _repository.SaveChanges(cancellationToken);
            throw new CisServiceServerErrorException(ErrorCodeMapper.ProduceSendSmsError, nameof(SendSmsHandler), "SendSms request failed due to internal server error.");
        }

        return new SendSmsResponse { NotificationId = result.Id };
    }
}