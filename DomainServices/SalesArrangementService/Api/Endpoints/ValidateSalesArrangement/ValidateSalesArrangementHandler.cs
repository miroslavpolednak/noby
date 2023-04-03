using DomainServices.SalesArrangementService.Contracts;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement;

internal sealed class ValidateSalesArrangementHandler
    : IRequestHandler<ValidateSalesArrangementRequest, ValidateSalesArrangementResponse>
{
    private static readonly int[] ValidCommonValues = { 0, 6 };

    private readonly Services.ValidationTransformationServiceFactory _transformationServiceFactory;
    private readonly Services.Forms.FormsService _formsService;
    private readonly Services.Forms.EasFormsManager _easFormsManager;

    public ValidateSalesArrangementHandler(
        Services.ValidationTransformationServiceFactory transformationServiceFactory,
        Services.Forms.FormsService formsService,
        Services.Forms.EasFormsManager easFormsManager)
    {
        _transformationServiceFactory = transformationServiceFactory;
        _formsService = formsService;
        _easFormsManager = easFormsManager;
    }

    public async Task<ValidateSalesArrangementResponse> Handle(ValidateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _formsService.LoadSalesArrangement(request.SalesArrangementId, cancellationToken);
        var category = await _formsService.LoadSalesArrangementCategory(salesArrangement, cancellationToken);

        var easFormResponse = await GetEasForm(salesArrangement, category, cancellationToken);

        return await CheckForms(easFormResponse,cancellationToken);
    }

    private async Task<GetEasFormResponse> GetEasForm(SalesArrangement salesArrangement, SalesArrangementCategories category, CancellationToken cancellationToken)
    {
        return category switch
        {
            SalesArrangementCategories.ProductRequest => await ProcessProductRequest(salesArrangement, cancellationToken),
            SalesArrangementCategories.ServiceRequest => await ProcessServiceRequest(salesArrangement, cancellationToken),
            _ => throw new NotImplementedException()
        };
    }

    private async Task<GetEasFormResponse> ProcessProductRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicFormValues = _formsService.CreateProductDynamicFormValues(salesArrangement, cancellationToken);

        await _easFormsManager.UpdateContractNumberIfNeeded(salesArrangement, cancellationToken);

        return await _formsService.LoadProductForm(salesArrangement, await dynamicFormValues.ToListAsync(cancellationToken), cancellationToken);
    }

    private async Task<GetEasFormResponse> ProcessServiceRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicFormValues = await _formsService.CreateServiceDynamicFormValues(salesArrangement, cancellationToken);

        return await _formsService.LoadServiceForm(salesArrangement.SalesArrangementId, new[] { dynamicFormValues }, cancellationToken);
    }

    private async Task<ValidateSalesArrangementResponse> CheckForms(GetEasFormResponse easForm, CancellationToken cancellationToken)
    {
        var checkForms = easForm.Forms.Select(f => new Eas.EasWrapper.CheckFormData
        {
            formular_id = Services.Forms.EasFormsManager.GetFormId(f.EasFormType),
            cislo_smlouvy = easForm.ContractNumber,
            dokument_id = "", //null neprojde
            datum_prijeti = DateTime.Now.Date,
            data = f.Json,
        });

        var validationResult = await Task.WhenAll(checkForms.Select(data => SendAndValidateForm(data, cancellationToken)));

        return new ValidateSalesArrangementResponse
        {
            ValidationMessages = { validationResult.SelectMany(messages => messages) }
        };
    }

    private async Task<List<ValidationMessage>> SendAndValidateForm(Eas.EasWrapper.CheckFormData checkFormData, CancellationToken cancellationToken)
    {
        var checkFormResult = await _easFormsManager.CheckForms(checkFormData, cancellationToken);

        if (!ValidCommonValues.Contains(checkFormResult.CommonValue))
        {
            var message = $"Check form common error [CommonValue: {checkFormResult.CommonValue}, CommonText: {checkFormResult.CommonText}]";

            throw new CisValidationException(checkFormResult.CommonValue == 2 ? ErrorCodeMapper.SendAndValidateForm2 : ErrorCodeMapper.SendAndValidateForm1, message);
        }

        var transformationService = _transformationServiceFactory.CreateService(checkFormData.formular_id);
        
        return transformationService.TransformErrors(checkFormData.data, checkFormResult.Detail?.errors);
    }
}

