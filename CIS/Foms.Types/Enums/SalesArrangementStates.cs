using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CIS.Core.Attributes;

namespace CIS.Foms.Enums;

[DataContract]
public enum SalesArrangementStates : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "Rozpracováno")]
    [CisStarbuildId(1)]
    InProgress = 1,
        
    [EnumMember]
    [Display(Name = "Předáno ke zpracování")]
    [CisStarbuildId(1)]
    InApproval = 2,
        
    [EnumMember]
    [Display(Name = "Zrušeno")]
    [CisStarbuildId(2)]
    Cancelled = 3,

    [EnumMember]
    [Display(Name = "Podepsáno")]
    [CisStarbuildId(1)]
    IsSigned = 4,

    [EnumMember]
    [CisDefaultValue]
    [Display(Name = "Nová")]
    [CisStarbuildId(1)]
    NewArrangement = 5
}