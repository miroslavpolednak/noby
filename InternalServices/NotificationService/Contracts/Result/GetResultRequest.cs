using CIS.Core.Validation;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result;

[ProtoContract]
public class GetResultRequest : IRequest<GetResultResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}