
namespace DomainServices.CodebookService.Contracts.Endpoints.Mandants
{
    [DataContract]
    public class MandantsItem
    {

        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Code { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

    }
}
