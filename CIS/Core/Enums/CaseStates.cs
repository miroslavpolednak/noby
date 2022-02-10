using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Core.Enums;

[DataContract]
public enum CaseStates : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "Rozpracováno")]
    [Attributes.CisDefaultValue]
    InProcess = 1,
        
    [EnumMember]
    [Display(Name = "Předáno")]
    HandedToSb = 2,
        
    [EnumMember]
    [Display(Name = "Stornováno")]
    Cancelled = 3
}