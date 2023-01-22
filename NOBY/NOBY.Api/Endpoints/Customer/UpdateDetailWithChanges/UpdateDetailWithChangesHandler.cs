using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using Newtonsoft.Json;
using NOBY.Api.Endpoints.Customer.Shared;

namespace NOBY.Api.Endpoints.Customer.UpdateDetailWithChanges;

internal sealed class UpdateDetailWithChangesHandler
    : AsyncRequestHandler<UpdateDetailWithChangesRequest>
{
    protected override async Task Handle(UpdateDetailWithChangesRequest request, CancellationToken cancellationToken)
    {
        // customer instance
        var customerOnSA = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);

        // kontrola identity KB
        var kbIdentity = customerOnSA.CustomerIdentifiers
            .FirstOrDefault(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb)
            ?? throw new CisValidationException("Customer is missing KB identity");

        // instance customer z KB CM
        var customer = await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);
        // convert DS contract to FE model
        var originalModel = customer.FillResponseDto(new UpdateDetailWithChangesRequest());

        // compare objects
        dynamic delta = new System.Dynamic.ExpandoObject();

        ModelComparers.CompareRoot(request, originalModel,delta);
        ModelComparers.ComparePerson(request.NaturalPerson, originalModel.NaturalPerson, delta);
        ModelComparers.CompareObjects(request.IdentificationDocument, originalModel.IdentificationDocument, "IdentificationDocument", delta);
        ModelComparers.CompareObjects(request.Addresses, originalModel.Addresses, "Addresses", delta);
        ModelComparers.CompareObjects(request.Contacts, originalModel.Contacts, "Contacts", delta);

        // https://jira.kb.cz/browse/HFICH-4200
        if (customerOnSA.CustomerAdditionalData is null)
            customerOnSA.CustomerAdditionalData = new();
        customerOnSA.CustomerAdditionalData.LegalCapacity = new()
        {
            RestrictionTypeId = request.LegalCapacity?.RestrictionTypeId,
            RestrictionUntil = request.LegalCapacity?.RestrictionUntil
        };

        string? finalJson = null;
        if (((IDictionary<string, Object>)delta).Count > 0)
        {
            finalJson = JsonConvert.SerializeObject(delta);
        }

        var updateRequest = new DomainServices.HouseholdService.Contracts.UpdateCustomerDetailRequest
        {
            CustomerOnSAId = customerOnSA.CustomerOnSAId,
            CustomerChangeData = finalJson,
            CustomerAdditionalData = customerOnSA.CustomerAdditionalData
        };
        await _customerOnSAService.UpdateCustomerDetail(updateRequest, cancellationToken);
    }

    private readonly ICodebookServiceClients _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerServiceClient _customerService;

    public UpdateDetailWithChangesHandler(
        ICustomerServiceClient customerService,
        ICodebookServiceClients codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerOnSAServiceClient customerOnSAService)
    {
        _customerService = customerService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _customerOnSAService = customerOnSAService;
    }
}
