﻿using CIS.Core.Validation;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Result;

[ProtoContract]
public class SearchResultsRequest : IRequest<SearchResultsResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public string? Identity { get; set; }

    [ProtoMember(2)]
    public string? IdentityScheme { get; set; }
    
    [ProtoMember(3)]
    public long? CaseId { get; set; }
    
    [ProtoMember(4)]
    public string? CustomId { get; set; }
    
    [ProtoMember(5)]
    public string? DocumentId { get; set; }
}