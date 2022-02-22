using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetCustomerDetailHandler : IRequestHandler<GetCustomerDetailMediatrRequest, Contracts.CustomerResponse>
    {
        private readonly ILogger<GetCustomerDetailHandler> _logger;
        private readonly CustomerManagement.ICMClient _cm;
        private readonly ICodebookServiceAbstraction _codebooks;

        public GetCustomerDetailHandler(ILogger<GetCustomerDetailHandler> logger, CustomerManagement.ICMClient cm, ICodebookServiceAbstraction codebooks)
        {
            _logger = logger;
            _cm = cm;
            _codebooks = codebooks;
        }

        public async Task<CustomerResponse> Handle(GetCustomerDetailMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get detail instance ID #{id}", request.Request);

            // zavolat CM
            var cmResponse = (await _cm.GetDetail(request.Request.Identity.IdentityId)).CheckCMResult<CustomerManagement.CMWrapper.CustomerBaseInfo>();

            var response = new CustomerResponse();

            // ciselniky
            var docTypes = await _codebooks.IdentificationDocumentTypes();
            var countries = await _codebooks.Countries();
            var genders = await _codebooks.Genders();
            var maritals = await _codebooks.MaritalStatuses();

            // identity
            response.Identities.Add(cmResponse.CustomerId.ToIdentity());

            // FO
            var np = (CustomerManagement.CMWrapper.NaturalPerson)cmResponse.Party;

            // customer
            response.NaturalPerson = new NaturalPerson
            {
                BirthNumber = np.CzechBirthNumber.ToEmptyString(),
                DateOfBirth = np.BirthDate,
                FirstName = np.FirstName.ToEmptyString(),
                LastName = np.Surname.ToEmptyString(),
                GenderId = genders.First(t => t.RDMCode == np.GenderCode.ToString()).Id,
                BirthName = np.BirthName.ToEmptyString(),
                PlaceOfBirth = np.BirthPlace.ToEmptyString(),
                BirthCountryId = countries.FirstOrDefault(t => t.Code == np.BirthCountryCode)?.Id,
                MaritalStatusStateId = maritals.FirstOrDefault(t => t.RDMCode == np.MaritalStatusCode)?.Id ?? 0,
            };

            if (np.CitizenshipCodes != null && np.CitizenshipCodes.Any())
                response.NaturalPerson.CitizenshipCountriesId.AddRange(countries.Where(t => np.CitizenshipCodes.Contains(t.Code)).Select(t => t.Id));

            // doklad
            if (cmResponse.PrimaryIdentificationDocument != null)
                response.IdentificationDocument = cmResponse.PrimaryIdentificationDocument.ToIdentificationDocument(countries, docTypes);

            return response;
        }
    }
}
