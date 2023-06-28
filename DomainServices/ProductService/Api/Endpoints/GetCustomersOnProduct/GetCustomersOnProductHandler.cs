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
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);
        }
        if (typUveru.Value != 3)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.InvalidArgument12019);
        }
        
        // Zjištění seznamu klientů na produktu, vyhodit tvrdou chybu pokud je množina prázdná (nesmí se stávat, pokud není nekonzistence dat)
        var customers = await _dbContext.Relationships
            .AsNoTracking()
            .Where(t => t.UverId == request.ProductId)
            .Select(t => new { Vztah = t.VztahId, MpId = t.PartnerId, KbId = t.Partner.KBId, t.Zmocnenec, t.Partner.StavKyc })
            .ToListAsync(cancellation);

        if (!customers.Any())
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.InvalidArgument12020);
        }

        if (customers.Any(t => !t.KbId.HasValue))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.InvalidArgument12021);
        }


        var model = new GetCustomersOnProductResponse();
        foreach (var customer in customers)
        {
            var item = new GetCustomersOnProductResponseItem
            {
                RelationshipCustomerProductTypeId = customer.Vztah,
                Agent = customer.Zmocnenec.GetValueOrDefault(),
                IsKYCSuccessful = customer.StavKyc == 1
            };
            item.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(customer.MpId, CIS.Foms.Enums.IdentitySchemes.Mp));
            item.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(customer.KbId!.Value, CIS.Foms.Enums.IdentitySchemes.Kb));

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
