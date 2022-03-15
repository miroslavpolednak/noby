using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.FixedRatePeriods
{
    [DataContract]
    public class FixedRatePeriodsItem
    {
        [DataMember(Order = 1)]
        public int ProductTypeId { get; set; }

        [DataMember(Order = 2)]
        public int FixedRatePeriod { get; set; }
    }
}
