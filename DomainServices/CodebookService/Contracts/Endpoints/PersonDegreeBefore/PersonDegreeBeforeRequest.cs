using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.PersonDegreeBefore
{
    [DataContract]
    public class PersonDegreeBeforeRequest : IRequest<List<GenericCodebookItem>>
    {
    }
}
