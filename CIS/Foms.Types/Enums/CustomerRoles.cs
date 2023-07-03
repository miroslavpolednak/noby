using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

// Pokud budes menit, upravit handler pro číselník (CustomerRolesHandler)

/// <summary>
/// Role klienta v obchodu
/// </summary>
[DataContract]
public enum CustomerRoles : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [Display(Name = "Dlužník", ShortName = "A")]
    [EnumMember]
    Debtor = 1,
    
    [Display(Name = "Spoludlužník", ShortName = "S")]
    [EnumMember]
    Codebtor = 2,
    
    [Display(Name = "Ručitel", ShortName = "R")]
    [EnumMember]
    Garantor = 8
}