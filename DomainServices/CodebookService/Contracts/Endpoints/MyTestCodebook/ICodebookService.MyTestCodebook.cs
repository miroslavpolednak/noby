using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        /// <summary>
        /// Objekt request musi implementovat MediatR interface IRequest[], jinak nedojde ke spusteni handleru
        /// </summary>
        [OperationContract]
        Task<List<GenericCodebookItem>> MyTestCodebook(Endpoints.MyTestCodebook.MyTestCodebookRequest request, CallContext context = default);
    }
}
