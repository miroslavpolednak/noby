using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum HouseholdTypes : byte
{
    [Display(Name = "unknown", ShortName = "")]
    [EnumMember]
    Unknown = 0,
    
    [Display(Name = "Hlavní", ShortName = "A")]
    [EnumMember]
    Main = 1,
    
    [Display(Name = "Spoludlužnická", ShortName = "A")]
    [EnumMember]
    Codebtor = 2,
    
    [Display(Name = "Ručitelská", ShortName = "R")]
    [EnumMember]
    Garantor = 128
}