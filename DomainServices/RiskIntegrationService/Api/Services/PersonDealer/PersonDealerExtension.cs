namespace Mpss.Rip.Infrastructure.Services.PersonDealer
{
    public class PersonDealerExtension
    {
        public PersonDealerExtension()
        {
            PersonDealerExists = false;
            IsDealer = false;
        }

        //public KBGroupPerson KBGroupPerson { get; set; }
        //public LoanApplicationDealer LoanApplicationDealer { get; set; }

        public long PersonID { get; set; }
        public string PersonSurname { get; set; }
        public long? PersonOrgUnitId { get; set; }
        public string PersonOrgUnitName { get; set; }
        public string PersonJobPostId { get; set; }
        public int? DealerCompanyId { get; set; }

        public bool PersonDealerExists { get; set; }
        public bool IsDealer { get; set; }
    }
}