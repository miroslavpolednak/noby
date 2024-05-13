using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum ExtraPaymentTypes
{
    [EnumMember, Display(Name = "Unknown")]
    Unknown = 0,

    [EnumMember, Display(Name = "Celkové splacení")]
    TotalRepayment = 1,

    [EnumMember, Display(Name = "Částečné splacení")]
    PartialRepayment = 2
}