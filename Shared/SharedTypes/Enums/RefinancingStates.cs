using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum RefinancingStates
{
    [Display(Name = "Neznámý")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "Rozpracováno v Noby")]
    [EnumMember]
    RozpracovanoVNoby = 1,

    [Display(Name = "Rozpracováno v SB")]
    [EnumMember]
    RozpracovanoVSB = 2,

    [Display(Name = "Podepisování")]
    [EnumMember]
    Podepisovani = 3,

    [Display(Name = "Dokončeno")]
    [EnumMember]
    Dokonceno = 4,

    [Display(Name = "Předáno RC2")]
    [EnumMember]
    PredanoRC2 = 5,

    [Display(Name = "Zrušeno")]
    [EnumMember]
    Zruseno = 6
}
