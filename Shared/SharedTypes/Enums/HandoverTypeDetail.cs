using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CIS.Core.Attributes;

namespace SharedTypes.Enums;

[DataContract]
public enum HandoverTypeDetail
{
    [EnumMember, Display(Name = "pošta")]
    Mail = 1,

    [EnumMember, Display(Name = "email")]
    Email = 2,

    [EnumMember, CisDefaultValue, Display(Name = "tisk na pobočce")]
    PrintingAtBranch = 3
}