using SharedTypes.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.GetCustomersOnProduct;

internal sealed class GetCustomersOnProductHandler
    : IRequestHandler<GetCustomersOnProductRequest, GetCustomersOnProductResponse>
{
    public async Task<GetCustomersOnProductResponse> Handle(GetCustomersOnProductRequest request, CancellationToken cancellation)
    {
        var productTypes = await _codebookService.ProductTypes(cancellation);

        var loan = await _repository.GetLoan(request.ProductId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        // Kontrola, zda se jedná o KB produkt, chyba pokud ne vyhodit chybu
        if (!productTypes.Any(p => p.Id == loan.ProductTypeId && p.MandantId == (int)Mandants.Kb))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.InvalidArgument12019);
        }

        // Zjištění seznamu klientů na produktu, vyhodit tvrdou chybu pokud je množina prázdná (nesmí se stávat, pokud není nekonzistence dat)
        var customers = await _repository.GetRelationships(request.ProductId, cancellation);
        
        if (!customers.Any())
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.InvalidArgument12020);
        }

        var model = new GetCustomersOnProductResponse();
        foreach (var customer in customers)
        {
            var item = new GetCustomersOnProductResponseItem
            {
                RelationshipCustomerProductTypeId = customer.ContractRelationshipTypeId,
                Agent = customer.Agent ?? false,
                IsKYCSuccessful = customer.Kyc ?? false
            };

            item.CustomerIdentifiers.Add(new SharedTypes.GrpcTypes.Identity(customer.PartnerId, IdentitySchemes.Mp));

            if (customer.KbId.HasValue)
                item.CustomerIdentifiers.Add(new SharedTypes.GrpcTypes.Identity(customer.KbId!.Value, IdentitySchemes.Kb));

            model.Customers.Add(item);
        }

        return model;
    }

    private readonly Database.LoanRepository _repository;
    private readonly ICodebookServiceClient _codebookService;

    public GetCustomersOnProductHandler(Database.LoanRepository repository, ICodebookServiceClient codebookService)
    {
        _repository = repository;
        _codebookService = codebookService;
    }
}
