﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

/// <summary>
/// Druhy příjmu
/// </summary>
[DataContract]
public enum EnumIncomeTypes : int
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,

    [EnumMember]
    [Display(Name = "Ze zaměstnání")]
    Employement = 1,

    [EnumMember]
    [Display(Name = "Z podnikání")]
    Entrepreneur = 2,

    [EnumMember]
    [Display(Name = "Z pronájmu")]
    Rent = 3,

    [EnumMember]
    [Display(Name = "Ostatní")]
    Other = 4
}
