using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates
{
    [DataContract]
    public class SalesArrangementStateItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        [JsonIgnore]
        public bool IsDefaultNewState { get; set; }
    }
}
