using CIS.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace CIS.Foms.Enums;

[DataContract]
public enum DrawingTypes : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,

    [Display(Name = "Postupné")]
    [EnumMember]
    [CisStarbuildId(1)]
    Gradual = 1,

    [Display(Name = "Jednorázové")]
    [EnumMember]
    [CisStarbuildId(0)]
    Disposable = 2,
}
