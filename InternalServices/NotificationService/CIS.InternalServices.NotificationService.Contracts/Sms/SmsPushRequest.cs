using CIS.Core.Validation;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract]
public class SmsPushRequest : SmsBaseRequest, IRequest<SmsPushResponse>, IValidatableRequest
{
    [ProtoMember(4)]
    public string Text { get; set; } = string.Empty;
}