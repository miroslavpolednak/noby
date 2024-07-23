using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement.ValidationStrategy;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement;

internal sealed class ValidateSalesArrangementHandler(
    Services.Forms.FormsService _formsService, 
    IServiceProvider _serviceProvider)
		: IRequestHandler<ValidateSalesArrangementRequest, ValidateSalesArrangementResponse>
{
	public async Task<ValidateSalesArrangementResponse> Handle(ValidateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _formsService.LoadSalesArrangement(request.SalesArrangementId, cancellationToken);
        var saType = await _formsService.LoadSalesArrangementType(salesArrangement.SalesArrangementTypeId, cancellationToken);

        var validationStrategy = GetValidationStrategy(saType);

        return await validationStrategy.Validate(salesArrangement, cancellationToken);
    }

    private ISalesArrangementValidationStrategy GetValidationStrategy(SalesArrangementTypesResponse.Types.SalesArrangementTypeItem saType)
    {
        if (!saType.IsFormSentToCmp)
            return _serviceProvider.GetRequiredService<ServiceAgreementValidation>();

        if (saType.Id != 6)
            return _serviceProvider.GetRequiredService<CheckFormWithCustomerDetailValidationStrategy>();

        return _serviceProvider.GetRequiredService<CheckFormSalesArrangementValidation>();
    }
}

