using CIS.Core.Attributes;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement.ValidationStrategy;

[ScopedService, SelfService]
internal class CheckFormWithCustomerDetailValidationStrategy : ISalesArrangementValidationStrategy
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
            await ValidateIdentificationDocument(customer.CustomerIdentification, cancellationToken);
        }

        return await _checkFormValidation.Validate(salesArrangement, cancellationToken);
    }

    private async Task ValidateIdentificationDetail(NaturalPerson naturalPerson, CancellationToken cancellationToken)
    {
        if (!naturalPerson.NetMonthEarningAmountId.HasValue || 
            !naturalPerson.NetMonthEarningTypeId.HasValue || 
            !naturalPerson.ProfessionCategoryId.HasValue || 
            !naturalPerson.ProfessionId.HasValue)
        {
            ThrowValidationException();
        }

        var maritalStates = await _codebookService.MaritalStatuses(cancellationToken);
        var educationLevels = await _codebookService.EducationLevels(cancellationToken);

        if (!maritalStates.Any(m => m.Id != 0 && m.Id == naturalPerson.MaritalStatusStateId))
            ThrowValidationException();

        if (!educationLevels.Any(m => m.Id != 0 && m.Id == naturalPerson.EducationLevelId))
            ThrowValidationException();
    }

    private static void ValidateAddresses(CustomerDetailResponse customer)
    {
        if (customer.Addresses.Any(a => a.AddressTypeId == (int)AddressTypes.Mailing))
            return;

        if (customer.Contacts.Any(c => c.ContactTypeId == (int)ContactTypes.Mobil && c.Mobile.IsPhoneConfirmed) &&
            customer.Contacts.Any(c => c.ContactTypeId == (int)ContactTypes.Email && c.Email.IsEmailConfirmed))
            return;

        ThrowValidationException();
    }

    private static void ValidateContacts(IEnumerable<Contact> contacts)
    {
        if (contacts.Any(c => c.ContactTypeId == (int)ContactTypes.Mobil))
            return;

        ThrowValidationException();
    }

    private async Task ValidateIdentificationDocument(CustomerIdentification? identification, CancellationToken cancellationToken)
    {
        if (identification is null)
            ThrowValidationException();

        if (identification!.IdentificationDate is not null && !string.IsNullOrWhiteSpace(identification.CzechIdentificationNumber))
            return;

        var identificationDocumentTypes = await _codebookService.IdentificationDocumentTypes(cancellationToken);

        if (identificationDocumentTypes.Any(i => i.Id != 0 && i.Id == identification.IdentificationMethodId))
            return;

        ThrowValidationException();
    }

    private static void ThrowValidationException() =>
        throw new CisValidationException(18087, "Customer validation failed, check customers detail");

    private static IEnumerable<Identity> GetCustomerIdentities(IEnumerable<CustomerOnSA> customersOnSa) =>
        customersOnSa.Select(c =>
        {
            return c.CustomerIdentifiers.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb) ??
                   throw new CisValidationException($"CustomerOnSa {c.CustomerOnSAId} does not have KB ID");
        });
}