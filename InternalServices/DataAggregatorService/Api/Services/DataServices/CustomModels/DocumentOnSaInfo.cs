﻿using DomainServices.CodebookService.Contracts.Endpoints.SigningMethodsForNaturalPerson;
using DomainServices.DocumentOnSAService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;

internal class DocumentOnSaInfo
{
    private readonly ICollection<DocumentOnSAToSign> _documentsOnSa;
    private readonly List<SigningMethodsForNaturalPersonItem> _signingMethods;

    public DocumentOnSaInfo(ICollection<DocumentOnSAToSign> documentsOnSa, List<SigningMethodsForNaturalPersonItem> signingMethods)
    {
        _documentsOnSa = documentsOnSa;
        _signingMethods = signingMethods;

        Configure();
    }

    public DocumentOnSAToSign? FinalDocument { get; private set; }

    public int SignatureMethodId { get; private set; }

    public DateTime? FirstSignatureDate { get; private set; }

    public List<object> FormIdList { get; private set; } = new();

    public void Configure(int? documentTypeId = default)
    {
        var documentsOnSa = GetDocumentsOnSa(documentTypeId).ToList();

        FinalDocument = documentsOnSa.FirstOrDefault(d => d.IsFinal);
        SignatureMethodId = GetSignatureMethodId(documentsOnSa);
        FirstSignatureDate = GetFirstSignatureDate(documentsOnSa);
        FormIdList = GetFormIdList(documentsOnSa);
    }

    private IEnumerable<DocumentOnSAToSign> GetDocumentsOnSa(int? documentTypeId = default) => 
        documentTypeId.HasValue ? _documentsOnSa.Where(d => d.DocumentTypeId == documentTypeId) : _documentsOnSa;

    private int GetSignatureMethodId(IEnumerable<DocumentOnSAToSign> documentsOnSa)
    {
        var signatureMethodCode = documentsOnSa.LastOrDefault(d => d.IsValid && d.IsSigned)?.SignatureMethodCode;

        return _signingMethods.Where(s => s.Code == signatureMethodCode).Select(s => s.StarbuildEnumId).FirstOrDefault(1);
    }

    private static DateTime? GetFirstSignatureDate(IEnumerable<DocumentOnSAToSign> documentsOnSa) =>
        documentsOnSa.Where(d => d.IsSigned).OrderBy(d => d.SignatureDateTime).Select(d => d.SignatureDateTime.ToDateTime()).FirstOrDefault();

    private static List<object> GetFormIdList(IEnumerable<DocumentOnSAToSign> documentsOnSa) => documentsOnSa.Select(d => new { d.FormId }).ToList<object>();
}