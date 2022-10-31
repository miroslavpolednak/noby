namespace DomainServices.CodebookService.Contracts.Endpoints.SigningMethodsForNaturalPerson
{
    [DataContract]
    public sealed class SigningMethodsForNaturalPersonItem
    {
        [DataMember(Order = 1)]
        public string Code { get; set; }


        [DataMember(Order = 2)]
        public int Order { get; set; }


        [DataMember(Order = 3)]
        public string Name { get; set; }


        [DataMember(Order = 4)]
        public string Description { get; set; }


        [DataMember(Order = 5)]
        public bool IsValid { get; set; }
    }
}