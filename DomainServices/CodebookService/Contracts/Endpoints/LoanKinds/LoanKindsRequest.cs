using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.LoanKinds
{
    [DataContract]
    public class LoanKindsRequest : IRequest<List<LoanKindsItem>>
    {
    }
}
