using CIS.Core.ErrorCodes;
using SharedTypes.Enums;
using DomainServices.CodebookService.Clients;

namespace DomainServices.ProductService.Api.Endpoints.GetCustomersOnProduct;

internal sealed class GetCustomersOnProductHandler : IRequestHandler<GetCustomersOnProductRequest, GetCustomersOnProductResponse>
{
    private readonly LoanRepository _repository;
    private readonly ICodebookServiceClient _codebookService;

    public GetCustomersOnProductHandler(LoanRepository repository, ICodebookServiceClient codebookService)
    {
        _repository = repository;
        _codebookService = codebookService;
    }

    public async Task<GetCustomersOnProductResponse> Handle(GetCustomersOnProductRequest request, CancellationToken cancellationToken)
    {
        var loan = await _repository.GetLoan(request.ProductId, cancellationToken)
            ?? throw ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        // Kontrola, zda se jedná o KB produkt, chyba pokud ne vyhodit chybu
        await CheckIfProductIsKb(loan.ProductTypeId, cancellationToken);

        // Zjištění seznamu klientů na produktu, vyhodit tvrdou chybu pokud je množina prázdná (nesmí se stávat, pokud není nekonzistence dat)
        var customers = await _repository.GetRelationships(request.ProductId, cancellationToken);

        if (customers.Count == 0)
        {
            throw ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.InvalidArgument12020);
        }

        return new GetCustomersOnProductResponse
        {
            Customers =
            {
                customers.Select(c =>
                {
                    var customerResponse = new GetCustomersOnProductResponseItem
                    {
                        RelationshipCustomerProductTypeId = c.ContractRelationshipTypeId,
                        Agent = c.Agent ?? false,
                        IsKYCSuccessful = c.Kyc ?? false
                    };

                    customerResponse.CustomerIdentifiers.Add(new SharedTypes.GrpcTypes.Identity(c.PartnerId, IdentitySchemes.Mp));

                    if (c.KbId.HasValue)
                        customerResponse.CustomerIdentifiers.Add(new SharedTypes.GrpcTypes.Identity(c.KbId!.Value, IdentitySchemes.Kb));

                    return customerResponse;
                })
            }
        };
    }

    private async Task CheckIfProductIsKb(int? productTypeId, CancellationToken cancellationToken)
    {
        var productTypes = await _codebookService.ProductTypes(cancellationToken);

        if (productTypes.Any(p => p.Id == productTypeId && p.MandantId == (int)Mandants.Kb))
            return;

        throw ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.InvalidArgument12019);
    }
}
