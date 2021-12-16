using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.CaseStates
{
    [DataContract]
    public class CaseStateItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public bool IsDefaultNewState { get; set; }
    }
}
