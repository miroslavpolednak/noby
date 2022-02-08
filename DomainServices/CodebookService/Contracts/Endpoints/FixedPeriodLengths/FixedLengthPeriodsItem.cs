using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.FixedPeriodLengths
{
    [DataContract]
    public class FixedLengthPeriodsItem
    {
        [DataMember(Order = 1)]
        public int ProductInstanceTypeId { get; set; }

        [DataMember(Order = 2)]
        public int FixedLengthPeriod { get; set; }
    }
}
