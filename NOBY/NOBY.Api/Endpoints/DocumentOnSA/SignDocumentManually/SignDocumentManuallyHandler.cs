using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Api.Endpoints.Customer;
using NOBY.Api.Endpoints.Customer.GetDetailWithChanges;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using NOBY.Api.SharedDto;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace NOBY.Api.Endpoints.DocumentOnSA.SignDocumentManually;

internal class SignDocumentManuallyHandler : IRequestHandler<SignDocumentManuallyRequest>
{
    private readonly IDocumentOnSAServiceClient _documentOnSaClient;
    private readonly ISalesArrangementServiceClient _salesArrangementServiceClient;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly ICodebookServiceClients _codebookServiceClients;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ICustomerOnSAServiceClient _customerOnSAServiceClient;
    private readonly ICustomerServiceClient _customerServiceClient;
    private readonly CustomerWithChangedDataService _changedDataService;

    public SignDocumentManuallyHandler(
        IDocumentOnSAServiceClient documentOnSaClient,
        ISalesArrangementServiceClient salesArrangementServiceClient,
        ISalesArrangementServiceClient arrangementServiceClient,
        ICodebookServiceClients codebookServiceClients,
        IHouseholdServiceClient householdClient,
        ICustomerOnSAServiceClient customerOnSAServiceClient,
        ICustomerServiceClient customerServiceClient,
        CustomerWithChangedDataService changedDataService)
    {
        _documentOnSaClient = documentOnSaClient;
        _salesArrangementServiceClient = salesArrangementServiceClient;
        _arrangementServiceClient = arrangementServiceClient;
        _codebookServiceClients = codebookServiceClients;
        _householdClient = householdClient;
        _customerOnSAServiceClient = customerOnSAServiceClient;
        _customerServiceClient = customerServiceClient;
        _changedDataService = changedDataService;
    }

    public async Task<Unit> Handle(SignDocumentManuallyRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSaClient.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        if (!documentOnSas.DocumentsOnSAToSign.Any(d => d.DocumentOnSAId == request.DocumentOnSAId))
        {
            throw new CisNotFoundException(ErrorCodes.DocumentOnSaNotExistForSalesArrangement, $"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");
        }

        var documentOnSa = documentOnSas.DocumentsOnSAToSign.Single(r => r.DocumentOnSAId == request.DocumentOnSAId);

        // CheckForm
        await ValidateSalesArrangement(request, cancellationToken);

        await _documentOnSaClient.SignDocumentManually(request.DocumentOnSAId, cancellationToken);

        if (documentOnSa.HouseholdId is null)
        {
            return Unit.Value;
        }

        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        if (salesArrangement is null)
        {
            throw new CisNotFoundException(19000, $"SalesArrangement{request.SalesArrangementId} does not exist.");
        }

        var salesArrangementType = await GetSalesArrangementType(salesArrangement, cancellationToken);

        if (salesArrangementType.SalesArrangementCategory == (int)SalesArrangementCategories.ServiceRequest)
        {
            throw new CisValidationException(90002, "Mp products not supported");
        }
        else if (salesArrangementType.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest)
        {
            // locally stored changes
            var customersOnSa = await GetCustomersOnSa(documentOnSa, cancellationToken);
            foreach (var customerOnSa in customersOnSa)
            {
                var detailWithChangedData = await _changedDataService.GetCustomerWithChangedData<GetDetailWithChangesResponse>(customerOnSa, cancellationToken);
                await _customerServiceClient.UpdateCustomer(MapUpdateCustomerRequest(detailWithChangedData), cancellationToken);
                //Throw away locally stored data(update CustomerChangeData with null)
                await _customerOnSAServiceClient.UpdateCustomerDetail(MapUpdateCustomerOnSaRequest(customerOnSa), cancellationToken);
            }
        }
        else
        {
            throw new NotSupportedException($"SalesArrangementCategory {salesArrangementType.SalesArrangementCategory} not supported");
        }

        return Unit.Value;
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

    private static DomainServices.CustomerService.Contracts.UpdateCustomerRequest MapUpdateCustomerRequest(GetDetailWithChangesResponse detailWithChangedData)
    {
        var updateRequest = new DomainServices.CustomerService.Contracts.UpdateCustomerRequest();
        if (detailWithChangedData.NaturalPerson is not null)
            updateRequest.NaturalPerson = MapNaturalPerson(detailWithChangedData.NaturalPerson);
        if (detailWithChangedData.IdentificationDocument is not null)
            updateRequest.IdentificationDocument = MapIdentificationDocument(detailWithChangedData.IdentificationDocument);
        if (detailWithChangedData.Addresses is not null && detailWithChangedData.Addresses.Any())
            updateRequest.Addresses.AddRange(MapAddresses(detailWithChangedData.Addresses));
        if (detailWithChangedData.Contacts is not null && detailWithChangedData.Contacts.Any())
            updateRequest.Contacts.AddRange(MapContacts(detailWithChangedData.Contacts));
        // Mandant not in GetDetailWithChangesResponse
        // CustomerIdentification not in GetDetailWithChangesResponse
        return updateRequest;
    }

    private static IEnumerable<Contact> MapContacts(List<CustomerContact> contacts)
    {
        return contacts.Select(contact => new Contact
        {
            ContactTypeId = contact.ContactTypeId ?? 0,
            Value = contact.Value ?? string.Empty,
            IsPrimary = contact.IsPrimary ?? false,
            Confirmed = contact.Confirmed,
        });
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
            PrimaryAddressFrom = address.PrimaryAddressFrom,
            AddressPointId = address.AddressPointId ?? string.Empty
        });
    }

    private static IdentificationDocument MapIdentificationDocument(IdentificationDocumentFull identificationDocument)
    {
        return new IdentificationDocument
        {
            IdentificationDocumentTypeId = identificationDocument.IdentificationDocumentTypeId,
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

    private async Task<List<CustomerOnSA>> GetCustomersOnSa(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
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

        return customers;
    }

    private async Task ValidateSalesArrangement(SignDocumentManuallyRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _salesArrangementServiceClient.ValidateSalesArrangement(request.SalesArrangementId, cancellationToken);

        if (validationResult is not null)
        {
            var nobyError = validationResult.ValidationMessages?
                                       .Where(t => t.NobyMessageDetail.Severity != ValidationMessageNoby.Types.NobySeverity.None)
                                       .GroupBy(t => t.NobyMessageDetail.Category)
                                       .OrderBy(t => t.Min(x => x.NobyMessageDetail.CategoryOrder))
                                       .Select(t => new
                                       {
                                           CategoryName = t.Key,
                                           ValidationMessages = t.Select(t2 => new
                                           {
                                               Message = t2.NobyMessageDetail.Message,
                                               Parameter = t2.NobyMessageDetail.ParameterName,
                                               Severity = t2.NobyMessageDetail.Severity == ValidationMessageNoby.Types.NobySeverity.Error ? MessageSeverity.Error : MessageSeverity.Warning
                                           }).ToList()
                                       }).ToList();

            if (nobyError is not null && nobyError.Any())
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };

                var errors = nobyError.Select(s => new CisExceptionItem(90009, JsonSerializer.Serialize(s, options)));
                throw new CisValidationException(errors);
            }
        }
    }

    private async Task<SalesArrangementTypeItem> GetSalesArrangementType(DomainServices.SalesArrangementService.Contracts.SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var salesArrangementTypes = await _codebookServiceClients.SalesArrangementTypes(cancellationToken);
        return salesArrangementTypes.Single(r => r.Id == salesArrangement.SalesArrangementTypeId);
    }
}
