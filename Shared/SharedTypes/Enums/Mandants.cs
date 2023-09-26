using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum Mandants : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "Modrá pyramida", ShortName = "MPSS")]
    [EnumMember]
    Mp = 1,

    [Display(Name = "Komerční banka", ShortName = "KB")]
    [EnumMember]
    Kb = 2
}
