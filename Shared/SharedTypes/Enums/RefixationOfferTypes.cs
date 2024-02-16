using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum RefixationOfferTypes : byte
{
    [EnumMember]
    [Display(Name = "Aktuální")]
    Aktualni = 1,

    [EnumMember]
    [Display(Name = "Sdělená")]
    Sdelena = 2,

    [EnumMember]
    [Display(Name = "Zákonné oznámení")]
    ZakonneOznameni = 3
}
