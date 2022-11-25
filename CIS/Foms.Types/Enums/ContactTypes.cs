using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

/// <summary>
/// Typy kontaktů
/// </summary>
[DataContract]
public enum ContactTypes : byte
{
    [EnumMember]
    Unknown = 0,

    [EnumMember]
    Mobil = 13,

    [EnumMember]
    Email = 14
}