using System.ComponentModel;
using System.Runtime.Serialization;

namespace CIS.Core.Enums;

[DataContract]
public enum IdentitySchemes : byte
{
    [Description("unknown")]
    [EnumMember]
    Unknown = 0,

    [Description("Modrá pyramida")]
    [EnumMember]
    Mp = 1,

    [Description("Komerční banka")]
    [EnumMember]
    Kb = 2
}
