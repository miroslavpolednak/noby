using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.FormTypes
{
    [DataContract]
    public class FormTypeItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }


        [DataMember(Order = 2)]
        public int Type { get; set; }


        [DataMember(Order = 3)]
        public string Version { get; set; }


        [DataMember(Order = 4)]
        public string Name { get; set; }


        [DataMember(Order = 5)]
        public int MandantId { get; set; }


        [DataMember(Order = 6)]
        [JsonIgnore]
        public bool IsValid { get; set; }

    }
}