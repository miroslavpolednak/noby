namespace NOBY.Services.CreateProductTrain;

public interface ICreateProductTrainService
{
    Task Run(
        long caseId,
        int salesArrangementId,
        int customerOnSAId,
        IEnumerable<SharedTypes.GrpcTypes.Identity>? customerIdentifiers,
        CancellationToken cancellationToken = default);
}
