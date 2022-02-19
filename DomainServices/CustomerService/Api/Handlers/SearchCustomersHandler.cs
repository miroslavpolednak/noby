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
                var docType = (await _codebooks.IdentificationDocumentTypes()).First(t => t.Id == request.Request.IdentificationDocument.IdentificationDocumentTypeId);
                cmRequest.IdDocumentTypeCode = docType.RDMCode;
                var country = (await _codebooks.Countries()).First(t => t.Id == request.Request.IdentificationDocument.IssuingCountryId);
                cmRequest.IdDocumentIssuingCountryCode = country.Code;
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

            var cmResponse = (await _cm.Search(cmRequest));//.CreateContact(request.Request.Contact.ToMpHomeContactData(), request.Request.Identity)).ToMpHomeResult<MpHome.MpHomeWrapper.ContactIdResponse>();

            
            var response = new SearchCustomersResponse();

            //foreach (var entity in entities)
            //    response.Customers.Add(entity.ToBasicDataResponse());

            return response;
        }
    }
}
