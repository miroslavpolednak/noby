using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes
{
    [DataContract]
    public class SalesArrangementTypeItem
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string VersionNumber { get; set; }
    }
}
