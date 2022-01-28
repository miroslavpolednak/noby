using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ProductLoanKinds
{
    [DataContract]
    public class ProductLoanKindsRequest : IRequest<List<ProductLoanKindsItem>>
    {
    }
}
