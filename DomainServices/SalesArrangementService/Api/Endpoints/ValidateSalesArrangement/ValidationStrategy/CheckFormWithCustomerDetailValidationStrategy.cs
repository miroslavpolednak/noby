using CIS.Core.Attributes;
using SharedTypes.GrpcTypes;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement.ValidationStrategy;

[ScopedService, SelfService]
internal sealed class CheckFormWithCustomerDetailValidationStrategy : ISalesArrangementValidationStrategy
{
    private readonly CheckFormSalesArrangementValidation _checkFormValidation;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICustomerChangeDataMerger _customerChangeDataMerger;
    private readonly ICodebookServiceClient _codebookService;

    public CheckFormWithCustomerDetailValidationStrategy(
        CheckFormSalesArrangementValidation checkFormValidation,
        ICustomerOnSAServiceClient customerOnSAService,
        ICustomerServiceClient customerService,
        ICustomerChangeDataMerger customerChangeDataMerger,
        ICodebookServiceClient codebookService)
    {
        _checkFormValidation = checkFormValidation;
        _customerOnSAService = customerOnSAService;
        _customerService = customerService;
        _customerChangeDataMerger = customerChangeDataMerger;
        _codebookService = codebookService;
    }

    public async Task<ValidateSalesArrangementResponse> Validate(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var customersOnSa = await _customerOnSAService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);
        var customerDetails = await _customerService.GetCustomerList(GetCustomerIdentities(customersOnSa), cancellationToken);

        foreach (var customerOnSA in customersOnSa)
        {
            var kbId = customerOnSA.CustomerIdentifiers.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb).IdentityId;

            var customerOnSaDetail = await _customerOnSAService.GetCustomer(customerOnSA.CustomerOnSAId, cancellationToken);
            var customer = customerDetails.Customers.First(c => c.Identities.Any(i => i.IdentityId == kbId && i.IdentityScheme == Identity.Types.IdentitySchemes.Kb));

            _customerChangeDataMerger.MergeAll(customer, customerOnSaDetail);

            await ValidateIdentificationDetail(customer.NaturalPerson, cancellationToken);
            ValidateAddresses(customer);
            ValidateContacts(customer.Contacts);
            await ValidateIdentificationDocument(customer.IdentificationDocument, customer.CustomerIdentification, cancellationToken);
        }

        return await _checkFormValidation.Validate(salesArrangement, cancellationToken);
    }

    private async Task ValidateIdentificationDetail(NaturalPerson naturalPerson, CancellationToken cancellationToken)
    {
        ValidateSuccessfullyOrThrow(IncomeValidation, "'e' Income");
        ValidateSuccessfullyOrThrow(EmploymentValidation, "'f' Employment");

        var maritalStates = await _codebookService.MaritalStatuses(cancellationToken);
        var educationLevels = await _codebookService.EducationLevels(cancellationToken);

        ValidateSuccessfullyOrThrow(MaritalStatesValidation, "'a.i' MaritalStatusId");
        ValidateSuccessfullyOrThrow(EducationLevelsValidation, "'a.ii' EducationLevelId");

        return;

        bool IncomeValidation() => naturalPerson.NetMonthEarningAmountId.HasValue && naturalPerson.NetMonthEarningTypeId.HasValue;
        bool EmploymentValidation() => naturalPerson.ProfessionCategoryId.HasValue && (naturalPerson.ProfessionCategoryId == 0 || naturalPerson.ProfessionId.HasValue);
        bool MaritalStatesValidation() => maritalStates.Any(m => m.Id != 0 && m.Id == naturalPerson.MaritalStatusStateId);
        bool EducationLevelsValidation() => educationLevels.Any(m => m.Id != 0 && m.Id == naturalPerson.EducationLevelId);
    }

    private static void ValidateAddresses(CustomerDetailResponse customer)
    {
        if (customer.Addresses.Any(a => a.AddressTypeId == (int)AddressTypes.Mailing))
            return;

        if (customer.Addresses.Any(a => a.AddressTypeId == (int)AddressTypes.Permanent && a.CountryId == 16))
            return;

        ValidateSuccessfullyOrThrow(ConfirmedContactsValidation, "'b.i.4' Both contacts are not confirmed");

        return;

        bool ConfirmedContactsValidation() =>
            customer.Contacts.Any(c => c.ContactTypeId == (int)ContactTypes.Mobil && c.Mobile.IsPhoneConfirmed) &&
            customer.Contacts.Any(c => c.ContactTypeId == (int)ContactTypes.Email && c.Email.IsEmailConfirmed);
    }

    private static void ValidateContacts(ICollection<Contact> contacts)
    {
        ValidateSuccessfullyOrThrow(ContactsValidation, "'c' Contacts");

        return;

        bool ContactsValidation() => contacts.Any(c => c.ContactTypeId == (int)ContactTypes.Mobil) && contacts.Any(c => c.ContactTypeId == (int)ContactTypes.Email);
    }

    private async Task ValidateIdentificationDocument(IdentificationDocument? identificationDocument, CustomerIdentification? customerIdentification, CancellationToken cancellationToken)
    {
        ValidateSuccessfullyOrThrow(CustomerIdentificationValidation, "'d.i' CustomerIdentification");

        var identificationDocumentTypes = await _codebookService.IdentificationDocumentTypes(cancellationToken);

        ValidateSuccessfullyOrThrow(IdentificationDocumentTypeValidation, "'d.ii' IdentificationDocumentType");

        return;

        bool CustomerIdentificationValidation() => customerIdentification?.IdentificationDate != null && !string.IsNullOrWhiteSpace(customerIdentification.CzechIdentificationNumber);

        bool IdentificationDocumentTypeValidation() => identificationDocument?.IdentificationDocumentTypeId is not null &&
                                                       identificationDocumentTypes.Any(i => i.Id != 0 && i.Id == identificationDocument!.IdentificationDocumentTypeId);
    }

    private static void ValidateSuccessfullyOrThrow(Func<bool> validation, string validationRule)
    {
        if (validation())
            return;

        throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CustomerValidationFailed, validation);
    }

    private static IEnumerable<Identity> GetCustomerIdentities(IEnumerable<CustomerOnSA> customersOnSa) =>
        customersOnSa.Select(c =>
        {
            return c.CustomerIdentifiers.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb) ??
                   throw new CisValidationException($"CustomerOnSa {c.CustomerOnSAId} does not have KB ID");
        });
}