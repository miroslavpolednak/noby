using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.MandantTypes.MandantTypesItem>> MandantTypes(Endpoints.MandantTypes.MandantTypesRequest request, CallContext context = default);
    }
}
