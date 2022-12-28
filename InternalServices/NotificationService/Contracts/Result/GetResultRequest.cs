using CIS.Core.Validation;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class GetResultRequest : IRequest<GetResultResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}