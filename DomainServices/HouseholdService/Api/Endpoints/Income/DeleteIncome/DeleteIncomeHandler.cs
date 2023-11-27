using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.DeleteIncome;

internal sealed class DeleteIncomeHandler
    : IRequestHandler<DeleteIncomeRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        var deletedRows = await _documentDataStorage.Delete<Database.DocumentDataEntities.Income>(request.IncomeId);

        if (deletedRows == 0)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.IncomeNotFound, request.IncomeId);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IDocumentDataStorage _documentDataStorage;

    public DeleteIncomeHandler(IDocumentDataStorage documentDataStorage)
    {
        _documentDataStorage = documentDataStorage;
    }
}