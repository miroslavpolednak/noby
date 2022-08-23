using DomainServices.CodebookService.Contracts.Endpoints.DrawingTypes;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<DrawingTypeItem>> DrawingTypes(DrawingTypesRequest request, CallContext context = default);
    }
}
