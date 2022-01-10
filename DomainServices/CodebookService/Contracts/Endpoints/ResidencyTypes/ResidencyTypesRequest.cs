using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ResidencyTypes
{
    [DataContract]
    public class ResidencyTypesRequest : IRequest<List<GenericCodebookItem>>
    {
    }
}
