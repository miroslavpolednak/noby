using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

/// <summary>
/// Druhy adres
/// </summary>
[DataContract]
public enum AddressTypes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "Trvalá adresa", ShortName = "PERMANENT")]
    [EnumMember]
    Permanent = 1,

    [Display(Name = "Kontaktní", ShortName = "MAILING")]
    [EnumMember]
    Mailing = 2,

    [Display(Name = "Jiný pobyt", ShortName = "OTHER")]
    [EnumMember]
    Other = 3
}