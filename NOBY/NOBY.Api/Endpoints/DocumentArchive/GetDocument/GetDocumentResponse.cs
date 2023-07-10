namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentResponse : IRequest<GetDocumentResponse>
{
    public DocumentMetadata Metadata { get; set; } = null!;

    public FileInfo Content { get; set; } = null!;
}

public class DocumentMetadata
{
    /// <summary>
    /// Unikátní Id obchodního případu
    /// </summary>
    public long? CaseId { get; set; }
    
    /// <summary>
    /// ID dokumentu
    /// </summary>
    public string DocumentId { get; set; } = null!;
    
    /// <summary>
    /// EaCodeMain.Id https://wiki.kb.cz/display/HT/EaCodeMain
    /// </summary>
    public int? EaCodeMainId { get; set; }
    
    /// <summary>
    /// Jméno souboru
    /// </summary>
    public string Filename { get; set; } = null!;

    /// <summary>
    /// Popis dokumentu 
    /// </summary>
    public string Description { get; set; } = null!;
    
    public int? OrderId { get; set; }
    
    /// <summary>
    /// Datum přijetí dokumentu 
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Login autora dokumentu, kdo dokument uložil
    /// </summary>
    public string AuthorUserLogin { get; set; } = null!;
    
    public string Priority { get; set; } = null!;
    
    /// <summary>
    /// Status dokumentu 
    /// </summary>
    public string Status { get; set; } = null!;

    /// <summary>
    /// Jedná se o seskupování dokumentů do virtuálních složek 
    /// </summary>
    public string FolderDocument { get; set; } = null!;

    public string FolderDocumentId { get; set; } = null!;

    public string DocumentDirection { get; set; } = null!;
    
    /// <summary>
    /// zdrojová aplikace dokumentu 
    /// </summary>
    public string SourceSystem { get; set; } = null!;
    
    /// <summary>
    /// ID formuláře 
    /// </summary>
    public string FormId { get; set; } = null!;
    
    /// <summary>
    /// Číslo smlouvy 
    /// </summary>
    public string ContractNumber { get; set; } = null!;
    
    /// <summary>
    /// Číslo zástavní smlouvy 
    /// </summary>
    public string PledgeAgreementNumber { get; set; } = null!;

    /// <summary>
    /// Kompletnost jen archiv TCP 1 - kompletní, 0 = nekompletní, null = neurčeno 
    /// </summary>
    public int? Completeness { get; set; }

    /// <summary>
    /// Vedlejší heslo jen archiv TCP 
    /// </summary>
    public int[] MinorCodes { get; set; } = null!;
}

public class FileInfo
{
    /// <summary>
    /// Soubour (pole bajtů) ve formě base64string
    /// </summary>
    public byte[] BinaryData { get; set; } = null!;
    
    /// <summary>
    /// MIME typ dokumentu 
    /// </summary>
    public string MimeType { get; set; } = null!;
}