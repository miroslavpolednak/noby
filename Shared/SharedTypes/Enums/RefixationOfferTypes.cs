using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum RefixationOfferTypes : byte
{
    [EnumMember]
    [Display(Name = "Aktuální")]
    Current = 1,

    [EnumMember]
    [Display(Name = "Sdělená")]
    Communicated = 2,

    [EnumMember]
    [Display(Name = "Zákonné oznámení")]
    LegalNotice = 3
}
