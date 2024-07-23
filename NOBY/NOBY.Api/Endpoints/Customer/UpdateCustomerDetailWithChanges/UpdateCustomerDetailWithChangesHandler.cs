using CIS.Core.Security;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.HouseholdService.Contracts.Model;
using NOBY.Api.Endpoints.Customer.Shared;
using NOBY.Dto.Customer;
using NOBY.Services.Customer;
using NOBY.Services.SigningHelper;

namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

internal sealed class UpdateCustomerDetailWithChangesHandler : IRequestHandler<UpdateCustomerDetailWithChangesRequest>
{
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISigningHelperService _signingHelperService;
    private readonly CustomerWithChangedDataService _customerChangedDataService;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ICurrentUserAccessor _userAccessor;

    public UpdateCustomerDetailWithChangesHandler(
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerOnSAServiceClient customerOnSAService,
        IUserServiceClient userServiceClient,
        IDocumentOnSAServiceClient documentOnSAService,
        ICurrentUserAccessor userAccessor,
        IHouseholdServiceClient householdService,
        ICustomerServiceClient customerService,
        ICodebookServiceClient codebookService,
        ISigningHelperService signingHelperService,
        CustomerWithChangedDataService customerChangedDataService)
    {
        _documentOnSAService = documentOnSAService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
        _customerOnSAService = customerOnSAService;
        _userServiceClient = userServiceClient;
        _userAccessor = userAccessor;
        _householdService = householdService;
        _customerService = customerService;
        _codebookService = codebookService;
        _signingHelperService = signingHelperService;
        _customerChangedDataService = customerChangedDataService;
    }

    public async Task Handle(UpdateCustomerDetailWithChangesRequest request, CancellationToken cancellationToken)
    {
        await _customerService.ValidateMobilePhone(request.MobilePhone, cancellationToken);
        await _customerService.ValidateEmail(request.EmailAddress, cancellationToken);

        var customerInfo =  await _customerChangedDataService.GetCustomerInfo(request.CustomerOnSAId, cancellationToken);
        var originalModel = CustomerMapper.MapCustomerToResponseDto<UpdateCustomerDetailWithChangesRequest>(customerInfo.CustomerDetail, customerInfo.CustomerOnSA);

        await UpdateBasicCustomerData(request, customerInfo.CustomerOnSA, cancellationToken);

        if (customerInfo.CustomerDetail.CustomerIdentification?.IdentificationMethodId is not 1 and 8)
        {
            var user = await _userServiceClient.GetUser(_userAccessor.User!.Id, cancellationToken);

            request.CustomerIdentification ??= new UpdateCustomerDetailWithChangesRequest.CustomerIdentificationObj
            {
                IdentificationDate = DateTime.Now.Date,
                CzechIdentificationNumber = user.UserInfo.Cin,
                IdentificationMethodId = user.UserInfo.IsInternal ? 1 : 8
            };
        }

        var (delta, deltaResult) = await PrepareDelta(request, originalModel, cancellationToken);

        var updateRequest = new UpdateCustomerDetailRequest
        {
            CustomerOnSAId = customerInfo.CustomerOnSA.CustomerOnSAId,
            CustomerChangeMetadata = createMetadata(originalModel, request, delta, deltaResult),
            CustomerAdditionalData = createAdditionalData(customerInfo.CustomerOnSA, request)
        };

        updateRequest.UpdateCustomerChangeDataObject(delta);

        await _customerOnSAService.UpdateCustomerDetail(updateRequest, cancellationToken);

        // jestlize se na klientovi neco menilo
        if (updateRequest.CustomerChangeMetadata.WasCRSChanged || updateRequest.CustomerChangeMetadata.WereClientDataChanged)
        {
            var wasCRSChanged = updateRequest.CustomerChangeMetadata.WasCRSChanged;

            if (wasCRSChanged)
            {
                //Original delta (before save)
                var previousDelta = customerInfo.CustomerOnSA.GetCustomerChangeDataObject();

                //Do not cancel CRS document if exists and tax residences ware not changed again in this request (They were changed in previous one).
                wasCRSChanged = request.IsUSPerson != customerInfo.CustomerOnSA.CustomerAdditionalData?.IsUSPerson || !ModelComparers.AreObjectsEqual(delta?.NaturalPerson?.TaxResidences, previousDelta?.NaturalPerson?.TaxResidences);
            }

            await cancelSigning(
                customerInfo.CustomerOnSA,
                updateRequest.CustomerChangeMetadata.WereClientDataChanged,
                wasCRSChanged,
                cancellationToken);
        }

        await _documentOnSAService.RefreshSalesArrangementState(customerInfo.CustomerOnSA.SalesArrangementId, cancellationToken);
    }

    // ----- update zakladnich udaju nasi instance customera
    // pokud se zmenili zakladni udaje jako jmeno, prijmeni, tak je potreba tuto zmenu
    // propsat take na CustomerOnSA (jedna se o props primo na entite, nikoliv v JSON datech) a Case
    private async Task UpdateBasicCustomerData(UpdateCustomerDetailWithChangesRequest request, CustomerOnSA customerOnSa, CancellationToken cancellationToken)
    {
        if (isStoredModelDifferentToRequest(customerOnSa, request))
        {
            var updateBaseRequest = new UpdateCustomerRequest
            {
                CustomerOnSAId = customerOnSa.CustomerOnSAId,
                Customer = new CustomerOnSABase
                {
                    MaritalStatusId = request.NaturalPerson?.MaritalStatusId,
                    Name = request.NaturalPerson?.LastName ?? "",
                    FirstNameNaturalPerson = request.NaturalPerson?.FirstName ?? "",
                    DateOfBirthNaturalPerson = request.NaturalPerson?.DateOfBirth,
                    LockedIncomeDateTime = customerOnSa.LockedIncomeDateTime
                }
            };
            if (customerOnSa.CustomerIdentifiers is not null)
                updateBaseRequest.Customer.CustomerIdentifiers.AddRange(customerOnSa.CustomerIdentifiers);

            await _customerOnSAService.UpdateCustomer(updateBaseRequest, cancellationToken);

            // update na CASE, pokud se jedna o hlavniho dluznika
            if (customerOnSa.CustomerRoleId == (int)SharedTypes.Enums.EnumCustomerRoles.Debtor)
            {
                var caseId = (await _salesArrangementService.GetSalesArrangement(customerOnSa.SalesArrangementId, cancellationToken)).CaseId;

                await _caseService.UpdateCustomerData(caseId, new DomainServices.CaseService.Contracts.CustomerData
                {
                    DateOfBirthNaturalPerson = request.NaturalPerson?.DateOfBirth,
                    FirstNameNaturalPerson = request.NaturalPerson?.FirstName ?? "",
                    Name = request.NaturalPerson?.LastName ?? "",
                    Identity = customerOnSa.CustomerIdentifiers?[0]
                }, cancellationToken);
            }
        }
    }

    private async Task<(CustomerChangeData? delta, DeltaComparerResult result)> PrepareDelta(UpdateCustomerDetailWithChangesRequest request, UpdateCustomerDetailWithChangesRequest originalModel, CancellationToken cancellationToken)
    {
        request.Addresses?.RemoveAll(address => address.AddressTypeId == (int)AddressTypes.Other);
        RemoveContactAddressIfNotConfirmedAndContactsAreConfirmed(originalModel, request);
        await RemoveTinMissingReasonIfTinIsNotRequired(originalModel.NaturalPerson?.TaxResidences, cancellationToken);

        // ----- update naseho detailu instance customera
        // updatujeme CustomerChangeData a CustomerAdditionalData na nasi entite CustomerOnSA
        var (delta, result) = CreateDelta(request, originalModel);

        //Update SingleLine address if address was changed
        if (delta?.Addresses is not null)
        {
            foreach (var requestAddress in request.Addresses!)
            {
                requestAddress.SingleLineAddressPoint = await _customerService.FormatAddress(requestAddress!, cancellationToken);
            }
        }

        return (delta, result);
    }

    private async Task cancelSigning(
        CustomerOnSA customerOnSA,
        bool wereClientDataChanged,
        bool wasCRSChanged,
        CancellationToken cancellationToken)
    {
        var documentsToSign = (await _documentOnSAService.GetDocumentsToSignList(customerOnSA.SalesArrangementId, cancellationToken))
            .DocumentsOnSAToSign
            .Where(t => t.DocumentOnSAId.HasValue);
        var usedDocumentIds = new List<int>();

        if (wereClientDataChanged) // zmena klientskych udaju
        {
            var household = (await _householdService.GetHouseholdList(customerOnSA.SalesArrangementId, cancellationToken))
                .First(t => t.CustomerOnSAId1 == customerOnSA.CustomerOnSAId || t.CustomerOnSAId2 == customerOnSA.CustomerOnSAId);

            foreach (var doc in documentsToSign.Where(t => t.HouseholdId == household.HouseholdId))
            {
                await _signingHelperService.StopSinningAccordingState(new()
                {
                    DocumentOnSAId = doc.DocumentOnSAId!.Value,
                    SignatureTypeId = doc.SignatureTypeId,
                    SalesArrangementId = doc.SalesArrangementId
                }, cancellationToken);

                usedDocumentIds.Add(doc.DocumentOnSAId.Value);
            }

            // set flow switches
            await _salesArrangementService.SetFlowSwitch(customerOnSA.SalesArrangementId, (household.HouseholdTypeId == (int)HouseholdTypes.Main ? FlowSwitches.Was3601MainChangedAfterSigning : FlowSwitches.Was3602CodebtorChangedAfterSigning), true, cancellationToken);
        }

        if (wasCRSChanged) // zmena CRS
        {
            var crsDoc = documentsToSign.FirstOrDefault(t => t.DocumentTypeId == 13 && t.CustomerOnSA.CustomerOnSAId == customerOnSA.CustomerOnSAId && !usedDocumentIds.Contains(t.DocumentOnSAId!.Value));//HH rikal, ze 14 neni spravne, ze to ma byt 13
            if (crsDoc != null)
            {
                await _signingHelperService.StopSinningAccordingState(new()
                {
                    DocumentOnSAId = crsDoc.DocumentOnSAId!.Value,
                    SignatureTypeId = crsDoc.SignatureTypeId,
                    SalesArrangementId = crsDoc.SalesArrangementId
                }, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Vraci true, pokud se zmenily zakladni udaje CustomerOnSA instance 
    /// (tj. pole, ktera jsou primo na entite, nikoliv v JSON datech)
    /// </summary>
    private static bool isStoredModelDifferentToRequest(CustomerOnSA customerOnSA, UpdateCustomerDetailWithChangesRequest request)
    {
        return !customerOnSA.Name.Equals(request.NaturalPerson?.LastName, StringComparison.Ordinal)
            || !customerOnSA.FirstNameNaturalPerson.Equals(request.NaturalPerson?.FirstName, StringComparison.Ordinal)
            || !customerOnSA.DateOfBirthNaturalPerson.Equals(request.NaturalPerson?.DateOfBirth)
            || customerOnSA.MaritalStatusId != request.NaturalPerson?.MaritalStatusId;
    }

    /// <summary>
    /// Vytvori metadata CustomerOnSA
    /// </summary>
    private static CustomerChangeMetadata createMetadata(UpdateCustomerDetailWithChangesRequest? originalModel, UpdateCustomerDetailWithChangesRequest request,
        CustomerChangeData? delta, DeltaComparerResult deltaResult)
    {
        var metadata = new CustomerChangeMetadata
        {
            WasCRSChanged = (request.IsUSPerson ?? false) ||
                            !ModelComparers.AreObjectsEqual(request.NaturalPerson?.TaxResidences, originalModel?.NaturalPerson?.TaxResidences)
        };

        if (metadata.WasCRSChanged && request.NaturalPerson?.TaxResidences?.ResidenceCountries?.Count > 8)
            throw new NobyValidationException(90042);

        if (delta is not null && deltaResult.ClientDataWereChanged)
        {
            metadata.WereClientDataChanged = true;
        }

        return metadata;
    }

    /// <summary>
    /// Vytvori JSON objekt, ktery obsahuje rozdil (deltu) mezi tim, co prislo v requestu a tim, co mame aktualne ulozene v CustomerOnSA a KB CM.
    /// </summary>
    private static (CustomerChangeData? delta, DeltaComparerResult result) CreateDelta(UpdateCustomerDetailWithChangesRequest request, UpdateCustomerDetailWithChangesRequest? originalModel)
    {
        var requestCustomerChangeData = CustomerMapper.MapCustomerDtoToChangeData(request);
        var originalCustomerChangeData = CustomerMapper.MapCustomerDtoToChangeData(originalModel);

        var delta = new CustomerChangeData();

        var result = ModelComparers.ComparePerson(requestCustomerChangeData?.NaturalPerson, originalCustomerChangeData?.NaturalPerson, delta);

        var hasDifferences = false;
        ModelComparers.CompareObjects(requestCustomerChangeData?.IdentificationDocument, originalCustomerChangeData?.IdentificationDocument, ref hasDifferences, obj => delta.IdentificationDocument = obj);
        ModelComparers.CompareObjects(requestCustomerChangeData?.CustomerIdentification, originalCustomerChangeData?.CustomerIdentification, ref hasDifferences, obj => delta.CustomerIdentification = obj);
        ModelComparers.CompareObjects(requestCustomerChangeData?.Addresses, originalCustomerChangeData?.Addresses, ref hasDifferences, obj => delta.Addresses = obj);

        // tohle je zajimavost - do delty ukladame zmeny jen u kontaktu, ktere nejsou v CM jako IsConfirmed=true
        if (!(originalModel?.EmailAddress?.IsConfirmed ?? false))
            ModelComparers.CompareObjects(requestCustomerChangeData?.EmailAddress, originalCustomerChangeData?.EmailAddress, ref hasDifferences, obj => delta.EmailAddress = obj);

        if (!(originalModel?.MobilePhone?.IsConfirmed ?? false))
            ModelComparers.CompareObjects(requestCustomerChangeData?.MobilePhone, originalCustomerChangeData?.MobilePhone, ref hasDifferences, obj => delta.MobilePhone = obj);

        result.ClientDataWereChanged = result.ClientDataWereChanged || hasDifferences;

        if (result.CrsWasChanged || result.ClientDataWereChanged)
        {
            return (delta, result);
        }

        return (default, result);
    }

    /// <summary>
    /// Vytvori / upravi JSON data v prop CustomerAdditionalData
    /// Data se upravuji na zaklade toho, co prijde v requestu.
    /// </summary>
    private static CustomerAdditionalData createAdditionalData(
        CustomerOnSA customerOnSA,
        UpdateCustomerDetailWithChangesRequest request)
    {
        var additionalData = customerOnSA.CustomerAdditionalData ?? new CustomerAdditionalData();

        // https://jira.kb.cz/browse/HFICH-4200
        // docasne reseni nez se CM rozmysli jak na to
        additionalData.LegalCapacity = new()
        {
            RestrictionTypeId = request.LegalCapacity?.RestrictionTypeId,
            RestrictionUntil = request.LegalCapacity?.RestrictionUntil
        };

        additionalData.HasRelationshipWithCorporate = request.HasRelationshipWithCorporate.GetValueOrDefault();
        additionalData.HasRelationshipWithKB = request.HasRelationshipWithKB.GetValueOrDefault();
        additionalData.HasRelationshipWithKBEmployee = request.HasRelationshipWithKBEmployee.GetValueOrDefault();
        additionalData.IsUSPerson = request.IsUSPerson.GetValueOrDefault();
        additionalData.IsPoliticallyExposed = request.IsPoliticallyExposed.GetValueOrDefault();

        return additionalData;
    }

    private static void RemoveContactAddressIfNotConfirmedAndContactsAreConfirmed(UpdateCustomerDetailWithChangesRequest original, UpdateCustomerDetailWithChangesRequest request)
    {
        if (original.Addresses is null || !original.Addresses.Any(address => address.AddressTypeId == (int)AddressTypes.Mailing && address.IsAddressConfirmed != true))
            return;

        if (original.EmailAddress?.IsConfirmed != true || original.MobilePhone?.IsConfirmed != true)
            return;

        request.Addresses?.RemoveAll(address => address.AddressTypeId == (int)AddressTypes.Mailing);
    }

    private async Task RemoveTinMissingReasonIfTinIsNotRequired(TaxResidenceItem? original, CancellationToken cancellationToken)
    {
        if (original?.ResidenceCountries is null)
            return;

        var countries = await _codebookService.Countries(cancellationToken);
        var tinMissingReasons = await _codebookService.TinNoFillReasonsByCountry(cancellationToken);

        var taxResidencyCountries = original.ResidenceCountries.Select(r => new { Country = countries.FirstOrDefault(c => c.Id == r.CountryId), TaxResidency = r });
        var taxResidencyCountriesWithTinRequiredFalse = taxResidencyCountries.Where(c => !tinMissingReasons.First(t => t.Id == c.Country?.ShortName).IsTinMandatory);

        foreach (var taxResidency in taxResidencyCountriesWithTinRequiredFalse)
        {
            taxResidency.TaxResidency.TinMissingReasonDescription = null;
        }
    }
}
