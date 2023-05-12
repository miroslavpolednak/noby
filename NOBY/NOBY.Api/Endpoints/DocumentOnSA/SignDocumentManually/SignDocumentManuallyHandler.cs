using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Customer;
using NOBY.Api.Endpoints.Customer.GetCustomerDetailWithChanges;
using NOBY.Api.Endpoints.SalesArrangement.ValidateSalesArrangement;
using NOBY.Api.SharedDto;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace NOBY.Api.Endpoints.DocumentOnSA.SignDocumentManually;

internal sealed class SignDocumentManuallyHandler : IRequestHandler<SignDocumentManuallyRequest>
{
    private readonly IDocumentOnSAServiceClient _documentOnSaClient;

    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly ICodebookServiceClients _codebookServiceClients;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ICustomerOnSAServiceClient _customerOnSAServiceClient;
    private readonly ICustomerServiceClient _customerServiceClient;
    private readonly CustomerWithChangedDataService _changedDataService;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly IMediator _mediator;

    public SignDocumentManuallyHandler(
        IDocumentOnSAServiceClient documentOnSaClient,
        ISalesArrangementServiceClient arrangementServiceClient,
        ICodebookServiceClients codebookServiceClients,
        IHouseholdServiceClient householdClient,
        ICustomerOnSAServiceClient customerOnSAServiceClient,
        ICustomerServiceClient customerServiceClient,
        CustomerWithChangedDataService changedDataService,
        ICaseServiceClient caseServiceClient,
        IMediator mediator)
    {
        _documentOnSaClient = documentOnSaClient;
        _arrangementServiceClient = arrangementServiceClient;
        _codebookServiceClients = codebookServiceClients;
        _householdClient = householdClient;
        _customerOnSAServiceClient = customerOnSAServiceClient;
        _customerServiceClient = customerServiceClient;
        _changedDataService = changedDataService;
        _caseServiceClient = caseServiceClient;
        _mediator = mediator;
    }

    public async Task Handle(SignDocumentManuallyRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSaClient.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        if (!documentOnSas.DocumentsOnSAToSign.Any(d => d.DocumentOnSAId == request.DocumentOnSAId))
        {
            throw new NobyValidationException($"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");
        }

        var documentOnSa = documentOnSas.DocumentsOnSAToSign.Single(r => r.DocumentOnSAId == request.DocumentOnSAId);

        // CheckForm
        await ValidateSalesArrangement(request, cancellationToken);

        await _documentOnSaClient.SignDocumentManually(request.DocumentOnSAId, cancellationToken);

        if (documentOnSa.HouseholdId is null)
        {
            return;
        }

        var mandantId = await GetMandantId(request, cancellationToken);

        if (mandantId != (int)CIS.Foms.Enums.Mandants.Kb)
        {
            throw new CisValidationException(90002, $"Mp products not supported (mandant {mandantId})");
        }

        var (household, customersOnSa) = await GetCustomersOnSa(documentOnSa, cancellationToken);
        foreach (var customerOnSa in customersOnSa)
        {
            if (!string.IsNullOrWhiteSpace(customerOnSa.CustomerChangeData))
            {
                var (detailWithChangedData, _) = await _changedDataService.GetCustomerWithChangedData<GetCustomerDetailWithChangesResponse>(customerOnSa, cancellationToken);
                await _customerServiceClient.UpdateCustomer(MapUpdateCustomerRequest(detailWithChangedData, mandantId.Value, customerOnSa), cancellationToken);
                //Throw away locally stored data(update CustomerChangeData with null)
                await _customerOnSAServiceClient.UpdateCustomerDetail(MapUpdateCustomerOnSaRequest(customerOnSa), cancellationToken);
            }
        }

        // HFICH-4165
        int flowSwitchId = household.HouseholdTypeId switch
        {
            (int)HouseholdTypes.Main => (int)FlowSwitches.Was3601MainChangedAfterSigning,
            (int)HouseholdTypes.Codebtor => (int)FlowSwitches.Was3602CodebtorChangedAfterSigning,
            _ => throw new NobyValidationException("Unsupported HouseholdType")
        };

        await _arrangementServiceClient.SetFlowSwitches(household.SalesArrangementId, new()
        {
            new() { FlowSwitchId = flowSwitchId, Value = false }
        }, cancellationToken);
    }

    private async Task<int?> GetMandantId(SignDocumentManuallyRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        if (salesArrangement is null)
        {
            throw new CisNotFoundException(19000, $"SalesArrangement{request.SalesArrangementId} does not exist.");
        }

        var caseDetail = await _caseServiceClient.GetCaseDetail(salesArrangement.CaseId, cancellationToken);
        var productTypes = await _codebookServiceClients.ProductTypes(cancellationToken);
        return productTypes.Single(r => r.Id == caseDetail.Data.ProductTypeId).MandantId;
    }

    private static UpdateCustomerDetailRequest MapUpdateCustomerOnSaRequest(CustomerOnSA customerOnSa)
    {
        return new UpdateCustomerDetailRequest
        {
            CustomerOnSAId = customerOnSa.CustomerOnSAId,
            CustomerAdditionalData = customerOnSa.CustomerAdditionalData,
            //Throw away locally stored data(update CustomerChangeData with null)
            CustomerChangeData = null
        };
    }

    private static DomainServices.CustomerService.Contracts.UpdateCustomerRequest MapUpdateCustomerRequest(GetCustomerDetailWithChangesResponse detailWithChangedData, int mandant, CustomerOnSA customerOnSA)
    {
        var updateRequest = new DomainServices.CustomerService.Contracts.UpdateCustomerRequest();
        updateRequest.Mandant = (CIS.Infrastructure.gRPC.CisTypes.Mandants)mandant;
        updateRequest.Identities.AddRange(customerOnSA.CustomerIdentifiers);

        if (detailWithChangedData.NaturalPerson is not null)
            updateRequest.NaturalPerson = MapNaturalPerson(detailWithChangedData.NaturalPerson);
        if (detailWithChangedData.IdentificationDocument is not null)
            updateRequest.IdentificationDocument = MapIdentificationDocument(detailWithChangedData.IdentificationDocument);
        if (detailWithChangedData.Addresses is not null && detailWithChangedData.Addresses.Any())
            updateRequest.Addresses.AddRange(MapAddresses(detailWithChangedData.Addresses));
        if (detailWithChangedData.EmailAddress is not null)
            updateRequest.Contacts.Add(MapEmailContact(detailWithChangedData.EmailAddress));
        if (detailWithChangedData.MobilePhone is not null)
            updateRequest.Contacts.Add(MapPhoneContact(detailWithChangedData.MobilePhone));
        
        if ((customerOnSA.CustomerAdditionalData?.CustomerIdentification?.IdentificationMethodId ?? 0) > 0)
        {
            updateRequest.CustomerIdentification = new CustomerIdentification
            {
                IdentificationMethodId = customerOnSA.CustomerAdditionalData!.CustomerIdentification.IdentificationMethodId!.Value,
                CzechIdentificationNumber = customerOnSA.CustomerAdditionalData.CustomerIdentification.CzechIdentificationNumber
            };
        }
        return updateRequest;
    }

    private static Contact MapEmailContact(EmailAddressConfirmedDto emailAddress)
    {
        return new Contact
        {
            IsPrimary = true,
            IsConfirmed = emailAddress.IsConfirmed,
            ContactTypeId = (int)ContactTypes.Email,
            Email = new EmailAddressItem
            {
                EmailAddress = emailAddress.EmailAddress
            }
        };
    }

    private static Contact MapPhoneContact(PhoneNumberConfirmedDto phoneNumber)
    {
        return new Contact
        {
            IsPrimary = true,
            IsConfirmed = phoneNumber.IsConfirmed,
            ContactTypeId = (int)ContactTypes.Mobil,
            Mobile = new MobilePhoneItem
            {
                PhoneIDC = phoneNumber.PhoneIDC,
                PhoneNumber = phoneNumber.PhoneNumber,
            }
        };
    }

    private static IEnumerable<GrpcAddress> MapAddresses(List<CIS.Foms.Types.Address> addresses)
    {
        return addresses.Select(address => new GrpcAddress
        {
            Street = address.Street ?? string.Empty,
            StreetNumber = address.StreetNumber ?? string.Empty,
            HouseNumber = address.HouseNumber ?? string.Empty,
            Postcode = address.Postcode ?? string.Empty,
            City = address.City ?? string.Empty,
            CountryId = address.CountryId,
            AddressTypeId = address.AddressTypeId,
            EvidenceNumber = address.EvidenceNumber ?? string.Empty,
            IsPrimary = address.IsPrimary,
            DeliveryDetails = address.DeliveryDetails ?? string.Empty,
            CityDistrict = address.CityDistrict ?? string.Empty,
            PragueDistrict = address.PragueDistrict ?? string.Empty,
            CountrySubdivision = address.CountrySubdivision ?? string.Empty,
            AddressPointId = address.AddressPointId ?? string.Empty
        });
    }

    private static IdentificationDocument MapIdentificationDocument(IdentificationDocumentFull identificationDocument)
    {
        return new IdentificationDocument
        {
            IdentificationDocumentTypeId = identificationDocument.IdentificationDocumentTypeId.GetValueOrDefault(),
            IssuingCountryId = identificationDocument.IssuingCountryId,
            Number = identificationDocument.Number ?? string.Empty,
            ValidTo = identificationDocument.ValidTo,
            IssuedOn = identificationDocument.IssuedOn,
            IssuedBy = identificationDocument.IssuedBy ?? string.Empty,
            RegisterPlace = identificationDocument.RegisterPlace ?? string.Empty
        };
    }

    private static NaturalPerson MapNaturalPerson(Customer.Shared.NaturalPerson naturalPerson)
    {
        var result = new NaturalPerson
        {
            FirstName = naturalPerson.FirstName ?? string.Empty,
            LastName = naturalPerson.LastName ?? string.Empty,
            DateOfBirth = naturalPerson.DateOfBirth,
            BirthNumber = naturalPerson.BirthNumber ?? string.Empty,
            GenderId = (int)naturalPerson.Gender,
            BirthName = naturalPerson.BirthName ?? string.Empty,
            PlaceOfBirth = naturalPerson.PlaceOfBirth ?? string.Empty,
            BirthCountryId = naturalPerson.BirthCountryId,
            // CitizenshipCountriesId bottom 
            MaritalStatusStateId = naturalPerson.MaritalStatusId ?? 0,
            DegreeBeforeId = naturalPerson.DegreeBeforeId,
            DegreeAfterId = naturalPerson.DegreeAfterId,
            // IsPoliticallyExposed this prop won't by updated
            EducationLevelId = naturalPerson.EducationLevelId ?? 0,
            // IsBrSubscribed this prop won't by updated 
            // KbRelationshipCode this prop won't by updated
            // Segment this prop won't by updated
            // IsUSPerson this prop won't by updated
            LegalCapacity = naturalPerson.LegalCapacity != null ? new NaturalPersonLegalCapacity
            {
                RestrictionTypeId = naturalPerson.LegalCapacity.RestrictionTypeId,
                RestrictionUntil = naturalPerson.LegalCapacity.RestrictionUntil ?? null
            } : null,
            // TaxResidence bottom 
            ProfessionCategoryId = naturalPerson.ProfessionCategoryId,
            ProfessionId = naturalPerson.ProfessionId,
            NetMonthEarningAmountId = naturalPerson.NetMonthEarningAmountId,
            NetMonthEarningTypeId = naturalPerson.NetMonthEarningTypeId
        };

        result.CitizenshipCountriesId.AddRange(naturalPerson.CitizenshipCountriesId);
        result.TaxResidence = naturalPerson.TaxResidences != null ? new NaturalPersonTaxResidence() : null;
        if (result.TaxResidence is not null)
        {
            result.TaxResidence.ValidFrom = naturalPerson.TaxResidences?.validFrom;
            if (naturalPerson.TaxResidences!.ResidenceCountries is not null && naturalPerson.TaxResidences.ResidenceCountries.Any())
            {
                result.TaxResidence.ResidenceCountries.AddRange(naturalPerson.TaxResidences.ResidenceCountries.Select(r => new NaturalPersonResidenceCountry
                {
                    CountryId = r.CountryId,
                    Tin = r.Tin ?? string.Empty
                }));
            }
        }

        return result;
    }

    private async Task<(DomainServices.HouseholdService.Contracts.Household Household, List<CustomerOnSA> Customers)> GetCustomersOnSa(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var houseHold = await _householdClient.GetHousehold(documentOnSa.HouseholdId!.Value, cancellationToken);

        var customers = new List<CustomerOnSA>();
        if (houseHold.CustomerOnSAId1 is not null)
        {
            customers.Add(await _customerOnSAServiceClient.GetCustomer(houseHold.CustomerOnSAId1.Value, cancellationToken));
        }
        if (houseHold.CustomerOnSAId2 is not null)
        {
            customers.Add(await _customerOnSAServiceClient.GetCustomer(houseHold.CustomerOnSAId2.Value, cancellationToken));
        }

        return (houseHold, customers);
    }

    private async Task ValidateSalesArrangement(SignDocumentManuallyRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _mediator.Send(new ValidateSalesArrangementRequest(request.SalesArrangementId), cancellationToken);

        if (validationResult is not null && validationResult.Categories is not null && validationResult.Categories.Any())
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var errors = validationResult.Categories.Select(s => new CisExceptionItem(90009, JsonSerializer.Serialize(s, options)));
            throw new CisValidationException(errors);
        }
    }
}
