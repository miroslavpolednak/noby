﻿using DomainServices.DocumentOnSAService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;

internal class DocumentOnSaInfo
{
    private readonly ICollection<DocumentOnSAToSign> _documentsOnSa;

    public DocumentOnSaInfo(ICollection<DocumentOnSAToSign> documentsOnSa)
    {
        _documentsOnSa = documentsOnSa;
        Configure();
    }

    public DocumentOnSAToSign? FinalDocument { get; private set; }

    public DocumentOnSAToSign? LastSignedDocument { get; private set; }

    public int SignatureMethodId { get; private set; }

    public DateTime? FirstSignatureDate { get; private set; }

    public List<object> FormIdList { get; private set; } = new();

    public void Configure(int? documentTypeId = default)
    {
        var documentsOnSa = GetDocumentsOnSa(documentTypeId).ToList();

        FinalDocument = documentsOnSa.FirstOrDefault(d => d.IsFinal);
        LastSignedDocument = documentsOnSa.OrderByDescending(d => d.SignatureDateTime).FirstOrDefault(d => d.IsSigned && d.IsValid);
        SignatureMethodId = GetSignatureTypeId(documentsOnSa);
        FirstSignatureDate = GetFirstSignatureDate(documentsOnSa);
        FormIdList = GetFormIdList(documentsOnSa);
    }

    private IEnumerable<DocumentOnSAToSign> GetDocumentsOnSa(int? documentTypeId = default) => 
        documentTypeId.HasValue ? _documentsOnSa.Where(d => d.DocumentTypeId == documentTypeId) : _documentsOnSa;

    private static int GetSignatureTypeId(IEnumerable<DocumentOnSAToSign> documentsOnSa)
    {
        const int DefaultSignatureMethodId = 1;

        var signatureTypeId = documentsOnSa.LastOrDefault(d => d.IsValid && d.IsSigned)?.SignatureTypeId ?? DefaultSignatureMethodId;

        return signatureTypeId switch
        {
            1 => 1,
            _ => 2
        };
    }

    private static DateTime? GetFirstSignatureDate(IEnumerable<DocumentOnSAToSign> documentsOnSa) =>
        documentsOnSa.Where(d => d.IsSigned).OrderBy(d => d.SignatureDateTime).Select(d => (DateTime?)d.SignatureDateTime.ToDateTime()).FirstOrDefault();

    private static List<object> GetFormIdList(IEnumerable<DocumentOnSAToSign> documentsOnSa) => documentsOnSa.Select(d => new { d.FormId }).ToList<object>();
}