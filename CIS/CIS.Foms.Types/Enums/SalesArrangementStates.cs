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
    [CisDefaultValue]
    InProcess = 1,
        
    [EnumMember]
    [Display(Name = "Předáno")]
    HandedToSb = 2,
        
    [EnumMember]
    [Display(Name = "Stornováno")]
    Cancelled = 3
}