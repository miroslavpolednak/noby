using System.Text.Json.Serialization;
namespace DomainServices.CodebookService.Contracts.Endpoints.ClassficationOfEconomicActivities;

[DataContract]
public class ClassficationOfEconomicActivitiesItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    [JsonIgnore]
    public bool IsValid { get; set; }
}