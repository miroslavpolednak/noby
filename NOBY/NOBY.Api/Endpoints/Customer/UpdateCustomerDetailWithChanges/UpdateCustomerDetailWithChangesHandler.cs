﻿using CIS.Core.Security;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients;
using Newtonsoft.Json;
using UserIdentity = CIS.Infrastructure.gRPC.CisTypes.UserIdentity;
using __Household = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

internal sealed class UpdateCustomerDetailWithChangesHandler
    : IRequestHandler<UpdateCustomerDetailWithChangesRequest>
{
    public async Task Handle(UpdateCustomerDetailWithChangesRequest request, CancellationToken cancellationToken)
    {
        // customer instance
        var customerOnSA = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);
        
        // customer from KB CM
        var (originalModel, identificationMethodId) = await _changedDataService.GetCustomerFromCM<UpdateCustomerDetailWithChangesRequest>(customerOnSA, cancellationToken);

        // compare objects
        dynamic delta = new System.Dynamic.ExpandoObject();

        ModelComparers.CompareRoot(request, originalModel, delta);
        ModelComparers.ComparePerson(request.NaturalPerson, originalModel.NaturalPerson, delta);
        ModelComparers.CompareObjects(request.IdentificationDocument, originalModel.IdentificationDocument, "IdentificationDocument", delta);
        ModelComparers.CompareObjects(request.Addresses, originalModel.Addresses, "Addresses", delta);
        //ModelComparers.CompareObjects(request.Contacts, originalModel.Contacts, "Contacts", delta);
        
        // tohle je zajimavost - do delty ukladame zmeny jen u kontaktu, ktere nejsou v CM jako IsConfirmed=true
        if (!(originalModel.EmailAddress?.IsConfirmed ?? false))
            ModelComparers.CompareObjects(request.EmailAddress, originalModel.EmailAddress, "EmailAddress", delta);
        if (!(originalModel.MobilePhone?.IsConfirmed ?? false))
            ModelComparers.CompareObjects(request.MobilePhone, originalModel.MobilePhone, "MobilePhone", delta);

        // zjistit zda uz existuji changeData a v nich CustomerIdentification
        /*bool identDocExists = false;
        if (!string.IsNullOrEmpty(customerOnSA.CustomerChangeData))
        {
            var deserializedChangedData = JsonConvert.DeserializeObject<UpdateCustomerDetailWithChangesRequest>(customerOnSA.CustomerChangeData);
            identDocExists = deserializedChangedData?.IdentificationDocument is not null;
        }

        // tady schvalne neresime prvni pindu z EA diagramu, protoze bysme museli z customerOnSA json delty udelat objekt a ten teprve kontrolovat. A to by bylo pomalejsi a narocnejsi nez tuhle podminku vzdy znovu projet.
        if (!identDocExists && identificationMethodId != 1 && identificationMethodId != 8)
        {
            if (_userAccessor.User?.Id != null)
            {
                var user = await _userServiceClient.GetUser(_userAccessor.User.Id, cancellationToken);
                var isBroker = user.UserIdentifiers.Any(u =>
                    u.IdentityScheme == UserIdentity.Types.UserIdentitySchemes.BrokerId);

                delta.CustomerIdentification.CzechIdentificationNumber = user.CzechIdentificationNumber;
                delta.CustomerIdentification.IdentificationMethodId = isBroker ? 8 : 1;
            }
        }*/

        // https://jira.kb.cz/browse/HFICH-4200
        // docasne reseni nez se CM rozmysli jak na to
        if (customerOnSA.CustomerAdditionalData is null)
            customerOnSA.CustomerAdditionalData = new DomainServices.HouseholdService.Contracts.CustomerAdditionalData();
        customerOnSA.CustomerAdditionalData.LegalCapacity = new()
        {
            RestrictionTypeId = request.LegalCapacity?.RestrictionTypeId,
            RestrictionUntil = request.LegalCapacity?.RestrictionUntil
        };
        
        // vytvoreni JSONu z delta objektu
        string? finalJson = null;
        if (((IDictionary<string, Object>)delta).Count > 0)
        {
            finalJson = JsonConvert.SerializeObject(delta);
        }

        var updateRequest = new __Household.UpdateCustomerDetailRequest
        {
            CustomerOnSAId = customerOnSA.CustomerOnSAId,
            CustomerChangeData = finalJson,
            CustomerAdditionalData = createAdditionalData(customerOnSA, request)
        };
        await _customerOnSAService.UpdateCustomerDetail(updateRequest, cancellationToken);
    }

    static __Household.CustomerAdditionalData createAdditionalData(__Household.CustomerOnSA customerOnSA, UpdateCustomerDetailWithChangesRequest request)
    {
        var additionalData = customerOnSA.CustomerAdditionalData is null ? new __Household.CustomerAdditionalData() : customerOnSA.CustomerAdditionalData;

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
        additionalData.IsPoliticallyExposed = request.IsUSPerson.GetValueOrDefault();

        return additionalData;
    }

    private readonly CustomerWithChangedDataService _changedDataService;
    private readonly ICodebookServiceClients _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ICurrentUserAccessor _userAccessor;

    public UpdateCustomerDetailWithChangesHandler(
        CustomerWithChangedDataService changedDataService,
        ICodebookServiceClients codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerOnSAServiceClient customerOnSAService,
        IUserServiceClient userServiceClient,
        ICurrentUserAccessor userAccessor)
    {
        _changedDataService = changedDataService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _customerOnSAService = customerOnSAService;
        _userServiceClient = userServiceClient;
        _userAccessor = userAccessor;
    }
}
