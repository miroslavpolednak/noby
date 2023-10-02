using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum Genders : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [Display(Name = "muž")]
    [EnumMember]
    Male = 1,
    
    [Display(Name = "žena")]
    [EnumMember]
    Female = 2,
}