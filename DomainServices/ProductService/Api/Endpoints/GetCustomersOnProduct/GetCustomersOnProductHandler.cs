using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Endpoints.GetCustomersOnProduct;

internal sealed class GetCustomersOnProductHandler
    : IRequestHandler<GetCustomersOnProductRequest, GetCustomersOnProductResponse>
{
    public async Task<GetCustomersOnProductResponse> Handle(GetCustomersOnProductRequest request, CancellationToken cancellation)
    {
        var productTypes = await _codebookService.ProductTypes(cancellation);

        var loan = await _dbContext.Loans.AsNoTracking().FirstOrDefaultAsync(t => t.Id == request.ProductId && !t.Neaktivni, cancellation) ??
                   throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        // Kontrola, zda se jedná o KB produkt, chyba pokud ne vyhodit chybu
        if (!productTypes.Any(p => p.Id == loan.KodProduktyUv && p.MandantId == (int)Mandants.Kb))
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.InvalidArgument12019);

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

            if (customer.KbId.HasValue)
                item.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(customer.KbId!.Value, CIS.Foms.Enums.IdentitySchemes.Kb));

            model.Customers.Add(item);
        }

        return model;
    }

    private readonly Database.ProductServiceDbContext _dbContext;
    private readonly ICodebookServiceClient _codebookService;

    public GetCustomersOnProductHandler(Database.ProductServiceDbContext dbContext, ICodebookServiceClient codebookService)
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
    }
}
