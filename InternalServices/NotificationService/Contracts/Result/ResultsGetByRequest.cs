using CIS.Core.Validation;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class ResultsGetByRequest : IRequest<ResultsGetByResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public string ClientId { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public string CustomId { get; set; } = string.Empty;
    
    [ProtoMember(3)]
    public string DocumentId { get; set; } = string.Empty;
}