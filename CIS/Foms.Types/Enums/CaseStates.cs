using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CIS.Core.Attributes;

namespace CIS.Foms.Enums;

/// <summary>
/// Stavy Case
/// </summary>
[DataContract]
public enum CaseStates : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "Příprava žádosti")]
    [CisDefaultValue]
    InProgress = 1,
        
    [EnumMember]
    [Display(Name = "Zpracování žádosti v továrně")]
    InApproval = 2,
        
    [EnumMember]
    [Display(Name = "Podepisování")]
    InSigning = 3,

    [EnumMember]
    [Display(Name = "Čerpání")]
    InDisbursement = 4,

    [EnumMember]
    [Display(Name = "Správa")]
    InAdministration = 5,

    [EnumMember]
    [Display(Name = "Ukončeno")]
    Finished = 6,

    [EnumMember]
    [Display(Name = "Zrušeno")]
    Cancelled = 7,

    [EnumMember]
    [Display(Name = "Zpracování žádosti")]
    InApprovalConfirmed = 8,

    [EnumMember]
    [Display(Name = "Žádost předána ke stornu")]
    ToBeCancelled = 9,

    [EnumMember]
    [Display(Name = "Zpracování storna žádosti")]
    ToBeCancelledConfirmed = 10,
}