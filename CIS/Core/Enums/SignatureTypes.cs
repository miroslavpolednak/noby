using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Core.Enums;

[DataContract]
public enum SignatureTypes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "Papírově")]
    Paper = 1,
        
    [EnumMember]
    [Display(Name = "Biometricky")]
    Biometric = 2,
        
    [EnumMember]
    [Display(Name = "Elektronicky")]
    Electronic = 3
}