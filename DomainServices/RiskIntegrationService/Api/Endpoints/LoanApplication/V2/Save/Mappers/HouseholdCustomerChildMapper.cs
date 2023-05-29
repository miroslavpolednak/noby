using _C4M = DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V3.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using CIS.Core;
using DomainServices.CodebookService.Contracts.v1;

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
            CustomerId = new _C4M.ResourceIdentifier
            {
                Id = customer.PrimaryCustomerId,
                Instance = Helpers.GetResourceInstanceFromMandant(_riskApplicationType.MandantId),
                Domain = "CM",
                Resource = "Customer"
            }.ToC4M(),
            GroupEmployee = customer.IsGroupEmployee,
            SpecialRelationsWithKB = customer.SpecialRelationsWithKB,
            BirthNumber = customer.BirthNumber,
            RoleCode = Helpers.GetEnumFromString<_C4M.RoleType>(customerRoles.FirstOrDefault(t => t.Id == customer.CustomerRoleId)?.RdmCode),
            Firstname = customer.Firstname,
            Surname = customer.Surname,
            BirthName = customer.BirthName,
            BirthDate = customer.BirthDate,
            BirthPlace = customer.BirthPlace,
            Address = customer.Address is null ? null : new _C4M.PrimaryAddress
            {
                City = customer.Address.City,
                CountryCode = countries.FirstOrDefault(t => t.Id == customer.Address.CountryId)?.ShortName,
                Street = customer.Address.Street,
                HouseNumber = customer.Address.HouseNumber,
                PostalCode = customer.Address.Postcode,
                StreetNumber = customer.Address.StreetNumber,
                RegionCode = "16"
            },
            AcademicTitlePrefix = customer.AcademicTitlePrefix,
            Contacts = string.IsNullOrEmpty(customer.MobilePhoneNumber) ? null : new List<_C4M.Contact> { new _C4M.Contact { ContactCategory = _C4M.ContactCategoryType.MOBILE, ContactType = _C4M.ContactType.PHONE, Value = customer.MobilePhoneNumber } },
            HasEmail = customer.HasEmail,
            IsPartner = customer.IsPartner,
            Income = await _incomeMapper.MapIncomes(customer.Income, verification),
            Taxpayer = customer.Taxpayer,
            CounterpartyType = "FOO",
            LoanApplicationPersonalDocument = customer.IdentificationDocument is null ? null : new _C4M.LoanApplicationPersonalDocument
            {
                Id = customer.IdentificationDocument.DocumentNumber,
                IssuedOn = customer.IdentificationDocument.IssuedOn,
                Type = identificationDocuments.FirstOrDefault(t => t.Id == customer.IdentificationDocument.IdentificationDocumentTypeId)?.RdmCode,
                ValidTo = customer.IdentificationDocument.ValidTo
            },
            Gender = Helpers.GetEnumFromString<_C4M.GenderType>(genders.FirstOrDefault(t => t.Id == customer.GenderId)?.KbCmCode),
            MaritalStatus = Helpers.GetEnumFromString<_C4M.MaritalStatusType>(maritalStatuses.FirstOrDefault(t => t.Id == customer.MaritalStateId)?.RdmCode),
            HighestEducation = educations.FirstOrDefault(t => t.Id == customer.EducationLevelId)?.ScoringCode,
            HousingCondition = Helpers.GetEnumFromString<_C4M.HousingConditionType>(housingConditions.FirstOrDefault(t => t.Id == customer.HousingConditionId)?.Code, _C4M.HousingConditionType.OW)
        }))
        .ToList();
    }

    private static long? getZipCode(string? zip)
    {
        long code;
        return long.TryParse(zip?.Replace(" ", ""), out code) ? code : null;
    }

    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly CancellationToken _cancellationToken;
    private readonly RiskApplicationTypesResponse.Types.RiskApplicationTypeItem _riskApplicationType;
    private readonly HouseholdCustomerIncomeChildMapper _incomeMapper;

    public HouseholdCustomerChildMapper(
        CodebookService.Clients.ICodebookServiceClient codebookService,
        RiskApplicationTypesResponse.Types.RiskApplicationTypeItem riskApplicationType,
        CancellationToken cancellationToken)
    {
        _incomeMapper = new HouseholdCustomerIncomeChildMapper(codebookService, cancellationToken);
        _riskApplicationType = riskApplicationType;
        _cancellationToken = cancellationToken;
        _codebookService = codebookService;
    }
}
