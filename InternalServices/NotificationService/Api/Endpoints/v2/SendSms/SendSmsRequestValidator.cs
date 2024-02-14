using CIS.InternalServices.NotificationService.Contracts.v2;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendSms;

internal sealed class SendSmsRequestValidator
    : AbstractValidator<SendSmsRequest>
{
    public SendSmsRequestValidator()
    {
        When(t => !string.IsNullOrEmpty(t.DocumentHash?.Hash), () =>
        {
            RuleFor(t => t.DocumentHash.HashAlgorithm)
                .Must(x => x != DocumentHash.Types.HashAlgorithms.Unknown)
                .WithErrorCode(ErrorCodeMapper.DocumentHashInvalid);
        });

        RuleFor(request => request.PhoneNumber)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.SmsPhoneNumberRequired)
            .ChildRules(t => { })
            .WithErrorCode(ErrorCodeMapper.SmsPhoneNumberInvalid);

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SmsProcessPriorityInvalid);

        RuleFor(request => request.Type)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.SmsTypeInvalid);

        RuleFor(request => request.Text)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.SmsTextRequired)
            .MaximumLength(480)
            .WithErrorCode(ErrorCodeMapper.SmsTextLengthLimitExceeded);

        When(request => request.Identifier is not null, () =>
        {
            RuleFor(request => request.Identifier.IdentityScheme)
                .Must(t => t != SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.Unknown)
                .WithErrorCode(ErrorCodeMapper.IdentifierInvalid);
        });

        When(request => request.CaseId.HasValue, () =>
        {
            RuleFor(request => request.CaseId!.Value)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodeMapper.CaseIdInvalid);
        });

        When(request => request.CustomId is not null, () =>
        {
            RuleFor(request => request.CustomId!)
                .Matches("^([A-Za-z0-9-_]{0,450})$")
                .WithErrorCode(ErrorCodeMapper.CustomIdInvalid);
        });
    }
}
