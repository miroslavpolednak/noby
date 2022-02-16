using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum Mandants : byte
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
