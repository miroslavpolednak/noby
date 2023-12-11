using CIS.Core.Validation;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Resend;

[ProtoContract]
public class ResendRequest
    : IRequest, IValidatableRequest
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}
