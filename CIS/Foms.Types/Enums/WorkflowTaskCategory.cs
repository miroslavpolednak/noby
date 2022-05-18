using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CIS.Core.Attributes;

namespace CIS.Foms.Enums;

[DataContract]
public enum WorkflowTaskCategory : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "Dožádání")]
    Request = 1,
        
    [EnumMember]
    [Display(Name = "Cenová výjimka")]
    PriceExemption = 2,
        
    [EnumMember]
    [Display(Name = "Konzultace")]
    Consultation = 3,

    [EnumMember]
    [Display(Name = "Ocenění nemovitosti")]
    RealEstateValuation = 4,

    [EnumMember]
    [Display(Name = "KYC procedura")]
    KycProcedure = 5,

    [EnumMember]
    [Display(Name = "Podpis dokumentů")]
    DocumentsSignature = 6,
}