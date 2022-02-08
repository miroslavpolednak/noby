using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.FixedPeriodLengths
{
    [DataContract]
    public class FixedLengthPeriodsRequest : IRequest<List<FixedLengthPeriodsItem>>
    {
    }
}
