using ProtoBuf;

namespace CIS.InternalServices.DocumentArchiveService.Contracts;

[ProtoContract]
public class GenerateDocumentIdResponse
{
    [ProtoMember(1)]
    public string? ResourceProcessId { get; set; }
}
