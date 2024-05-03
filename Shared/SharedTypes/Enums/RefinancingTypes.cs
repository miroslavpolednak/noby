using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum RefinancingTypes : byte
{
    [EnumMember]
    [Display(Name = "Unknown")]
    Unknown = 0,

    [EnumMember]
    [Display(Name = "Dodatek - Retence")]
    MortgageRetention = 1,

    [EnumMember]
    [Display(Name = "Dodatek - Refixace")]
    MortgageRefixation = 2,

    [EnumMember]
    [Display(Name = "Zákonné oznámení")]
    MortgageLegalNotice = 3,

    [EnumMember]
    [Display(Name = "Dokument mimořádné splátky")]
    MortgageExtraPayment = 4
}
