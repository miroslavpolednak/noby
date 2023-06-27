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
    [Obsolete("Na confl je to preskrtnute, ale nevim o tom, ze by byl pokyn smazat...")]
    IsSigned = 4,

    [EnumMember]
    [CisDefaultValue]
    [Display(Name = "Nová")]
    [CisStarbuildId(1)]
    NewArrangement = 5,

    [EnumMember]
    [Display(Name = "Vyčerpáno")]
    [CisStarbuildId(1)]
    Disbursed = 6,

    [EnumMember]
    [Display(Name = "Podepisování")]
    [CisStarbuildId(1)]
    InSigning = 7,

    [EnumMember]
    [Display(Name = "K odeslání")]
    [CisStarbuildId(1)]
    ToSend = 8
}