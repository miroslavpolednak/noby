using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.GetDeveloper;

[DataContract]
public class DeveloperItem
{
    [JsonIgnore]
    [DataMember(Order = 1)]
    public int Id { get; set; }

    /// <summary>
    /// Jméno developera
    /// </summary>
    [DataMember(Order = 2)]
    public string Name { get; set; }

    /// <summary>
    /// ICO/RČ developera
    /// </summary>
    [DataMember(Order = 3)]
    public string Cin { get; set; }

    /// <summary>
    /// Id statusu developera
    /// </summary>
    [DataMember(Order = 5)]
    public int? StatusId { get; set; }

    /// <summary>
    /// Status developera
    /// </summary>
    [DataMember(Order = 6)]
    public string StatusText { get; set; }

    [JsonIgnore]
    [DataMember(Order = 7)]
    public bool IsValid { get; set; }

    /// <summary>
    /// Balíček benefitu
    /// </summary>
    [DataMember(Order = 8)]
    public bool BenefitPackage { get; set; }

    [JsonIgnore]
    [DataMember(Order = 9)]
    public bool IsBenefitValid { get; set; }

    /// <summary>
    /// Benefity nad rámec balíčku
    /// </summary>
    [DataMember(Order = 10)]
    public bool BenefitsBeyondPackage { get; set; }
}