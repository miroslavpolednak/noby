using CIS.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum SignatureTypes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "Fyzicky")]
    Paper = 1,
        
    /*[EnumMember]
    [Display(Name = "Biometricky")]
    Biometric = 2,*/
        
    [EnumMember]
    [CisDefaultValue]
    [Display(Name = "Elektronicky")]
    Electronic = 3
}