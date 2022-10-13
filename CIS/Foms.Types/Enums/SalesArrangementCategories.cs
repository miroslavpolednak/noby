using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum SalesArrangementCategories : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "Produktová žádost")]
    ProductRequest = 1,
        
    [EnumMember]
    [Display(Name = "Servisní žádost")]
    ServiceRequest = 2,
}