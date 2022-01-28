﻿using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ProductInstanceTypes
{
    [DataContract]
    public enum ProductInstanceTypeCategory
    {
        [DataMember(Order = 1)]
        [EnumMember]
        Unknown = 0,

        [DataMember(Order = 2)]
        [EnumMember]
        BuildingSavings = 1,

        [DataMember(Order = 3)]
        [EnumMember]
        BuildingSavingsLoan = 2,

        [DataMember(Order = 4)]
        [EnumMember]
        Mortgage = 3
    }
}
