using DomainServices.DocumentOnSAService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;

internal class DocumentOnSaInfo
{
    public DocumentOnSaInfo(ICollection<DocumentOnSAToSign> documentsOnSa)
    {
        FinalDocument = documentsOnSa.FirstOrDefault(d => d.IsFinal);
        FirstSignatureDate = GetFirstSignatureDate(documentsOnSa);
        FormIdList = GetFormIdList(documentsOnSa);
    }

    public required int SignatureMethodId { get; init; }

    public DocumentOnSAToSign? FinalDocument { get; }

    public DateTime? FirstSignatureDate { get; }

    public List<object> FormIdList { get; }

    private static DateTime? GetFirstSignatureDate(IEnumerable<DocumentOnSAToSign> documentsOnSa) =>
        documentsOnSa.Where(d => d.IsSigned).OrderBy(d => d.SignatureDateTime).Select(d => d.SignatureDateTime.ToDateTime()).FirstOrDefault();

    private static List<object> GetFormIdList(IEnumerable<DocumentOnSAToSign> documentsOnSa) => documentsOnSa.Select(d => new { d.FormId }).ToList<object>();
}