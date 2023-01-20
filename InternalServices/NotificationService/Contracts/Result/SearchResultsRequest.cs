using CIS.Core.Validation;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result;

[ProtoContract]
public class SearchResultsRequest : IRequest<SearchResultsResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public string? Identity { get; set; }

    [ProtoMember(2)]
    public string? IdentityScheme { get; set; }
    
    [ProtoMember(3)]
    public string? CustomId { get; set; }
    
    [ProtoMember(4)]
    public string? DocumentId { get; set; }
}