// using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ObligationTypes
{
    [DataContract]
    public class ObligationTypesItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string Code { get; set; }

        [DataMember(Order = 4)]
        public List<int> ObligationCorrectionTypeIds { get; set; }

        [DataMember(Order = 5)]
        // TODO: ačkoliv se to nemá propagovat na FE API, používá to aktuálně RIP na CodebooksAPI !? 
        // [JsonIgnore]
        public bool IsValid { get; set; }

        [DataMember(Order = 6)]
        public int Order { get; set; }
    }
}
