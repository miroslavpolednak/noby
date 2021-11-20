using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ProductInstanceTypes
{
    [DataContract]
    public class ProductInstanceTypesRequest : IRequest<List<ProductInstanceTypeItem>>
    {
    }
}
