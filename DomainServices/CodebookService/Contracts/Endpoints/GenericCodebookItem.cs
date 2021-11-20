using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts
{
    [DataContract]
    public sealed class GenericCodebookItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name {  get; set; }
    }
}
