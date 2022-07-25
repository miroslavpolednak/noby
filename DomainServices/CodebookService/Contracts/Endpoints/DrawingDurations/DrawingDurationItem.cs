using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.DrawingDurations
{
    [DataContract]
    public class DrawingDurationItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public int DrawingDuration { get; set; }

        [DataMember(Order = 3)]
        public bool IsDefault { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public bool IsValid { get; set; }
    }
}
