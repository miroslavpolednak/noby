using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum HouseholdTypes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [Display(Name = "Hlavní domácnost")]
    [EnumMember]
    Main = 1,
    
    [Display(Name = "Spoludlužnická domácnost")]
    [EnumMember]
    Codebtor = 2,
    
    [Display(Name = "Ručitelská domácnost")]
    [EnumMember]
    Garantor = 3
}