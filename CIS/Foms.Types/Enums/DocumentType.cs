using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum DocumentType : byte
{
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    NABIDKA = 1,
        
    [EnumMember]
    KALKULHU = 2,
        
    [EnumMember]
    SPLKALHU = 3,

    [EnumMember]
    ZADOSTHU = 4,

    [EnumMember]
    ZADOSTHD = 5,

    [EnumMember]
    ZADOCERP = 6,

    [EnumMember]
    SDELUCET = 7,

    [EnumMember]
    ZAOZMPAR = 8,

    [EnumMember]
    ZAOZMDLU = 9,

    [EnumMember]
    ZAODHUBN = 10,

    [EnumMember]
    ZADOOPCI = 11,
}
