using DomainServices.ProductService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Endpoints.GetCustomersOnProduct;

internal sealed class GetCustomersOnProductHandler
    : IRequestHandler<Contracts.GetCustomersOnProductRequest, GetCustomersOnProductResponse>
{
    public async Task<GetCustomersOnProductResponse> Handle(Contracts.GetCustomersOnProductRequest request, CancellationToken cancellation)
    {
        // Kontrola, zda se jedná o KB produkt (typ uveru = 3), chyba pokud ne vyhodit chybu
        var typUveru = (await _dbContext.Loans.AsNoTracking().FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellation))?.TypUveru;
        if (typUveru == null)
            throw new CisNotFoundException(12001, "ProductInstanceId does not exist in KonsDB.");
        else if (typUveru.Value != 3)
            throw new CisValidationException(12019, "Unsupported product type.");

        // Zjištění seznamu klientů na produktu, vyhodit tvrdou chybu pokud je množina prázdná (nesmí se stávat, pokud není nekonzistence dat)
        var customers = await _dbContext.Relationships
            .AsNoTracking()
            .Where(t => t.UverId == request.ProductId)
            .Select(t => new { Vztah = t.VztahId, MpId = t.PartnerId, KbId = t.Partner.KBId })
            .ToListAsync(cancellation);
        if (!customers.Any())
            throw new CisValidationException(12020, "Customers not found for product.");
        if (customers.Any(t => !t.KbId.HasValue))
            throw new CisValidationException(12021, "Not all customers does have KB ID");

        var model = new GetCustomersOnProductResponse();
        foreach (var customer in customers)
        {
            var item = new GetCustomersOnProductResponseItem
            {
                RelationshipCustomerProductTypeId = customer.Vztah
            };
            item.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(customer.MpId, CIS.Foms.Enums.IdentitySchemes.Mp));
            item.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(customer.KbId!.Value, CIS.Foms.Enums.IdentitySchemes.Kb));

            //TODO MOCK https://jira.kb.cz/browse/HFICH-3284
            var random = new Random();
            item.IsKYCSuccessful = random.Next(2) == 1;

            model.Customers.Add(item);
        }

        return model;
    }

    private readonly Database.ProductServiceDbContext _dbContext;

    public GetCustomersOnProductHandler(Database.ProductServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
