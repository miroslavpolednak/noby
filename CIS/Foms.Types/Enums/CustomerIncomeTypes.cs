﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum CustomerIncomeTypes : int
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,

    [EnumMember]
    [Display(Name = "Ze zaměstnání")]
    Employement = 1,

    [EnumMember]
    [Display(Name = "Ostatní")]
    Other = 5,
}
