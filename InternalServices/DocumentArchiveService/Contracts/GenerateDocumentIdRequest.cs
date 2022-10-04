using MediatR;
using ProtoBuf;
using System.Text.Json.Serialization;

namespace CIS.InternalServices.DocumentArchiveService.Contracts;

[ProtoContract]
public class GenerateDocumentIdRequest
    : IRequest<GenerateDocumentIdResponse>
{
    [ProtoMember(1)]
    public EnvironmentNames EnvironmentName { get; set; }

    [ProtoMember(2)]
    public int? EnvironmentIndex { get; set; }
}

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EnvironmentNames
{
    [ProtoEnum]
    Unknown = 0,

    [ProtoEnum]
    DEV = 1,

    [ProtoEnum]
    FAT = 2,

    [ProtoEnum]
    SIT = 3,

    [ProtoEnum]
    UAT = 4,

    [ProtoEnum]
    PREPROD = 5,

    [ProtoEnum]
    EDU = 6,

    [ProtoEnum]
    PROD = 7
}
