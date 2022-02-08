using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.MandantTypes
{
    [DataContract]
    public class MandantTypesItem
    {
        [DataMember(Order = 1)]
        public string Code { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public int StarbuildId { get; set; }
    }
}
