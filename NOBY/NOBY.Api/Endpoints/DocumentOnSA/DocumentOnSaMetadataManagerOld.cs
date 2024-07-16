namespace NOBY.Api.Endpoints.DocumentOnSA;

public class DocIdentificator
{
    public int? DocumentTypeId { get; set; }

    public int? EACodeMainId { get; set; }
}

public class DocumentOnSAInfo
{
    public bool IsValid { get; set; }

    public int? DocumentOnSAId { get; set; }

    public bool IsSigned { get; set; }

    public Source Source { get; set; }

    public int? SalesArrangementTypeId { get; set; }

    public int SignatureTypeId { get; set; }

    public int? EaCodeMainId { get; set; }

    public IReadOnlyCollection<string> EArchivIdsLinked { get; set; } = null!;
}
