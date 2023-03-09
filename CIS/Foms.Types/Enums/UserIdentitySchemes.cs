using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum UserIdentitySchemes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "KBUID")]
    [EnumMember]
    KbUid = 4,

    [Display(Name = "M04ID")]
    [EnumMember]
    M04Id = 5,

    [Display(Name = "M17ID")]
    [EnumMember]
    M17Id = 6,

    [Display(Name = "BrokerId")]
    [EnumMember]
    BrokerId = 7,

    [Display(Name = "MPAD")]
    [EnumMember]
    Mpad = 8
}
