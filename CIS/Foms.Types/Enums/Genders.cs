using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum Genders : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [Display(Name = "Muž")]
    [EnumMember]
    Male = 1,
    
    [Display(Name = "Žena")]
    [EnumMember]
    Female = 2,
}