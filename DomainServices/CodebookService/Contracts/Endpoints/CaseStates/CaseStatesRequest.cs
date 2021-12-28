using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.CaseStates
{
    [DataContract]
    public class CaseStatesRequest : IRequest<List<CaseStateItem>>
    {
    }
}
