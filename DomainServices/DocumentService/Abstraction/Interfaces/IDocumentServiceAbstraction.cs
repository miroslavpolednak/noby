using CIS.Core;
using CIS.Core.Results;


namespace DomainServices.DocumentService.Abstraction.Interfaces
{
    public interface IDocumentServiceAbstraction
    {
        /// <summary>
        /// Detail dokumentu  (Metoda slouží k získání detailu dokumentu, jeho kompletních metadat a kopie samotného dokumentu (BLOB) na základě documentId (= id dokumentu v eArchivu).) //TODO: response type (metadata, blob) ?
        /// </summary>
        /// <returns>
        /// SuccessfulServiceCallResult[GetDocumentResponse] - OK
        /// </returns>
        Task<IServiceCallResult> GetDocument(string documentId, IdentitySchemes mandant, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Seznamu dokumentů z ePodpisů a eArchivu podle CaseId
        /// </summary>
        /// <returns>
        /// SuccessfulServiceCallResult[GetDocumentsListByCaseIdResponse] - OK
        /// </returns>
        Task<IServiceCallResult> GetDocumentsListByCaseId(Int32 caseId, CancellationToken cancellationToken = default(CancellationToken));
    }
}