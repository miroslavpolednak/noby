using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using _CB = DomainServices.CodebookService.Contracts.Endpoints;
using Google.Protobuf.Reflection;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal static class SaveRequestHouseholdExtensions
{
    public static async Task<List<LoanApplicationHousehold>> ToC4m(this List<_V2.LoanApplicationHousehold> households, _RAT.RiskApplicationTypeItem riskApplicationType, CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService, CancellationToken cancellation)
    {
        var householdTypes = await _codebookService.HouseholdTypes(cancellation);
        var propSettlements = await _codebookService.PropertySettlements(cancellation);
        var countries = await _codebookService.Countries(cancellation);
        var customerRoles = await _codebookService.CustomerRoles(cancellation);
        var genders = await _codebookService.Genders(cancellation);
        var maritalStatuses = await _codebookService.MaritalStatuses(cancellation);
        var educations = await _codebookService.EducationLevels(cancellation);
        var housingConditions = await _codebookService.HousingConditions(cancellation);
        var identificationDocuments = await _codebookService.IdentificationDocumentTypes(cancellation);

        return households.Select(household => new LoanApplicationHousehold
            {
                Id = household.HouseholdId,
                RoleCode = householdTypes.FirstOrDefault(t => t.Id == household.HouseholdTypeId)?.RdmCode,
                ChildrenUnderAnd10 = household.ChildrenUpToTenYearsCount,
                ChildrenOver10 = household.ChildrenOverTenYearsCount,
                HouseholdExpensesSummary = household.Expenses?.ToC4m(),
                SettlementTypeCode = propSettlements.FirstOrDefault(t => t.Id == household.PropertySettlementId)?.Code,
                CounterParty = household.Customers?.ToC4m(riskApplicationType, countries, customerRoles, genders, maritalStatuses, educations, housingConditions, identificationDocuments),
                HouseholdCreditLiabilitiesSummaryOutHomeCompany = null,
                HouseholdInstallmentsSummaryOutHomeCompany = null
            })
            .ToList();
    }

    public static List<LoanApplicationCounterParty> ToC4m(
        this List<_V2.LoanApplicationCustomer> customers, 
        _RAT.RiskApplicationTypeItem riskApplicationType,
        List<_CB.Countries.CountriesItem> countries,
        List<_CB.CustomerRoles.CustomerRoleItem> customerRoles,
        List<_CB.Genders.GenderItem> genders,
        List<_CB.MaritalStatuses.MaritalStatusItem> maritalStatuses,
        List<_CB.EducationLevels.EducationLevelItem> educations,
        List<_CB.HousingConditions.HousingConditionItem> housingConditions,
        List<_CB.IdentificationDocumentTypes.IdentificationDocumentTypesItem> identificationDocuments)
        => customers.Select(customer =>
        {
            if (!FastEnum.TryParse(customerRoles.FirstOrDefault(t => t.Id == customer.CustomerRoleId)?.RdmCode, out LoanApplicationCounterPartyRoleCode customerRole))
                throw new CisValidationException(0, $"Can't cast LoanApplicationCounterPartyRoleCode '{customer.CustomerRoleId}' to C4M enum");
            
            var model = new LoanApplicationCounterParty
            {
                Id = customer.InternalCustomerId,
                CustomerId = new ResourceIdentifier
                {
                    Id = customer.PrimaryCustomerId,
                    Instance = Helpers.GetResourceInstanceFromMandant(riskApplicationType.MandantId),
                    Domain = "CM",
                    Resource = "Customer"
                },
                GroupEmployee = customer.IsGroupEmployee,
                SpecialRelationsWithKB = customer.SpecialRelationsWithKB,
                BirthNumber = customer.BirthNumber,
                RoleCode = customerRole,
                Firstname = customer.Firstname,
                Surname = customer.Surname,
                BirthName = customer.BirthName,
                BirthDate = customer.BirthDate,
                BirthPlace = customer.BirthPlace,
                Address = customer.Address is null ? null : new PrimaryAddress//TODO zmeni c4m long na string?
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
                    new PhoneContact { ContactType = PhoneContactContactType.MOBILE, PhoneNumber = customer.MobilePhoneNumber }
                },
                HasEmail = customer.HasEmail,
                IsPartner = customer.IsPartner,
                ManagementType = "XX",
                Income = null,
                Taxpayer = customer.Taxpayer,
                CounterpartyType = "FOO",
                LoanApplicationPersonalDocument = customer.IdentificationDocument is null ? null : new LoanApplicationPersonalDocument
                {
                    Id = customer.IdentificationDocument.DocumentNumber,
                    IssuedOn = customer.IdentificationDocument.IssuedOn,
                    Type = identificationDocuments.FirstOrDefault(t => t.Id == customer.IdentificationDocument.IdentificationDocumentTypeId)?.RdmCode,
                    ValidTo = customer.IdentificationDocument.ValidTo
                }
            };

            if (customer.GenderId.HasValue)
            {
                if (!FastEnum.TryParse(genders.FirstOrDefault(t => t.Id == customer.GenderId)?.KbCmCode, out LoanApplicationCounterPartyGender gender))
                    throw new CisValidationException(0, $"Can't cast LoanApplicationCounterPartyGender '{customer.GenderId}' to C4M enum");
                model.Gender = gender;
            }
            if (customer.MaritalStateId.HasValue)
            {
                if (!FastEnum.TryParse(maritalStatuses.FirstOrDefault(t => t.Id == customer.MaritalStateId)?.RdmMaritalStatusCode, out LoanApplicationCounterPartyMaritalStatus maritalStatus))
                    throw new CisValidationException(0, $"Can't cast LoanApplicationCounterPartyMaritalStatus '{customer.MaritalStateId}' to C4M enum");
                model.MaritalStatus = maritalStatus;
            }
            if (customer.EducationLevelId.HasValue)
            {
                if (!FastEnum.TryParse(educations.FirstOrDefault(t => t.Id == customer.EducationLevelId)?.RDMCode, out LoanApplicationCounterPartyHighestEducation education))
                    throw new CisValidationException(0, $"Can't cast LoanApplicationCounterPartyHighestEducation '{customer.EducationLevelId}' to C4M enum");
                model.HighestEducation = education;
            }
            if (customer.HousingConditionId.HasValue)
            {
                if (!FastEnum.TryParse(housingConditions.FirstOrDefault(t => t.Id == customer.HousingConditionId)?.Code, out LoanApplicationCounterPartyHousingCondition housingCondition))
                    throw new CisValidationException(0, $"Can't cast LoanApplicationCounterPartyHousingCondition '{customer.HousingConditionId}' to C4M enum");
                model.HousingCondition = housingCondition;
            }
            else
                model.HousingCondition = LoanApplicationCounterPartyHousingCondition.OW;

            return model;
        })
        .ToList();

    public static List<ExpensesSummary> ToC4m(this Contracts.Shared.V1.ExpensesSummary expenses)
        => new List<ExpensesSummary>()
        {
            new() { Amount = expenses.Rent.ToAmount(), Category = ExpensesSummaryCategory.RENT },
            new() { Amount = expenses.Saving.ToAmount(), Category = ExpensesSummaryCategory.SAVINGS },
            new() { Amount = expenses.Insurance.ToAmount(), Category = ExpensesSummaryCategory.INSURANCE },
            new() { Amount = expenses.Other.ToAmount(), Category = ExpensesSummaryCategory.OTHER }
        };
}
