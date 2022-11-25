using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

/// <summary>
/// 
/// </summary>
[DataContract]
public enum CustomerProfiles : byte
{
    [Display(Name = "unknown", ShortName = "")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "Subjekt s identifikací", ShortName = "IDENTIFIED_SUBJECT")]
    [EnumMember]
    IdentifiedSubject = 1,

    [Display(Name = "Subjekt s plnou KYC", ShortName = "KYC_SUBJECT")]
    [EnumMember]
    KycSubject = 2,

    [Display(Name = "Subjekt s plnou KYC pro New Digital Bank", ShortName = "NDB_KYC_SUBJECT")]
    [EnumMember]
    NdbKycSubject = 3,

    [Display(Name = "Alerty ZOOM subjekt s identifikací", ShortName = "ZOOM_IDENT_SUBJECT")]
    [EnumMember]
    ZoomIdentSubject = 4,

    [Display(Name = "Alerty ZOOM subjekt s plnou KYC", ShortName = "ZOOM_KYC_SUBJECT")]
    [EnumMember]
    ZoomKycSubject = 5,
}
