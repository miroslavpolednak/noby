﻿namespace NOBY.Services.CreateProductTrain;

public interface ICreateProductTrainService
{
    Task RunAll(
        long caseId,
        int salesArrangementId,
        int customerOnSAId,
        IEnumerable<SharedTypes.GrpcTypes.Identity>? customerIdentifiers,
        CancellationToken cancellationToken = default);
}
