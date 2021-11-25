using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> ActionCodesSavingsLoan(Endpoints.ActionCodesSavingsLoan.ActionCodesSavingsLoanRequest request, CallContext context = default);
    }
}
