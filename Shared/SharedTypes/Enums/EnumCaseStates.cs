using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CIS.Core.Attributes;

namespace SharedTypes.Enums;

/// <summary>
/// Stavy Case
/// </summary>
[DataContract]
public enum EnumCaseStates : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "Příprava žádosti")]
    [CisDefaultValue]
    InProgress = 1,
        
    [EnumMember]
    [Display(Name = "Žádost předána ke zpracování")]
    InProcessing = 2,
        
    [EnumMember]
    [Display(Name = "Schváleno / Podepisování s klientem")]
    InSigning = 3,

    [EnumMember]
    [Display(Name = "Smlouva účinná")]
    InDisbursement = 4,

    [EnumMember]
    [Display(Name = "Splácení úvěru")]
    InAdministration = 5,

    [EnumMember]
    [Display(Name = "Ukončeno")]
    Finished = 6,

    [EnumMember]
    [Display(Name = "Zrušeno")]
    Cancelled = 7,

    [EnumMember]
    [Display(Name = "Zpracování žádosti")]
    InProcessingConfirmed = 8,

    [EnumMember]
    [Display(Name = "Žádost předána ke stornu")]
    ToBeCancelled = 9,

    [EnumMember]
    [Display(Name = "Zpracování storna žádosti")]
    ToBeCancelledConfirmed = 10,

    [EnumMember]
    [Display(Name = "Předáno do schvalování")]
    InApproval = 11,

    [EnumMember]
    [Display(Name = "Vráceno zpracovateli")]
    ReturnedForProcessing = 12,

    [EnumMember]
    [Display(Name = "Žádost o čerpání přijata ke zpracování")]
    DrawdownRequested = 13
}