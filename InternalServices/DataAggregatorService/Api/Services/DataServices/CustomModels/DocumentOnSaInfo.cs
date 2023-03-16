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

    public required int SigningMethodId { get; init; }

    public DocumentOnSAToSign? FinalDocument { get; }

    public DateTime? FirstSignatureDate { get; }

    public List<string> FormIdList { get; }

    private static DateTime? GetFirstSignatureDate(IEnumerable<DocumentOnSAToSign> documentsOnSa) =>
        documentsOnSa.Where(d => d.IsSigned).OrderBy(d => d.SignatureDateTime).Select(d => d.SignatureDateTime.ToDateTime()).FirstOrDefault();

    private static List<string> GetFormIdList(IEnumerable<DocumentOnSAToSign> documentsOnSa) => documentsOnSa.Select(d => d.FormId).ToList();
}