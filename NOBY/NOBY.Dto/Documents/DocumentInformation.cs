﻿namespace NOBY.Dto.Documents;

public class DocumentInformation
{
    /// <summary>
    /// GUID dokumentu v dočasném úložišti
    /// </summary>
    public Guid? Guid { get; set; }

    /// <summary>
    /// Id EA kódu
    /// </summary>
    public int? EaCodeMainId { get; set; }

    /// <summary>
    /// Popis dokumentu
    /// </summary>
    public string? Description { get; set; }
}