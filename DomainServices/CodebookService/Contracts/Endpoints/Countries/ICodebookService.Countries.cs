using DomainServices.CodebookService.Contracts.Endpoints.Countries;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<CountriesItem>> Countries(CountriesRequest request, CallContext context = default);
    }
}
