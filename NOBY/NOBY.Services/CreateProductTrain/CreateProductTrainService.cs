﻿using Microsoft.Extensions.Logging;
using NOBY.Services.CreateProductTrain.Handlers;

namespace NOBY.Services.CreateProductTrain;

[ScopedService, AsImplementedInterfacesService]
internal sealed class CreateProductTrainService
    : ICreateProductTrainService
{
    public async Task RunAll(
        long caseId,
        int salesArrangementId,
        int customerOnSAId,
        IEnumerable<SharedTypes.GrpcTypes.Identity>? customerIdentifiers,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting CreateProductTrainService");

        await _updateCustomer.Run(caseId, customerOnSAId, cancellationToken);

        await _product.Run(caseId, salesArrangementId, customerOnSAId, customerIdentifiers, cancellationToken);

        long? mpId = customerIdentifiers?.GetMpIdentityOrDefault()?.IdentityId;
        long? kbId = customerIdentifiers?.GetKbIdentityOrDefault()?.IdentityId;
        if (mpId.HasValue && kbId.HasValue)
        {
            await _createRiskBusinessCase.Run(salesArrangementId, cancellationToken);
        }
        else
        {
            _logger.LogInformation("CreateRiskBusinessCaseService for CaseId #{CaseId} not proceeding / missing MP ID", caseId);
        }
        
        _logger.LogDebug("CreateProductTrainService finished");
    }

    private readonly ILogger<CreateProductTrainService> _logger;
    private readonly CreateProduct _product;
    private readonly UpdateCustomerOnCase _updateCustomer;
    private readonly CreateRiskBusinessCase _createRiskBusinessCase;

    public CreateProductTrainService(CreateProduct createProduct, UpdateCustomerOnCase updateCustomer, CreateRiskBusinessCase createRiskBusinessCase, ILogger<CreateProductTrainService> logger)
    {
        _logger = logger;
        _updateCustomer = updateCustomer;
        _createRiskBusinessCase = createRiskBusinessCase;
        _product = createProduct;
    }
}
