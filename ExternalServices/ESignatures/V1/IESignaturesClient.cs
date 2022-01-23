
using ExternalServices.ESignatures.V1.ESignaturesWrapper;

namespace ExternalServices.ESignatures.V1
{
    public interface IESignaturesClient
    {
        /// <summary>
        /// Popis . . . .
        /// </summary>
        Task<IServiceCallResult> PrepareDocument(PrepareDocumentRequest request, string org);

    }
}
