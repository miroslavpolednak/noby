using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.FixedRatePeriods
{
    [DataContract]
    public class FixedRatePeriodsRequest : IRequest<List<FixedRatePeriodsItem>>
    {
    }
}
