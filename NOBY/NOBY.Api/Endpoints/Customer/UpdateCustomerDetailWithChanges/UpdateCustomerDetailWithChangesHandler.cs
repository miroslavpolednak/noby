using CIS.Core.Security;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients;
using Newtonsoft.Json;
using __Household = DomainServices.HouseholdService.Contracts;
using DomainServices.CaseService.Clients;
using SharedTypes.Enums;
using DomainServices.CustomerService.Clients;
using DomainServices.DocumentOnSAService.Clients;

namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

internal sealed class UpdateCustomerDetailWithChangesHandler
    : IRequestHandler<UpdateCustomerDetailWithChangesRequest>
{
    public async Task Handle(UpdateCustomerDetailWithChangesRequest request, CancellationToken cancellationToken)
    {
        // customer instance
        var customerOnSA = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);
        
        // customer from KB CM
        var (originalModel, customerIdentification) = await _changedDataService.GetCustomerFromCM<UpdateCustomerDetailWithChangesRequest>(customerOnSA, cancellationToken);

        // ----- update zakladnich udaju nasi instance customera
        // pokud se zmenili zakladni udaje jako jmeno, prijmeni, tak je potreba tuto zmenu
        // propsat take na CustomerOnSA (jedna se o props primo na entite, nikoliv v JSON datech) a Case
        if (isStoredModelDifferentToRequest(customerOnSA, request))
        {
            var updateBaseRequest = new __Household.UpdateCustomerRequest
            {
                CustomerOnSAId = customerOnSA.CustomerOnSAId,
                Customer = new __Household.CustomerOnSABase
                {
                    MaritalStatusId = request.NaturalPerson?.MaritalStatusId,
                    Name = request.NaturalPerson?.LastName ?? "",
                    FirstNameNaturalPerson = request.NaturalPerson?.FirstName ?? "",
                    DateOfBirthNaturalPerson = request.NaturalPerson?.DateOfBirth,
                    LockedIncomeDateTime = customerOnSA.LockedIncomeDateTime
                }
            };
            if (customerOnSA.CustomerIdentifiers is not null)
                updateBaseRequest.Customer.CustomerIdentifiers.AddRange(customerOnSA.CustomerIdentifiers);

            //

            await _customerOnSAService.UpdateCustomer(updateBaseRequest, cancellationToken);

            // update na CASE, pokud se jedna o hlavniho dluznika
            if (customerOnSA.CustomerRoleId == (int)CustomerRoles.Debtor)
            {
                var caseId = (await _salesArrangementService.GetSalesArrangement(customerOnSA.SalesArrangementId, cancellationToken)).CaseId;

                await _caseService.UpdateCustomerData(caseId, new DomainServices.CaseService.Contracts.CustomerData
                {
                    DateOfBirthNaturalPerson = request.NaturalPerson?.DateOfBirth,
                    FirstNameNaturalPerson = request.NaturalPerson?.FirstName ?? "",
                    Name = request.NaturalPerson?.LastName ?? "",
                    Identity = customerOnSA.CustomerIdentifiers?[0]
                }, cancellationToken);
            }
        }

        if (customerIdentification != 1 && customerIdentification != 8)
        {
            var user = await _userServiceClient.GetUser(_userAccessor.User!.Id, cancellationToken);

            request.CustomerIdentification ??= new UpdateCustomerDetailWithChangesRequest.CustomerIdentificationObj
            {
                IdentificationDate = DateTime.Now.Date,
                CzechIdentificationNumber = user.UserInfo.Cin,
                IdentificationMethodId = user.UserInfo.IsInternal ? 1 : 8
            };
        }

        // ----- update naseho detailu instance customera
        // updatujeme CustomerChangeData a CustomerAdditionalData na nasi entite CustomerOnSA
        var delta = createDelta(originalModel, request);

        //Update SingleLine address if address was changed
        if (((IDictionary<string, object>)delta).ContainsKey("Addresses"))
        {
            foreach (var requestAddress in request.Addresses!)
            {
                requestAddress.SingleLineAddressPoint = await _customerService.FormatAddress(requestAddress!, cancellationToken);
            }
        }

        var updateRequest = new __Household.UpdateCustomerDetailRequest
        {
            CustomerOnSAId = customerOnSA.CustomerOnSAId,
            CustomerChangeData = createJsonFromDelta(delta),
            CustomerChangeMetadata = createMetadata(originalModel, request, delta),
            CustomerAdditionalData = createAdditionalData(customerOnSA, request)
        };
        await _customerOnSAService.UpdateCustomerDetail(updateRequest, cancellationToken);

        // jestlize se na klientovi neco menilo
        if (updateRequest.CustomerChangeMetadata.WasCRSChanged || updateRequest.CustomerChangeMetadata.WereClientDataChanged)
        {
            await cancelSigning(
                customerOnSA.CustomerOnSAId,
                customerOnSA.SalesArrangementId,
                updateRequest.CustomerChangeMetadata.WereClientDataChanged,
                updateRequest.CustomerChangeMetadata.WasCRSChanged,
                cancellationToken);
        }
    }

    private async Task cancelSigning(
        int customerOnSAId,
        int salesArrangementId,
        bool wereClientDataChanged,
        bool wasCRSChanged,
        CancellationToken cancellationToken)
    {
        var documentsToSign = (await _documentOnSAService.GetDocumentsToSignList(salesArrangementId, cancellationToken))
            .DocumentsOnSAToSign
            .Where(t => t.DocumentOnSAId.HasValue);
        var usedDocumentIds = new List<int>();

        if (wereClientDataChanged) // zmena klientskych udaju
        {
            var household = (await _householdService.GetHouseholdList(salesArrangementId, cancellationToken))
                .First(t => t.CustomerOnSAId1 == customerOnSAId || t.CustomerOnSAId2 == customerOnSAId);

            foreach (var doc in documentsToSign.Where(t => t.HouseholdId == household.HouseholdId))
            {
                await _documentOnSAService.StopSigning(new() { DocumentOnSAId = doc.DocumentOnSAId!.Value }, cancellationToken);
                usedDocumentIds.Add(doc.DocumentOnSAId.Value);
            }

            // set flow switches
            await _salesArrangementService.SetFlowSwitches(salesArrangementId, new()
            {
                new()
                {
                    FlowSwitchId = (int)(household.HouseholdTypeId == (int)HouseholdTypes.Main ? FlowSwitches.Was3601MainChangedAfterSigning : FlowSwitches.Was3602CodebtorChangedAfterSigning),
                    Value = true
                }
            }, cancellationToken);
        }
        
        if (wasCRSChanged) // zmena CRS
        {
            var crsDoc = documentsToSign.FirstOrDefault(t => t.DocumentTypeId == 13 && t.CustomerOnSA.CustomerOnSAId == customerOnSAId && !usedDocumentIds.Contains(t.DocumentOnSAId!.Value));//HH rikal, ze 14 neni spravne, ze to ma byt 13
            if (crsDoc != null)
            {
                await _documentOnSAService.StopSigning(new() { DocumentOnSAId = crsDoc.DocumentOnSAId!.Value }, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Vraci true, pokud se zmenily zakladni udaje CustomerOnSA instance 
    /// (tj. pole, ktera jsou primo na entite, nikoliv v JSON datech)
    /// </summary>
    private static bool isStoredModelDifferentToRequest(__Household.CustomerOnSA customerOnSA, UpdateCustomerDetailWithChangesRequest request)
    {
        return !customerOnSA.Name.Equals(request.NaturalPerson?.LastName, StringComparison.Ordinal)
            || !customerOnSA.FirstNameNaturalPerson.Equals(request.NaturalPerson?.FirstName, StringComparison.Ordinal)
            || !customerOnSA.DateOfBirthNaturalPerson.Equals(request.NaturalPerson?.DateOfBirth)
            || customerOnSA.MaritalStatusId != request.NaturalPerson?.MaritalStatusId;
    }

    /// <summary>
    /// Vytvori metadata CustomerOnSA
    /// </summary>
    private static __Household.CustomerChangeMetadata createMetadata(UpdateCustomerDetailWithChangesRequest? originalModel, UpdateCustomerDetailWithChangesRequest request, dynamic? delta)
    {
        var metadata = new __Household.CustomerChangeMetadata();

        if (originalModel?.IsUSPerson != request.IsUSPerson)
        {
            metadata.WasCRSChanged = true;
        }
        else if (!ModelComparers.AreObjectsEqual(request.NaturalPerson?.TaxResidences, originalModel?.NaturalPerson?.TaxResidences))
        {
            metadata.WasCRSChanged = true;
        }

        if (delta is not null)
        {
            var dict = (IDictionary<string, Object>)delta;
            if (dict.Count > 0 &&
                (dict.Count > 1
                || !dict.ContainsKey("NaturalPerson")
                || (dict.ContainsKey("NaturalPerson") && ((IDictionary<string, Object>)dict["NaturalPerson"]).Any(t => t.Key != "TaxResidences"))
                || !metadata.WasCRSChanged))
            {
                metadata.WereClientDataChanged = true;
            }
        }

        return metadata;
    }

    private static string? createJsonFromDelta(dynamic? delta)
    {
        if (delta is not null && ((IDictionary<string, Object>)delta).Count > 0)
        {
            return JsonConvert.SerializeObject(delta);
        }
        return null;
    }

    /// <summary>
    /// Vytvori JSON objekt, ktery obsahuje rozdil (deltu) mezi tim, co prislo v requestu a tim, co mame aktualne ulozene v CustomerOnSA a KB CM.
    /// </summary>
    private static dynamic createDelta(UpdateCustomerDetailWithChangesRequest? originalModel, UpdateCustomerDetailWithChangesRequest request)
    {
        // compare objects
        dynamic delta = new System.Dynamic.ExpandoObject();

        ModelComparers.ComparePerson(request.NaturalPerson, originalModel?.NaturalPerson, delta);
        ModelComparers.CompareObjects(request.IdentificationDocument, originalModel?.IdentificationDocument, "IdentificationDocument", delta);
        ModelComparers.CompareObjects(request.CustomerIdentification, originalModel?.CustomerIdentification, "CustomerIdentification", delta);
        ModelComparers.CompareObjects(request.Addresses, originalModel?.Addresses, "Addresses", delta);
        //ModelComparers.CompareObjects(request.Contacts, originalModel.Contacts, "Contacts", delta);

        // tohle je zajimavost - do delty ukladame zmeny jen u kontaktu, ktere nejsou v CM jako IsConfirmed=true
        if (!(originalModel?.EmailAddress?.IsConfirmed ?? false))
            ModelComparers.CompareObjects(request.EmailAddress, originalModel!.EmailAddress, "EmailAddress", delta);
        if (!(originalModel?.MobilePhone?.IsConfirmed ?? false))
            ModelComparers.CompareObjects(request.MobilePhone, originalModel!.MobilePhone, "MobilePhone", delta);

        return delta;
    }

    /// <summary>
    /// Vytvori / upravi JSON data v prop CustomerAdditionalData
    /// Data se upravuji na zaklade toho, co prijde v requestu.
    /// </summary>
    private __Household.CustomerAdditionalData createAdditionalData(
        __Household.CustomerOnSA customerOnSA,
        UpdateCustomerDetailWithChangesRequest request)
    {
        var additionalData = customerOnSA.CustomerAdditionalData ?? new __Household.CustomerAdditionalData();

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

    private readonly CustomerWithChangedDataService _changedDataService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerServiceClient _customerService;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ICurrentUserAccessor _userAccessor;

    public UpdateCustomerDetailWithChangesHandler(
        ICaseServiceClient caseService,
        CustomerWithChangedDataService changedDataService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerOnSAServiceClient customerOnSAService,
        IUserServiceClient userServiceClient,
        IDocumentOnSAServiceClient documentOnSAService,
        ICurrentUserAccessor userAccessor,
        IHouseholdServiceClient householdService,
        ICustomerServiceClient customerService)
    {
        _documentOnSAService = documentOnSAService;
        _caseService = caseService;
        _changedDataService = changedDataService;
        _salesArrangementService = salesArrangementService;
        _customerOnSAService = customerOnSAService;
        _userServiceClient = userServiceClient;
        _userAccessor = userAccessor;
        _householdService = householdService;
        _customerService = customerService;
    }
}
