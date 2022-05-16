
namespace DomainServices.CodebookService.Contracts.Endpoints.IdentificationDocumentTypes
{
    [DataContract]
    public class IdentificationDocumentTypesItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string ShortName { get; set; }

        [DataMember(Order = 4)]
        public string RDMCode { get; set; }

        [DataMember(Order = 5)]
        public bool IsDefault { get; set; }
    }
}
