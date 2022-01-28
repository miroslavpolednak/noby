using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.FixationPeriodLengths
{
    [DataContract]
    public class FixationPeriodLengthsRequest : IRequest<List<FixationPeriodLengthsItem>>
    {
    }
}
