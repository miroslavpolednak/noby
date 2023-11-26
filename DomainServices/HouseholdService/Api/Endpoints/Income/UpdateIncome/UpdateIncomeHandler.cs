using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.UpdateIncome;

internal sealed class UpdateIncomeHandler
    : IRequestHandler<UpdateIncomeRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateIncomeRequest request, CancellationToken cancellationToken)
    {
        var documentEntity = await _incomeMapper.MapToDocumentData(request.IncomeTypeId, request.BaseData, request.Employement, request.Entrepreneur, request.Other, cancellationToken);

        await _documentDataStorage.Update(request.IncomeId, documentEntity, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Services.IncomeToDataMapper _incomeMapper;

    public UpdateIncomeHandler(
        Services.IncomeToDataMapper incomeMapper,
        IDocumentDataStorage documentDataStorage)
    {
        _documentDataStorage = documentDataStorage;
        _incomeMapper = incomeMapper;
    }
}