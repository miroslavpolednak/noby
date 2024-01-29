using CIS.InternalServices.NotificationService.Contracts.v2;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendSms;

internal sealed class SendSmsRequestValidator
    : AbstractValidator<SendSmsRequest>
{
    public SendSmsRequestValidator()
    {
        When(t => !string.IsNullOrEmpty(t.Sms.DocumentHash?.Hash), () =>
        {
            RuleFor(t => t.Sms.DocumentHash.HashAlgorithm)
                .Must(x => x != DocumentHash.Types.HashAlgorithms.Unknown)
                .WithErrorCode(ErrorCodeMapper.DocumentHashInvalid);
        });

        RuleFor(request => request.Sms.PhoneNumber)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.SmsPhoneNumberRequired)
            .ChildRules(t => { })
            .WithErrorCode(ErrorCodeMapper.SmsPhoneNumberInvalid);

        RuleFor(request => request.Sms.ProcessingPriority)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SmsProcessPriorityInvalid);

        RuleFor(request => request.Sms.Type)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.SmsTypeInvalid);

        RuleFor(request => request.Text)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.SmsTextRequired)
            .MaximumLength(480)
            .WithErrorCode(ErrorCodeMapper.SmsTextLengthLimitExceeded);

        When(request => request.Sms.Identifier is not null, () =>
        {
            RuleFor(request => request.Sms.Identifier.IdentityScheme)
                .Must(t => t != SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.Unknown)
                .WithErrorCode(ErrorCodeMapper.IdentifierInvalid);
        });

        When(request => request.Sms.CaseId.HasValue, () =>
        {
            RuleFor(request => request.Sms.CaseId!.Value)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodeMapper.CaseIdInvalid);
        });

        When(request => request.Sms.CustomId is not null, () =>
        {
            RuleFor(request => request.Sms.CustomId!)
                .Matches("^([A-Za-z0-9-_]{0,450})$")
                .WithErrorCode(ErrorCodeMapper.CustomIdInvalid);
        });
    }
}
