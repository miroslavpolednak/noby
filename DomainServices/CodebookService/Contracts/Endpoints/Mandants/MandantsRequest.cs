using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.Mandants
{
    [DataContract]
    public class MandantsRequest : IRequest<List<MandantsItem>>
    {
    }
}
