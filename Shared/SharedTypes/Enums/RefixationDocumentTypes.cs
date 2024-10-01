using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SharedTypes.Enums;

[DataContract]
public enum RefixationDocumentTypes
{
    [EnumMember, Display(Name = "Individuální sdělení")]
    InterestRateNotification = 1,

    [EnumMember, Display(Name = "Dodatek")]
    HedgeAppendix = 2
}