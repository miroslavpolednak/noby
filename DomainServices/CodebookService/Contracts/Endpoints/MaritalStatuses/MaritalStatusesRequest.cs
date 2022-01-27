using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.MaritalStatuses
{
    [DataContract]
    public class MaritalStatusesRequest : IRequest<List<MaritalStatusItem>>
    {
    }
}
