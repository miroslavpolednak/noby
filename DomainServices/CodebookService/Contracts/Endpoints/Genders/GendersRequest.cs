using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.Genders
{
    [DataContract]
    public class GendersRequest : IRequest<List<GenericCodebookItem>>
    {
    }
}
