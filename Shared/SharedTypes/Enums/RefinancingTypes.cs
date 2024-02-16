using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum RefinancingTypes : byte
{
    [EnumMember]
    [Display(Name = "Dodatek - Retence")]
    Retence = 1,

    [EnumMember]
    [Display(Name = "Dodatek - Refixace")]
    Refixace = 2,

    [EnumMember]
    [Display(Name = "Mimořádná splátka")]
    MimoradnaSplatka = 3
}
