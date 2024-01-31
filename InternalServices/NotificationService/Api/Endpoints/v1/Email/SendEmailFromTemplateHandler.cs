using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.S3.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Email;
using DomainServices.CodebookService.Clients;
using MediatR;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Email;

public class SendEmailFromTemplateHandler : IRequestHandler<SendEmailFromTemplateRequest, SendEmailFromTemplateResponse>
{
    private readonly TimeProvider _dateTime;
    private readonly IMcsEmailProducer _mcsEmailProducer;
    private readonly IUserAdapterService _userAdapterService;
    private readonly INotificationRepository _repository;
    private readonly ICodebookServiceClient _codebookService;
    private readonly IS3AdapterService _s3Service;
    private readonly S3Buckets _buckets;
    private readonly HashSet<string> _mcsSenders;
    private readonly HashSet<string> _mpssSenders;
    private readonly ILogger<SendEmailFromTemplateHandler> _logger;

    public SendEmailFromTemplateHandler(
        TimeProvider dateTime,
        IMcsEmailProducer mcsEmailProducer,
        IUserAdapterService userAdapterService,
        INotificationRepository repository,
        ICodebookServiceClient codebookService,
        IS3AdapterService s3Service,
        IOptions<AppConfiguration> options,
        ILogger<SendEmailFromTemplateHandler> logger)
    {
        _dateTime = dateTime;
        _mcsEmailProducer = mcsEmailProducer;
        _userAdapterService = userAdapterService;
        _repository = repository;
        _codebookService = codebookService;
        _s3Service = s3Service;
        _buckets = options.Value.S3Buckets;
        _mcsSenders = options.Value.EmailSenders.Mcs.Select(e => e.ToLowerInvariant()).ToHashSet();
        _mpssSenders = options.Value.EmailSenders.Mpss.Select(e => e.ToLowerInvariant()).ToHashSet();
        _logger = logger;
    }
    
    public async Task<SendEmailFromTemplateResponse> Handle(SendEmailFromTemplateRequest request, CancellationToken cancellationToken)
    {
        var username = _userAdapterService
            .CheckSendEmailAccess()
            .GetUsername();
        
        var hashAlgorithms = await _codebookService.HashAlgorithms(cancellationToken);
        var hashAlgorithmCodes = string.Join(", ", hashAlgorithms.Select(s => s.Code));
        var hashAlgorithm = string.IsNullOrEmpty(request.DocumentHash?.HashAlgorithm)
            ? null
            : hashAlgorithms.FirstOrDefault(s => s.Code == request.DocumentHash.HashAlgorithm) ?? 
              throw new CisValidationException($"Invalid HashAlgorithm = '{request.DocumentHash.HashAlgorithm}'. Allowed HashAlgorithms: {hashAlgorithmCodes}");
        
        var attachmentKeyFilenames = new List<KeyValuePair<string, string>>();
        var domainName = request.From.Value.ToLowerInvariant().Split('@').Last();

        var result = _repository.NewEmailResult();
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CaseId = request.CaseId;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.DocumentHash = request.DocumentHash?.Hash;
        result.HashAlgorithm = request.DocumentHash?.HashAlgorithm;
        result.RequestTimestamp = _dateTime.GetLocalNow().DateTime;
        result.SenderType = _mcsSenders.Contains(domainName) ? Contracts.Statistics.Dto.SenderType.KB
            : _mpssSenders.Contains(domainName) ? Contracts.Statistics.Dto.SenderType.MP
            : throw new ArgumentException(domainName);

        result.CreatedBy = username;
        
        try
        {
            //await _repository.AddResult(result, cancellationToken);
            //await _repository.SaveChanges(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not create EmailResult.");
            throw new CisServiceServerErrorException(ErrorHandling.ErrorCodeMapper.CreateEmailResultFailed, nameof(SendEmailFromTemplateHandler), "SendEmailFromTemplate request failed due to internal server error.");
        }
        
        var consumerId = _userAdapterService.GetConsumerId();
        
        // todo:
        // var sendEmail = new SendEmail
        // {
            // id = result.Id.ToString(),
            // sender = request.From.Map(),
            // to = request.To.Map().ToList(),
            // bcc = request.Bcc.Map().ToList(),
            // cc = request.Cc.Map().ToList(),
            // replyTo = request.ReplyTo?.Map(),
            // subject = request.Subject,
            // content = request.Content.Map(),
            // attachments = attachments,
            // notificationConsumer = new NotificationConsumer
            // {
            //     consumerId = consumerId
            // }
        // };
        
        try
        {
            if (_mcsSenders.Contains(domainName))
            {
                // await _mcsEmailProducer.SendEmail(sendEmail, cancellationToken);
            }
            else if (_mpssSenders.Contains(domainName))
            {
                // await _mpssEmailProducer.SendEmail(sendEmail, cancellationToken);
            }
            else
            {
                throw new ArgumentException(domainName);
            }
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Could not produce message SendEmail to KAFKA.");
            _repository.DeleteResult(result);
            await _repository.SaveChanges(cancellationToken);
            throw new CisServiceServerErrorException(ErrorHandling.ErrorCodeMapper.ProduceSendEmailError, nameof(SendEmailFromTemplateHandler), "SendEmailFromTemplate request failed due to internal server error.");
        }
        
        return new SendEmailFromTemplateResponse { NotificationId = result.Id };
    }
}