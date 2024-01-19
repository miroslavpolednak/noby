using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.LegacyContracts.Common;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Sms;

[ProtoContract]
public class SendSmsRequest : IRequest<SendSmsResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public string PhoneNumber { get; set; } = default!;
    
    [ProtoMember(2)]
    public int? ProcessingPriority { get; set; }

    [ProtoMember(3)]
    public string Type { get; set; } = string.Empty;
    
    [ProtoMember(4)]
    public string Text { get; set; } = string.Empty;

    [ProtoMember(5)]
    public Identifier? Identifier { get; set; }
    
    [ProtoMember(6)]
    public long? CaseId { get; set; }
    
    [ProtoMember(7)]
    public string? CustomId { get; set; }
    
    [ProtoMember(8)]
    public string? DocumentId { get; set; }
    
    [ProtoMember(9)]
    public DocumentHash? DocumentHash { get; set; }
}