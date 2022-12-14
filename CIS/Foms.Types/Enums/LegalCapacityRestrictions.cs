using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum LegalCapacityRestrictions : byte
{
    [Display(Name = "unknown", ShortName = "")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "Omezená svéprávnost", ShortName = "disability")]
    [EnumMember]
    D = 1,

    [Display(Name = "Bez omezení", ShortName = "no restriction")]
    [EnumMember]
    N = 2,

    [Display(Name = "Jiné omezení", ShortName = "other restriction")]
    [EnumMember]
    O = 3,
}