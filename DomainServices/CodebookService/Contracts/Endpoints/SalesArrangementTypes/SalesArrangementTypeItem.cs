using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes
{
    [DataContract]
    public class SalesArrangementTypeItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public int? ProductTypeId { get; set; }
        
        [DataMember(Order = 4)]
        public bool IsDefault { get; set; }
    }
}
