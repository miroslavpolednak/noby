using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes
{
    [DataContract]
    public class SalesArrangementTypesRequest : IRequest<List<SalesArrangementTypeItem>>
    {
    }
}
