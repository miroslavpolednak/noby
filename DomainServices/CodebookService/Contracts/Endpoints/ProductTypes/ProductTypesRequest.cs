using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ProductTypes
{
    [DataContract]
    public class ProductTypesRequest : IRequest<List<ProductTypeItem>>
    {
    }
}
