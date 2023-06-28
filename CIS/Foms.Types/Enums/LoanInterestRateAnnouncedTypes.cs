using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum LoanInterestRateAnnouncedTypes : byte
{
    [Display(Name = "unknown", ShortName = "Unknown")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "standardní", ShortName = "STD")]
    [EnumMember]
    STD = 1,

    [Display(Name = "VIP", ShortName = "VIP")]
    [EnumMember]
    VIP = 2,

    [Display(Name = "developerská", ShortName = "DEV")]
    [EnumMember]
    DEV = 3,

    [Display(Name = "zaměstnanecká", ShortName = "ZAM")]
    [EnumMember]
    ZAM = 4
}
