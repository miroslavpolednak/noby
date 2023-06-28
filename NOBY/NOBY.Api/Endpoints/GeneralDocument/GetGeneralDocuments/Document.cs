namespace NOBY.Api.Endpoints.GeneralDocument.GetGeneralDocuments;

public class Document
{
    /// <summary>
    /// Id dokumentu
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Jméno dokumentu
    /// </summary>
    /// <example>Potvrzení o příjmech</example>
    public string? Name { get; set; }
    
    /// <summary>
    /// Jméno souboru ke stažení
    /// </summary>
    /// <example>potvrzeni_o_prijmech.pdf</example>
    public string? Filename { get; set; }
    
    /// <summary>
    /// Formát dokumentu
    /// </summary>
    /// <example>PDF</example>
    public string? Format { get; set; }
}