﻿using CIS.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum SignatureTypes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "fyzicky")]
    Paper = 1,
        
    /*[EnumMember]
    [Display(Name = "Biometricky")]
    Biometric = 2,*/
        
    [EnumMember]
    [Display(Name = "elektronicky")]
    Electronic = 3
}