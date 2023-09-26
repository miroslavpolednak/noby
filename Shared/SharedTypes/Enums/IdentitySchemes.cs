using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum IdentitySchemes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    [JsonIgnore]
    Unknown = 0,

    [Display(Name = "Modrá pyramida")]
    [EnumMember]
    Mp = 1,

    [Display(Name = "Komerční banka")]
    [EnumMember]
    Kb = 2
}
