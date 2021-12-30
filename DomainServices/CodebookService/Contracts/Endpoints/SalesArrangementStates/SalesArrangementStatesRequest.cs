using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates
{
    [DataContract]
    public class SalesArrangementStatesRequest : IRequest<List<GenericCodebookItem>>
    {
    }
}
