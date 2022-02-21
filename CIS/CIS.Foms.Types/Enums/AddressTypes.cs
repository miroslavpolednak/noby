﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum AddressTypes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "Trvalá adresa")]
    [EnumMember]
    PERMANENT = 1,

    [Display(Name = "Korespondenční")]
    [EnumMember]
    MAILING = 2,

    [Display(Name = "Adresa v zahraničí pro daňové nerezidenty")]
    [EnumMember]
    ABROAD = 3
}