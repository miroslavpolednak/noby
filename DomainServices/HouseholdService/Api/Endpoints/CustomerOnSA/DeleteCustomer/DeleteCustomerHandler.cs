using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteCustomer;

internal sealed class DeleteCustomerHandler
    : IRequestHandler<DeleteCustomerRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        // instance customera z DB
        var customer = await _dbContext
            .Customers
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(t => new { t.CustomerRoleId, t.SalesArrangementId, t.Identities })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId);

        // KB identita pokud existuje
        var kbIdentity = customer
            .Identities?
            .FirstOrDefault(t => t.IdentityScheme == IdentitySchemes.Kb);

        // SULM
        if (kbIdentity is not null)
        {
            await _sulmClient.StartUse(kbIdentity.IdentityId, ExternalServices.Sulm.V1.ISulmClient.PurposeMLAX, cancellationToken);
        }

        // Invalidate DocumentsOnSa Crs
        var documentsOnSaToSing = await _documentOnSAServiceClient.GetDocumentsToSignList(customer.SalesArrangementId, cancellationToken);
        var documentsOnSaCrs = documentsOnSaToSing.DocumentsOnSAToSign.Where(r => r.DocumentOnSAId is not null && r.CustomerOnSA?.CustomerOnSAId == request.CustomerOnSAId);
        await StopSigning(documentsOnSaCrs, cancellationToken);

        // smazat customer + prijmy + obligations + identities
        await deleteEntities(request.CustomerOnSAId, cancellationToken);

        // smazat Agent z SA, pokud je Agent=aktualni CustomerOnSAId
        var saInstance = await _salesArrangementService.GetSalesArrangement(customer.SalesArrangementId, cancellationToken);
        if (saInstance.Mortgage?.Agent == request.CustomerOnSAId)
        {
            await deleteAgentFromSalesArrangement(saInstance, customer.SalesArrangementId, cancellationToken);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private async Task deleteEntities(int customerOnSAId, CancellationToken cancellationToken)
    {
        await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == customerOnSAId)
            .ExecuteDeleteAsync(cancellationToken);

        // smazat identity
        await _dbContext.CustomersIdentities
            .Where(t => t.CustomerOnSAId == customerOnSAId)
            .ExecuteDeleteAsync(cancellationToken);

        // smazat prijmy
        await _documentDataStorage.DeleteByEntityId<Database.DocumentDataEntities.Income>(customerOnSAId, cancellationToken);

        // smazat zavazky
        await _dbContext.CustomersObligations
            .Where(t => t.CustomerOnSAId == customerOnSAId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private async Task StopSigning(IEnumerable<DocumentOnSAToSign> documentOnSAToSigns, CancellationToken cancellationToken)
    {
        foreach (var doc in documentOnSAToSigns)
        {
            // Have to be call one by one
            await _documentOnSAServiceClient.StopSigning(new() 
            { 
                DocumentOnSAId = doc.DocumentOnSAId!.Value,
                SkipValidations = true
            }, cancellationToken);
        }
    }

    private async Task deleteAgentFromSalesArrangement(_SA.SalesArrangement saInstance, int salesArrangementId, CancellationToken cancellationToken)
    {
        // ziskat ID hlavniho customera
        int? mainCustomerOnSAId = (await _dbContext
            .Households
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.SalesArrangementId == salesArrangementId && t.HouseholdTypeId == HouseholdTypes.Main, cancellationToken)
        )?.CustomerOnSAId1;

        saInstance.Mortgage.Agent = mainCustomerOnSAId;

        await _salesArrangementService.UpdateSalesArrangementParameters(new _SA.UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangementId,
            Mortgage = saInstance.Mortgage
        }, cancellationToken);
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly SulmService.ISulmClientHelper _sulmClient;
    private readonly Database.HouseholdServiceDbContext _dbContext;
    private readonly IDocumentOnSAServiceClient _documentOnSAServiceClient;

    public DeleteCustomerHandler(
        IDocumentDataStorage documentDataStorage,
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        SulmService.ISulmClientHelper sulmClient,
        Database.HouseholdServiceDbContext dbContext,
        IDocumentOnSAServiceClient documentOnSAServiceClient)
    {
        _documentDataStorage = documentDataStorage;
        _salesArrangementService = salesArrangementService;
        _sulmClient = sulmClient;
        _dbContext = dbContext;
        _documentOnSAServiceClient = documentOnSAServiceClient;
    }
}