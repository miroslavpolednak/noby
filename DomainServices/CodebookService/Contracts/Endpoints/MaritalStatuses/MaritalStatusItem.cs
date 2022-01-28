using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.MaritalStatuses
{
    [DataContract]
    public class MaritalStatusItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string C4mStatus { get; set; }
    }
}
