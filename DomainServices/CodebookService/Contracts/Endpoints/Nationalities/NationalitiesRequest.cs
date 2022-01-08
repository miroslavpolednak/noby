using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.Nationalities
{
    [DataContract]
    public class NationalitiesRequest : IRequest<List<GenericCodebookItem>>
    {
    }
}
