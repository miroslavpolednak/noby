using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum Source : byte
{
    [EnumMember]
    Unknown = 0,
    [EnumMember]
    Noby = 1,
    [EnumMember]
    Workflow = 2
}
