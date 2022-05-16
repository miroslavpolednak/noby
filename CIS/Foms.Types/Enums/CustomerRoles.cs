using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

// Pokud budes menit, upravit handler pro číselník (CustomerRolesHandler)

[DataContract]
public enum CustomerRoles : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [Display(Name = "Dlužník")]
    [EnumMember]
    Debtor = 1,
    
    [Display(Name = "Spoludlužník")]
    [EnumMember]
    Codebtor = 2,
    
    [Display(Name = "Ručitel")]
    [EnumMember]
    Garantor = 128,
}