using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.GetDeveloperProject;

[DataContract]
public class DeveloperProjectItem
{
    [JsonIgnore]
    [DataMember(Order = 1)]
    public int Id { get; set; }

    /// <summary>
    /// Jméno developerského projektu
    /// </summary>
    [DataMember(Order = 2)]
    public string Name { get; set; }

    /// <summary>
    /// Upozornění pro KB
    /// </summary>
    [DataMember(Order = 3)]
    public string WarningForKb { get; set; }

    /// <summary>
    /// Upozornění pro MP
    /// </summary>
    [DataMember(Order = 4)]
    public string WarningForMp { get; set; }

    /// <summary>
    /// Stránky projektu
    /// </summary>
    [DataMember(Order = 5)]
    public string Web { get; set; }

    /// <summary>
    /// Hromadné ocenění
    /// </summary>
    [DataMember(Order = 6)]
    public string MassEvaluationText { get; set; }

    [JsonIgnore]
    [DataMember(Order = 7)]
    public string Recommandation { get; set; }

    /// <summary>
    /// Lokalita
    /// </summary>
    [DataMember(Order = 8)]
    public string Place { get; set; }

    [JsonIgnore]
    [DataMember(Order = 9)]
    public bool IsValid { get; set; }
}
