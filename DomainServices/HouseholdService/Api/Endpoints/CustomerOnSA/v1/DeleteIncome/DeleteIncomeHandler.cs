using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.DeleteIncome;

internal sealed class DeleteIncomeHandler(IDocumentDataStorage _documentDataStorage)
        : IRequestHandler<DeleteIncomeRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        int count = await _documentDataStorage.Delete<Database.DocumentDataEntities.Income>(request.IncomeId);

        if (count == 0)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.IncomeNotFound, request.IncomeId);
        }

        return new();
    }
}