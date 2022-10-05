using ProtoBuf;

namespace CIS.InternalServices.DocumentArchiveService.Contracts;

[ProtoContract]
public class GenerateDocumentIdResponse
{
    /// <summary>
    /// plné ID dokumentu - je určeno jako identifikátor pro uložení do eArchivu
    /// </summary>
    [ProtoMember(1)]
    public string? DocumentId { get; set; }
}
