using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ProductLoanPurposes
{
    [DataContract]
    public class ProductLoanPurposesRequest : IRequest<List<ProductLoanPurposesItem>>
    {
    }
}
