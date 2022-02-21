using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Api.Repositories;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class SearchCustomersHandler : IRequestHandler<SearchCustomersMediatrRequest, SearchCustomersResponse>
    {
        private readonly KonsDbRepository _repository;
        private readonly ILogger<SearchCustomersHandler> _logger;
        private readonly CustomerManagement.ICMClient _cm;
        private readonly ICodebookServiceAbstraction _codebooks;

        public SearchCustomersHandler(KonsDbRepository repository, ILogger<SearchCustomersHandler> logger, CustomerManagement.ICMClient cm, ICodebookServiceAbstraction codebooks)
        {
            _repository = repository;
            _logger = logger;
            _cm = cm;
            _codebooks = codebooks;
        }

        public async Task<SearchCustomersResponse> Handle(SearchCustomersMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get list instance Identities #{id}", string.Join(",", request.Request));

            // ciselniky
            var docTypes = await _codebooks.IdentificationDocumentTypes();
            var countries = await _codebooks.Countries();

            // request pro vyhledavani v CM, zatim podle natural person
            var cmRequest = new CustomerManagement.CMWrapper.SearchCustomerRequest
            {
                CustomerId = request.Request.Identity?.IdentityId,
                FirstName = request.Request.NaturalPerson?.FirstName,
                Name = request.Request.NaturalPerson?.LastName,
                BirthEstablishedDate = request.Request.NaturalPerson?.DateOfBirth
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
                
                cmRequest.IdDocumentTypeCode = docTypes.First(t => t.Id == request.Request.IdentificationDocument.IdentificationDocumentTypeId).RDMCode;
                cmRequest.IdDocumentIssuingCountryCode = countries.First(t => t.Id == request.Request.IdentificationDocument.IssuingCountryId).Code;
                cmRequest.IdDocumentNumber = request.Request.IdentificationDocument.Number;
            }

            // podle kontaktu
            if (request.Request.Contact != null)
            {
                if ((CIS.Foms.Enums.ContactTypes)request.Request.Contact.ContactTypeId == CIS.Foms.Enums.ContactTypes.Email)
                    cmRequest.Email = request.Request.Contact.Value;
                else
                    cmRequest.PhoneNumber = request.Request.Contact.Value;
            }

            // zavolat CM
            var cmResponse = (await _cm.Search(cmRequest)).ToCMResult<CustomerManagement.CMWrapper.CustomerSearchResult>();

            var response = new SearchCustomersResponse();

            // ciselniky
            var genders = await _codebooks.Genders();

            // bez PO
            foreach (var item in cmResponse.ResultRows.Where(t => t.Party is CustomerManagement.CMWrapper.NaturalPersonSearchResult))
            {
                var customer = new SearchCustomerResult({  });

                customer.Identities.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity
                {
                    IdentityId = (int)item.CustomerId,
                    IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemas.Kb
                });

                var np = (CustomerManagement.CMWrapper.NaturalPersonSearchResult)item.Party;

                customer.NaturalPerson = new NaturalPersonBaseData
                {
                    BirthNumber = np.CzechBirthNumber,
                    DateOfBirth = np.BirthDate,
                    FirstName = np.FirstName,
                    LastName = np.Surname,
                    GenderId = genders.First(t => t.RDMCode == np.GenderCode.ToString()).Id
                };

                if (item.PrimaryIdentificationDocument != null)
                {
                    customer.IdentificationDocument.RegisterPlace = item.PrimaryIdentificationDocument.RegisterPlace;
                    customer.IdentificationDocument.ValidTo = item.PrimaryIdentificationDocument.ValidTo;
                    customer.IdentificationDocument.IssuedOn = item.PrimaryIdentificationDocument.IssuedOn;
                    customer.IdentificationDocument.IssuedBy = item.PrimaryIdentificationDocument.IssuedBy;
                    customer.IdentificationDocument.Number = item.PrimaryIdentificationDocument.DocumentNumber;
                    customer.IdentificationDocument.IssuingCountryId = countries.First(t => t.Code == item.PrimaryIdentificationDocument.IssuingCountryCode).Id;
                    customer.IdentificationDocument.IdentificationDocumentTypeId = docTypes.First(t => t.RDMCode == item.PrimaryIdentificationDocument.TypeCode).Id;
                }

                if (item.PrimaryAddress != null)
                {
                    customer.Addresses.Add(new Address
                    {
                        
                    });
                }

                response.Customers.Add(new SearchCustomerResult());
            }

            return response;
        }
    }
}
