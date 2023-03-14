using System.Runtime.Serialization;

namespace CIS.Foms.Types.Enums;

[DataContract]
public enum SalesArrangementTypes : byte
{
    [EnumMember]
    Mortgage = 1,

    [EnumMember]
    Drawing = 6,

    [EnumMember]
    GeneralChange = 7,

    [EnumMember]
    HUBN = 8,

    [EnumMember]
    CustomerChange = 9
}
