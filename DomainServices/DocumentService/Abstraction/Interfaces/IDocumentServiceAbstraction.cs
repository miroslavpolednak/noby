using CIS.Core.Enums;
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
        /// SuccessfulServiceCallResult[GetDocumentsListResponse] - OK
        /// </returns>
        Task<IServiceCallResult> GetDocumentsListByCaseId(Int32 caseId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Seznamu dokumentů z ePodpisů a eArchivu na základě contractNumber a typu mandanta (KB/MP).
        /// </summary>
        /// <returns>
        /// SuccessfulServiceCallResult[GetDocumentsListResponse] - OK
        /// </returns>
        Task<IServiceCallResult> GetDocumentsListByContractNumber(string contractNumber, IdentitySchemes mandant, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Seznamu dokumentů z ePodpisů a eArchivu na základě customer id. (Metoda slouží jako obálka eArchivů (primárně pro CAS - Credit Antifraud System)) //TODO: request params?
        /// </summary>
        /// <returns>
        /// SuccessfulServiceCallResult[GetDocumentsListResponse] - OK
        /// </returns>
        Task<IServiceCallResult> GetDocumentsListByCustomerId(string customerId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Seznamu dokumentů z ePodpisů a eArchivu na základě relation id.
        /// </summary>
        /// <returns>
        /// SuccessfulServiceCallResult[GetDocumentsListResponse] - OK
        /// </returns>
        Task<IServiceCallResult> GetDocumentsListByRelationId(string relationId, IdentitySchemes mandant, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Stav dokumentu.
        /// </summary>
        /// <returns>
        /// SuccessfulServiceCallResult[GetDocumentStatusResponse] - OK
        /// </returns>
        Task<IServiceCallResult> GetDocumentStatus(string documentId, IdentitySchemes mandant, CancellationToken cancellationToken = default(CancellationToken));
    }
}