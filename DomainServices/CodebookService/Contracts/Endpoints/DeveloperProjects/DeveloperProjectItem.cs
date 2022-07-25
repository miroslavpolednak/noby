using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.DeveloperProjects
{
    [DataContract]
    public class DeveloperProjectItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        [JsonIgnore]
        public int DeveloperId { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public string WarningForKb { get; set; }

        [DataMember(Order = 5)]
        [JsonIgnore]
        public string WarningForMp { get; set; }

        [DataMember(Order = 6)]
        [JsonIgnore]
        public string Web { get; set; }

        [DataMember(Order = 7)]
        [JsonIgnore]
        public bool MassValuation { get; set; }

        [DataMember(Order = 8)]
        [JsonIgnore]
        public string Recommandation { get; set; }

        [DataMember(Order = 9)]
        [JsonIgnore]
        public string Place { get; set; }

        [DataMember(Order = 10)]
        [JsonIgnore]
        public bool IsValid { get; set; }
    }
}
