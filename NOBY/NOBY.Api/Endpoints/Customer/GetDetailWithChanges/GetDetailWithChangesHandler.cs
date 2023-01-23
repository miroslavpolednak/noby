using DomainServices.SalesArrangementService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using CIS.Infrastructure.gRPC.CisTypes;
using NOBY.Api.Endpoints.Customer.Shared;
using Newtonsoft.Json.Linq;

namespace NOBY.Api.Endpoints.Customer.GetDetailWithChanges;

internal sealed class GetDetailWithChangesHandler
    : IRequestHandler<GetDetailWithChangesRequest, GetDetailWithChangesResponse>
{
    public async Task<GetDetailWithChangesResponse> Handle(GetDetailWithChangesRequest request, CancellationToken cancellationToken)
    {
        // customer instance
        var customerOnSA = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);

        // kontrola identity KB
        var kbIdentity = customerOnSA.CustomerIdentifiers
            .FirstOrDefault(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb)
            ?? throw new CisValidationException("Customer is missing KB identity");

        // SA instance
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(customerOnSA.SalesArrangementId, cancellationToken);

        // kontrola mandanta
        var productTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken)).First(t => t.Id == salesArrangement.SalesArrangementTypeId).ProductTypeId;
        // mandant produktu
        var productMandant = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == productTypeId).MandantId;
        if (productMandant != 2) // muze byt jen KB
            throw new CisValidationException("Product type mandant is not KB");

        // instance customer z KB CM
        var customer = await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);

        // convert DS contract to FE model
        var model = customer.FillResponseDto(new GetDetailWithChangesResponse());

        // https://jira.kb.cz/browse/HFICH-4200
        if (customerOnSA.CustomerAdditionalData?.LegalCapacity is not null)
        {
            model.LegalCapacity = new()
            {
                RestrictionTypeId = customerOnSA.CustomerAdditionalData.LegalCapacity.RestrictionTypeId,
                RestrictionUntil = customerOnSA.CustomerAdditionalData.LegalCapacity.RestrictionUntil
            };
        }

        // changed data already exist in database
        if (!string.IsNullOrEmpty(customerOnSA.CustomerChangeData))
        {
            // provide saved changes to original model
            var original = JObject.FromObject(model);
            var delta = JObject.Parse(customerOnSA.CustomerChangeData);

            original.Merge(delta, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Replace,
                MergeNullValueHandling = MergeNullValueHandling.Merge
            });

            var changedData = original.ToObject<GetDetailWithChangesResponse>()!;

            // https://jira.kb.cz/browse/HFICH-4200
            // docasne reseni nez se CM rozmysli jak na to
            changedData.LegalCapacity = new()
            {
                RestrictionTypeId = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionTypeId,
                RestrictionUntil = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionUntil
            };

            return changedData;
        }
        else
        {
            return model;
        }
    }

    private readonly ICodebookServiceClients _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerServiceClient _customerService;

    public GetDetailWithChangesHandler(
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
