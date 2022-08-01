using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum LoanInterestRateAnnouncedTypes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "standardní")]
    [EnumMember]
    STD = 1,

    [Display(Name = "VIP")]
    [EnumMember]
    VIP = 2,

    [Display(Name = "developerská")]
    [EnumMember]
    DEV = 3,

    [Display(Name = "zaměstnanecká")]
    [EnumMember]
    ZAM = 4,
}
