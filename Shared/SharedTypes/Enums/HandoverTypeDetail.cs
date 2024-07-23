using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CIS.Core.Attributes;

namespace SharedTypes.Enums;

[DataContract]
public enum HandoverTypeDetail
{
    [EnumMember, Display(Name = "pošta", ShortName = "1")]
    Mail = 1,

    [EnumMember, Display(Name = "email", ShortName = "13")]
    Email = 2,

    [EnumMember, CisDefaultValue, Display(Name = "tisk na pobočce", ShortName = "0")]
    PrintingAtBranch = 3
}