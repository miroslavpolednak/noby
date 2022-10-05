using MediatR;
using ProtoBuf;
using System.Text.Json.Serialization;

namespace CIS.InternalServices.DocumentArchiveService.Contracts;

[ProtoContract]
public class GenerateDocumentIdRequest
    : IRequest<GenerateDocumentIdResponse>
{
    /// <summary>
    /// kód volajícího prostředí systému
    /// </summary>
    [ProtoMember(1)]
    public EnvironmentNames EnvironmentName { get; set; }

    /// <summary>
    /// index prostředí (default 0) - jednociferné číslo
    /// </summary>
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
    Dev = 1,

    [ProtoEnum]
    Fat = 2,

    [ProtoEnum]
    Sit = 3,

    [ProtoEnum]
    Uat = 4,

    [ProtoEnum]
    Preprod = 5,

    [ProtoEnum]
    Edu = 6,

    [ProtoEnum]
    Prod = 7
}
