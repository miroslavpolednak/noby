using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.PersonDegreeAfter
{
    [DataContract]
    public class PersonDegreeAfterRequest : IRequest<List<GenericCodebookItem>>
    {
    }
}
