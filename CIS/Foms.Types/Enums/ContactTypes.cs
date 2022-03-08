using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum ContactTypes : byte
{
    [EnumMember]
    Unknown = 0,

    [EnumMember]
    MobilPrivate = 1,

    [EnumMember]
    MobilWork = 2,

    [EnumMember]
    LandlineHome = 3,

    [EnumMember]
    Email = 5
}