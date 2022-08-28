﻿using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using System.Diagnostics;
using DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;
using NaturalPerson = DomainServices.CustomerService.Contracts.NaturalPerson;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetCustomerDetailHandler : IRequestHandler<GetCustomerDetailMediatrRequest, Contracts.CustomerResponse>
    {
        private readonly ILogger<GetCustomerDetailHandler> _logger;
        private readonly CustomerManagement.ICMClient _cm;
        private readonly ICodebookServiceAbstraction _codebooks;
        private readonly ICustomerManagementClient _customerManagement;

        public GetCustomerDetailHandler(ILogger<GetCustomerDetailHandler> logger, CustomerManagement.ICMClient cm, ICodebookServiceAbstraction codebooks, ICustomerManagementClient customerManagement)
        {
            _logger = logger;
            _cm = cm;
            _codebooks = codebooks;
            _customerManagement = customerManagement;
        }

        public async Task<CustomerResponse> Handle(GetCustomerDetailMediatrRequest request, CancellationToken cancellationToken)
        {
            return new CustomerResponse();

            _logger.LogInformation("Get detail instance ID #{id}", request.Request);

            // zavolat CM
            var cmResponse = (
                await _cm.GetDetail(
                    request.Request.Identity.IdentityId, Activity.Current?.TraceId.ToHexString() ?? "", 
                    cancellationToken)
                ).CheckCMResult<CustomerManagement.CMWrapper.CustomerBaseInfo>();

            var response = new CustomerResponse();

            // ciselniky
            var docTypes = await _codebooks.IdentificationDocumentTypes();
            var countries = await _codebooks.Countries();
            var genders = await _codebooks.Genders();
            var maritals = await _codebooks.MaritalStatuses();
            var titles = await _codebooks.AcademicDegreesBefore();
            var educations = await _codebooks.EducationLevels();

            // identity
            response.Identities.Add(cmResponse.CustomerId.ToIdentity());

            // FO
            var np = (CustomerManagement.CMWrapper.NaturalPerson)cmResponse.Party;

            // customer
            response.NaturalPerson = new NaturalPerson
            {
                BirthNumber = np.CzechBirthNumber ?? "",
                DateOfBirth = np.BirthDate,
                FirstName = np.FirstName ?? "",
                LastName = np.Surname ?? "",
                GenderId = genders.First(t => t.KbCmCode == np.GenderCode.ToString()).Id,
                BirthName = np.BirthName ?? "",
                PlaceOfBirth = np.BirthPlace ?? "",
                BirthCountryId = countries.FirstOrDefault(t => t.ShortName == np.BirthCountryCode)?.Id,
                MaritalStatusStateId = maritals.FirstOrDefault(t => t.RdmMaritalStatusCode == np.MaritalStatusCode)?.Id ?? 0,
                DegreeBeforeId = titles.FirstOrDefault(t => String.Equals(t.Name, np.Title, StringComparison.InvariantCultureIgnoreCase))?.Id,
                EducationLevelId = educations.FirstOrDefault(t => t.RDMCode.Equals(cmResponse.Kyc?.NaturalPersonKyc?.EducationCode ?? "", StringComparison.InvariantCultureIgnoreCase))?.Id ?? 0,
                IsPoliticallyExposed = cmResponse.IsPoliticallyExposed
            };

            // 
            if (np.CitizenshipCodes != null && np.CitizenshipCodes.Any())
                response.NaturalPerson.CitizenshipCountriesId.AddRange(countries.Where(t => np.CitizenshipCodes.Contains(t.ShortName)).Select(t => t.Id));

            // doklad
            if (cmResponse.PrimaryIdentificationDocument != null)
                response.IdentificationDocument = cmResponse.PrimaryIdentificationDocument.ToIdentificationDocument(countries, docTypes);

            // adresa
            if (cmResponse.PrimaryAddress?.Address != null)
                response.Addresses.Add(cmResponse.PrimaryAddress.Address.ToAddress(cmResponse.PrimaryAddress.ComponentAddress, CIS.Foms.Enums.AddressTypes.PERMANENT, true, countries));
            if (cmResponse.ContactAddress?.Address != null)
                response.Addresses.Add(cmResponse.ContactAddress.Address.ToAddress(cmResponse.ContactAddress.ComponentAddress, CIS.Foms.Enums.AddressTypes.MAILING, true, countries));

            // kontakty - mobil
            if (cmResponse.PrimaryPhone != null)
                response.Contacts.Add(new Contact { ContactTypeId = (int)CIS.Foms.Enums.ContactTypes.MobilPrivate, Value = cmResponse.PrimaryPhone.PhoneNumber, IsPrimary = true });
            // email
            if (cmResponse.PrimaryEmail != null)
                response.Contacts.Add(new Contact { ContactTypeId = (int)CIS.Foms.Enums.ContactTypes.Email, Value = cmResponse.PrimaryEmail.EmailAddress, IsPrimary = true });

            return response;
        }
    }
}
