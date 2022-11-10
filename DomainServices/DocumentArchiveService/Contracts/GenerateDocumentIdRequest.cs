using MediatR;
using ProtoBuf;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DomainServices.DocumentArchiveService.Contracts;

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
    [EnumMember(Value = "D")]
    Dev = 1,

    [ProtoEnum]
    [EnumMember(Value = "F")]
    Fat = 2,

    [ProtoEnum]
    [EnumMember(Value = "S")]
    Sit = 3,

    [ProtoEnum]
    [EnumMember(Value = "U")]
    Uat = 4,

    [ProtoEnum]
    [EnumMember(Value = "P")]
    Preprod = 5,

    [ProtoEnum]
    [EnumMember(Value = "E")]
    Edu = 6,

    [ProtoEnum]
    [EnumMember(Value = "R")]
    Prod = 7
}
