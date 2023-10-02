using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum PayoutTypes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "Výplata  s konsolidací")]
    [EnumMember]
    WithConsolidation = 1,

    [Display(Name = "Výplata bez konsolidace")]
    [EnumMember]
    WithoutConsolidation = 2,
}
