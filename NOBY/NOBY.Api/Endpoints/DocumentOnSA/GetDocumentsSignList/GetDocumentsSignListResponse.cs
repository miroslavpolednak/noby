﻿using CIS.Foms.Enums;
using NOBY.Dto.Signing;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;

public class GetDocumentsSignListResponse
{
    public IReadOnlyCollection<GetDocumentsSignListData> Data { get; set; } = null!;
}

public class GetDocumentsSignListData
{
    /// <summary>
    /// Id dokumentu. Vrací se pouze pro dokumenty se zahájeným podepisovacím procesem.
    /// </summary>
    public int? DocumentOnSAId { get; set; }

    /// <summary>
    /// Typ dokumentu. Číselník DocumentOnSAType.
    /// </summary>
    public int? DocumentTypeId { get; set; }

    /// <summary>
    /// Businessové modré ID
    /// </summary>
    public string? FormId { get; set; }

    /// <summary>
    /// Příznak, zda byl dokument již podepsán.
    /// </summary>
    public bool IsSigned { get; set; }

    /// <summary>
    /// Metoda podpisu (manuální/elektronický). Číselník SignatureType.
    /// </summary>
    public int? SignatureTypeId { get; set; }

    /// <summary>
    /// Timestamp, kdy došlo k podpisu celého dokumentu.
    /// </summary>
    public DateTime? SignatureDateTime { get; set; }

    public SignatureState SignatureState { get; set; } = null!;

    public EACodeMainItem EACodeMainItem { get; set; } = null!;

    /// <summary>
    /// Only for CRS
    /// </summary>
    public CustomerOnSa CustomerOnSa { get; set; } = null!;

    /// <summary>
    /// Příznak, zda byl elektronicky podepisovaný dokument odeslán na klienta.
    /// </summary>
    public bool IsPreviewSentToCustomer { get; set; }

    /// <summary>
    /// Id elektronicky podepisovaného dokumentu nahraného v ePodpisech
    /// </summary>
    public string? ExternalId { get; set; }

    public Source Source { get; set; }
}

