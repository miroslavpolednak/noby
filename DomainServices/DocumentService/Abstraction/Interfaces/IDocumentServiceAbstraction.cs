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
        /// SuccessfulServiceCallResult[GetBuildingSavingsDataResponse] - OK  //TODO: response type ?
        /// </returns>
        Task<IServiceCallResult> GetDocument(string documentId, IdentitySchemes mandant, CancellationToken cancellationToken = default(CancellationToken));

    }
}