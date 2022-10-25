using CIS.Core.Validation;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class ResultGetRequest : IRequest<ResultGetResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}