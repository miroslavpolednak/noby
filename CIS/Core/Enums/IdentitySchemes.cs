using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Core.Enums;

[DataContract]
public enum IdentitySchemes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "Modrá pyramida")]
    [EnumMember]
    Mp = 1,

    [Display(Name = "Komerční banka")]
    [EnumMember]
    Kb = 2
}
