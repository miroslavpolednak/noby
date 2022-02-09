using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using DomainServices.CodebookService.Contracts.Endpoints.CaseStates;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<CaseStateItem>> CaseStates(CaseStatesRequest request, CallContext context = default);
}
