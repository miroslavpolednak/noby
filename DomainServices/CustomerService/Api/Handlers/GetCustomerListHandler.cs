using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetCustomerListHandler : IRequestHandler<GetCustomerListMediatrRequest, Contracts.CustomerListResponse>
    {
        private readonly ILogger<GetCustomerListHandler> _logger;
        private readonly CustomerManagement.ICMClient _cm;
        private readonly ICodebookServiceAbstraction _codebooks;

        public GetCustomerListHandler(ILogger<GetCustomerListHandler> logger, CustomerManagement.ICMClient cm, ICodebookServiceAbstraction codebooks)
        {
            _logger = logger;
            _cm = cm;
            _codebooks = codebooks;
        }

        public async Task<CustomerListResponse> Handle(GetCustomerListMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get list instance Identities #{id}", string.Join(",", request.Request.Identities));

            // zavolat CM
            var cmResponse = (await _cm.GetList(request.Request.Identities.Select(t => (long)t.IdentityId))).CheckCMResult<IEnumerable<CustomerManagement.CMWrapper.CustomerBaseInfo>>();

            var response = new CustomerListResponse();

            // ciselniky
            var docTypes = await _codebooks.IdentificationDocumentTypes();
            var countries = await _codebooks.Countries();
            var genders = await _codebooks.Genders();

            // jen FO
            foreach (var item in cmResponse.Where(t => t.Party is CustomerManagement.CMWrapper.NaturalPerson))
            {
                var customer = new CustomerListResult();

                // identity
                customer.Identities.Add(item.CustomerId.ToIdentity());

                // FO
                var np = (CustomerManagement.CMWrapper.NaturalPerson)item.Party;

                // customer
                customer.NaturalPerson = new NaturalPersonBaseData
                {
                    BirthNumber = np.CzechBirthNumber.ToEmptyString(),
                    DateOfBirth = np.BirthDate,
                    FirstName = np.FirstName.ToEmptyString(),
                    LastName = np.Surname.ToEmptyString(),
                    GenderId = genders.First(t => t.RDMCode == np.GenderCode.ToString()).Id
                };

                // doklad
                if (item.PrimaryIdentificationDocument != null)
                    customer.IdentificationDocument = item.PrimaryIdentificationDocument.ToIdentificationDocument(countries, docTypes);

                // adresa
                if (item.PrimaryAddress?.Address != null)
                    customer.Addresses.Add(item.PrimaryAddress.Address.ToAddress(countries));
                if (item.ContactAddress?.Address != null)
                    customer.Addresses.Add(item.ContactAddress.Address.ToAddress(countries));

                // kontakty - mobil
                if (item.PrimaryPhoneConfirmed != null)
                    customer.Contacts.Add(new Contact { ContactTypeId = (int)CIS.Foms.Enums.ContactTypes.MobilPrivate, Value = item.PrimaryPhoneConfirmed.PhoneNumber, IsPrimary = true });
                else if (item.PrimaryPhone != null)
                    customer.Contacts.Add(new Contact { ContactTypeId = (int)CIS.Foms.Enums.ContactTypes.MobilPrivate, Value = item.PrimaryPhone.PhoneNumber, IsPrimary = true });
                // email
                if (item.PrimaryEmailConfirmed != null)
                    customer.Contacts.Add(new Contact { ContactTypeId = (int)CIS.Foms.Enums.ContactTypes.Email, Value = item.PrimaryEmailConfirmed.EmailAddress, IsPrimary = true });
                else if (item.PrimaryEmail != null)
                    customer.Contacts.Add(new Contact { ContactTypeId = (int)CIS.Foms.Enums.ContactTypes.Email, Value = item.PrimaryEmail.EmailAddress, IsPrimary = true });

                response.Customers.Add(customer);
            }

            return response;
        }
    }
}
