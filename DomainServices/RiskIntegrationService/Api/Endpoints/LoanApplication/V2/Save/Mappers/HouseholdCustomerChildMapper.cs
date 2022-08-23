using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using CIS.Core;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

internal sealed class HouseholdCustomerChildMapper
{
    public async Task<List<_C4M.LoanApplicationCounterParty>> MapCustomers(List<_V2.LoanApplicationCustomer> customers, bool verification)
    {
        var countries = await _codebookService.Countries(_cancellationToken);
        var customerRoles = await _codebookService.CustomerRoles(_cancellationToken);
        var genders = await _codebookService.Genders(_cancellationToken);
        var maritalStatuses = await _codebookService.MaritalStatuses(_cancellationToken);
        var educations = await _codebookService.EducationLevels(_cancellationToken);
        var housingConditions = await _codebookService.HousingConditions(_cancellationToken);
        var identificationDocuments = await _codebookService.IdentificationDocumentTypes(_cancellationToken);

        return (await customers.SelectAsync(async customer => new _C4M.LoanApplicationCounterParty
        {
            Id = customer.InternalCustomerId,
            CustomerId = new ResourceIdentifier
            {
                Id = customer.PrimaryCustomerId,
                Instance = Helpers.GetResourceInstanceFromMandant(_riskApplicationType.MandantId),
                Domain = "CM",
                Resource = "Customer"
            },
            GroupEmployee = customer.IsGroupEmployee,
            SpecialRelationsWithKB = customer.SpecialRelationsWithKB,
            BirthNumber = customer.BirthNumber,
            RoleCode = Helpers.GetEnumFromString<LoanApplicationCounterPartyRoleCode>(customerRoles.FirstOrDefault(t => t.Id == customer.CustomerRoleId)?.RdmCode),
            Firstname = customer.Firstname,
            Surname = customer.Surname,
            BirthName = customer.BirthName,
            BirthDate = customer.BirthDate,
            BirthPlace = customer.BirthPlace,
            Address = customer.Address is null ? null : new _C4M.PrimaryAddress//TODO zmeni c4m long na string?
            {
                City = customer.Address.City,
                CountryCode = countries.FirstOrDefault(t => t.Id == customer.Address.CountryId)?.ShortName,
                Street = customer.Address.Street,
                /*StreetNumber = customer.Address.LandRegistryNumber,
                PostCode = customer.Address.Postcode,
                HouseNumber = customer.Address.BuildingIdentificationNumber*/
            },
            AcademicTitlePrefix = customer.AcademicTitlePrefix,
            Phone = string.IsNullOrEmpty(customer.MobilePhoneNumber) ? null : new List<PhoneContact>
                {
                    new _C4M.PhoneContact { ContactType = PhoneContactContactType.MOBILE, PhoneNumber = customer.MobilePhoneNumber }
                },
            HasEmail = customer.HasEmail,
            IsPartner = customer.IsPartner,
            ManagementType = "XX",
            Income = await _incomeMapper.MapIncomes(customer.Income, verification),
            Taxpayer = customer.Taxpayer,
            CounterpartyType = "FOO",
            LoanApplicationPersonalDocument = customer.IdentificationDocument is null ? null : new LoanApplicationPersonalDocument
            {
                Id = customer.IdentificationDocument.DocumentNumber,
                IssuedOn = customer.IdentificationDocument.IssuedOn,
                Type = identificationDocuments.FirstOrDefault(t => t.Id == customer.IdentificationDocument.IdentificationDocumentTypeId)?.RdmCode,
                ValidTo = customer.IdentificationDocument.ValidTo
            },
            Gender = Helpers.GetEnumFromString<LoanApplicationCounterPartyGender>(genders.FirstOrDefault(t => t.Id == customer.GenderId)?.KbCmCode),
            MaritalStatus = Helpers.GetEnumFromString<LoanApplicationCounterPartyMaritalStatus>(maritalStatuses.FirstOrDefault(t => t.Id == customer.MaritalStateId)?.RdmMaritalStatusCode),
            HighestEducation = Helpers.GetEnumFromString<LoanApplicationCounterPartyHighestEducation>(educations.FirstOrDefault(t => t.Id == customer.EducationLevelId)?.RdmCode),
            HousingCondition = Helpers.GetEnumFromString<LoanApplicationCounterPartyHousingCondition>(housingConditions.FirstOrDefault(t => t.Id == customer.HousingConditionId)?.Code, LoanApplicationCounterPartyHousingCondition.OW)
        }))
        .ToList();
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly CancellationToken _cancellationToken;
    private readonly _RAT.RiskApplicationTypeItem _riskApplicationType;
    private readonly HouseholdCustomerIncomeChildMapper _incomeMapper;

    public HouseholdCustomerChildMapper(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        _RAT.RiskApplicationTypeItem riskApplicationType,
        CancellationToken cancellationToken)
    {
        _incomeMapper = new HouseholdCustomerIncomeChildMapper(_codebookService!, cancellationToken);
        _riskApplicationType = riskApplicationType;
        _cancellationToken = cancellationToken;
        _codebookService = codebookService;
    }
}
