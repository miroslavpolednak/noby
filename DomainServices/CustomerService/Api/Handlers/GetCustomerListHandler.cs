using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using System.Diagnostics;

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
            var cmResponse = (
                await _cm.GetList(
                    request.Request.Identities.Select(t => t.IdentityId), Activity.Current?.TraceId.ToHexString() ?? "", 
                    cancellationToken)
                ).CheckCMResult<IEnumerable<CustomerManagement.CMWrapper.CustomerBaseInfo>>();

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
                    BirthNumber = np.CzechBirthNumber ?? "",
                    DateOfBirth = np.BirthDate,
                    FirstName = np.FirstName ?? "",
                    LastName = np.Surname ?? "",
                    GenderId = genders.First(t => t.KbCmCode == np.GenderCode.ToString()).Id
                };

                // doklad
                if (item.PrimaryIdentificationDocument != null)
                    customer.IdentificationDocument = item.PrimaryIdentificationDocument.ToIdentificationDocument(countries, docTypes);

                // adresa
                if (item.PrimaryAddress?.Address != null)
                    customer.Addresses.Add(item.PrimaryAddress.Address.ToAddress(item.PrimaryAddress.ComponentAddress, CIS.Foms.Enums.AddressTypes.PERMANENT, true, countries));
                if (item.ContactAddress?.Address != null)
                    customer.Addresses.Add(item.ContactAddress.Address.ToAddress(item.ContactAddress.ComponentAddress, CIS.Foms.Enums.AddressTypes.MAILING, true, countries));

                // kontakty - mobil
                if (item.PrimaryPhone != null)
                    customer.Contacts.Add(new Contact { ContactTypeId = (int)CIS.Foms.Enums.ContactTypes.MobilPrivate, Value = item.PrimaryPhone.PhoneNumber, IsPrimary = true });
                // email
                if (item.PrimaryEmail != null)
                    customer.Contacts.Add(new Contact { ContactTypeId = (int)CIS.Foms.Enums.ContactTypes.Email, Value = item.PrimaryEmail.EmailAddress, IsPrimary = true });

                response.Customers.Add(customer);
            }

            return response;
        }
    }
}
