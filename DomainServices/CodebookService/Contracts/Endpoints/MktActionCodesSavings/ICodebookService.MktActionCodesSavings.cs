using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> MktActionCodesSavings(Endpoints.MktActionCodesSavings.MktActionCodesSavingsRequest request, CallContext context = default);
    }
}
