using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum DocumentFileTypes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "pdfa")]
    PdfA = 1,
        
    [EnumMember]
    [Display(Name = "openform")]
    OpenForm = 2,
}