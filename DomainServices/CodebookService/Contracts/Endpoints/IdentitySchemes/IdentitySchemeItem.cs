using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.IdentitySchemes
{
    [DataContract]
    public sealed class IdentitySchemeItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        [JsonIgnore]
        public string Code { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public int MandantId { get; set; }

        [DataMember(Order = 5)]
        [JsonIgnore]
        public string Category { get; set; }

        [DataMember(Order = 6)]
        [JsonIgnore]
        public int? ChannelId { get; set; }
    }
}