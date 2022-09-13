using CIS.Core.Validation;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract]
public class SmsFromTemplatePushRequest : SmsBaseRequest, IRequest<SmsFromTemplatePushResponse>, IValidatableRequest
{
    [ProtoMember(4)]
    public Dictionary<string, object> Placeholders { get; set; } = default!;
}