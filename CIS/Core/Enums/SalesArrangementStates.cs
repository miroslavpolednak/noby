using System.ComponentModel;
using System.Runtime.Serialization;

namespace CIS.Core.Enums;

[DataContract]
public enum SalesArrangementStates : byte
{
    [Description("unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Description("Rozpracováno")]
    InProcess = 1,
        
    [EnumMember]
    [Description("Předáno")]
    HandedToSb = 2,
        
    [EnumMember]
    [Description("Stornováno")]
    Cancelled = 3
}