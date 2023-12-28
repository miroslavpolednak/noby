using NOBY.Services.CreateProductTrain.Handlers;

namespace NOBY.Services.CreateProductTrain;

[ScopedService, AsImplementedInterfacesService]
internal sealed class CreateProductTrainService
    : ICreateProductTrainService
{
    public async Task Run(
        long caseId,
        int salesArrangementId,
        int customerOnSAId,
        IEnumerable<SharedTypes.GrpcTypes.Identity>? customerIdentifiers,
        CancellationToken cancellationToken = default)
    {
        await _updateCustomer.Run(caseId, customerOnSAId, cancellationToken);

        await _product.Run(caseId, salesArrangementId, customerOnSAId, customerIdentifiers, cancellationToken);

        await _createRiskBusinessCase.Run(salesArrangementId, cancellationToken);
    }

    private readonly CreateProduct _product;
    private readonly UpdateCustomerOnCase _updateCustomer;
    private readonly CreateRiskBusinessCase _createRiskBusinessCase;

    public CreateProductTrainService(CreateProduct createProduct, UpdateCustomerOnCase updateCustomer, CreateRiskBusinessCase createRiskBusinessCase)
    {
        _updateCustomer = updateCustomer;
        _createRiskBusinessCase = createRiskBusinessCase;
        _product = createProduct;
    }
}
