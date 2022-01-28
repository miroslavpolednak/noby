using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.FixationPeriodLengths
{
    [DataContract]
    public class FixationPeriodLengthsItem
    {
        [DataMember(Order = 1)]
        public int ProductInstanceTypeId { get; set; }

        [DataMember(Order = 2)]
        public int FixationMonths { get; set; }
    }
}
