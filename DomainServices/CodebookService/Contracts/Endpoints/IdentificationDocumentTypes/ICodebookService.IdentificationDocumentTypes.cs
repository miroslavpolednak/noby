using DomainServices.CodebookService.Contracts.Endpoints.IdentificationDocumentTypes;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<IdentificationDocumentTypesItem>> IdentificationDocumentTypes(IdentificationDocumentTypesRequest request, CallContext context = default);
    }
}
