using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using System.Diagnostics;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class SearchCustomersHandler : IRequestHandler<SearchCustomersMediatrRequest, SearchCustomersResponse>
    {
        private readonly ILogger<SearchCustomersHandler> _logger;
        private readonly CustomerManagement.ICMClient _cm;
        private readonly ICodebookServiceAbstraction _codebooks;

        public SearchCustomersHandler(ILogger<SearchCustomersHandler> logger, CustomerManagement.ICMClient cm, ICodebookServiceAbstraction codebooks)
        {
            _logger = logger;
            _cm = cm;
            _codebooks = codebooks;
        }

        public async Task<SearchCustomersResponse> Handle(SearchCustomersMediatrRequest request, CancellationToken cancellationToken)
        {
            // ciselniky
            var docTypes = await _codebooks.IdentificationDocumentTypes(cancellationToken);
            var countries = await _codebooks.Countries(cancellationToken);

            // request pro vyhledavani v CM, zatim podle natural person
            var cmRequest = new CustomerManagement.CMWrapper.SearchCustomerRequest
            {
                NumberOfEntries = 20,
                CustomerId = request.Request.Identity?.IdentityId,
                FirstName = request.Request.NaturalPerson?.FirstName.ToCMstring(),
                Name = request.Request.NaturalPerson?.LastName.ToCMstring(),
                BirthEstablishedDate = request.Request.NaturalPerson?.DateOfBirth,
                Email = request.Request.Email.ToCMstring(),
                PhoneNumber = request.Request.PhoneNumber.ToCMstring()
            };

            // podle RC
            if (!string.IsNullOrEmpty(request.Request.NaturalPerson?.BirthNumber))
            {
                cmRequest.IdentifierTypeCode = "CZ_RC";
                cmRequest.IdentifierValue = request.Request.NaturalPerson.BirthNumber;
            }

            // podle dokladu
            if (request.Request.IdentificationDocument != null)
            {                
                cmRequest.IdDocumentTypeCode = docTypes.First(t => t.Id == request.Request.IdentificationDocument.IdentificationDocumentTypeId).RdmCode;
                cmRequest.IdDocumentIssuingCountryCode = countries.First(t => t.Id == request.Request.IdentificationDocument.IssuingCountryId).ShortName;
                cmRequest.IdDocumentNumber = request.Request.IdentificationDocument.Number;
            }

            // zavolat CM
            var cmResponse = (await _cm.Search(cmRequest, Activity.Current?.TraceId.ToHexString() ?? "", cancellationToken)).CheckCMResult<CustomerManagement.CMWrapper.CustomerSearchResult>();

            var response = new SearchCustomersResponse();

            // ciselniky
            var genders = await _codebooks.Genders(cancellationToken);

            // bez PO
            foreach (var item in cmResponse.ResultRows.Where(t => t.Party is CustomerManagement.CMWrapper.NaturalPersonSearchResult))
            {
                SearchCustomerResult customer = new ();

                // identity
                customer.Identities.Add(item.CustomerId.ToIdentity());

                // FO
                var np = (CustomerManagement.CMWrapper.NaturalPersonSearchResult)item.Party;

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
                {
                    customer.Street = item.PrimaryAddress.Address.Street ?? "";
                    customer.City = item.PrimaryAddress.Address.City ?? "";
                    customer.Postcode = item.PrimaryAddress.Address.PostCode ?? "";
                    customer.CountryId = countries.FirstOrDefault(t => t.ShortName == item.PrimaryAddress.Address.CountryCode)?.Id;
                }

                response.Customers.Add(customer);
            }

            return response;
        }
    }
}
