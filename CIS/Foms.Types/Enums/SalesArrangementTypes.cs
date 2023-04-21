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
    CustomerChange = 9,

    // absolutne netusim jak tyhle ptakoviny pojmenovat...
    [EnumMember]
    CustomerChange3602A = 10,

    [EnumMember]
    CustomerChange3602B = 11,

    [EnumMember]
    CustomerChange3602C = 12
}
