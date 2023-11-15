using CIS.Core.Attributes;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement.ValidationStrategy;

[ScopedService, SelfService]
internal class CheckFormSalesArrangementValidation : ISalesArrangementValidationStrategy
{
    private static readonly int[] ValidCommonValues = { 0, 6 };

    private readonly Services.Forms.FormsService _formsService;
    private readonly Services.ValidationTransformationServiceFactory _transformationServiceFactory;
    private readonly Eas.IEasClient _easClient;

    public CheckFormSalesArrangementValidation(Services.Forms.FormsService formsService, Services.ValidationTransformationServiceFactory transformationServiceFactory, Eas.IEasClient easClient)
    {
        _formsService = formsService;
        _transformationServiceFactory = transformationServiceFactory;
        _easClient = easClient;
    }

    public async Task<ValidateSalesArrangementResponse> Validate(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var saType = await _formsService.LoadSalesArrangementType(salesArrangement.SalesArrangementTypeId, cancellationToken);

        var easForm = await GetEasForm(salesArrangement, (SalesArrangementCategories)saType.SalesArrangementCategory, cancellationToken);

        var checkForms = easForm.Forms.Select(f => new Eas.EasWrapper.CheckFormData
        {
            formular_id = GetFormId(f.EasFormType),
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

        return await _formsService.LoadProductForm(salesArrangement, await dynamicFormValues.ToListAsync(cancellationToken), isCancelled: false, cancellationToken);
    }

    private async Task<GetEasFormResponse> ProcessServiceRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicFormValues = await _formsService.CreateServiceDynamicFormValues(salesArrangement, cancellationToken);

        return await _formsService.LoadServiceForm(salesArrangement, dynamicFormValues, cancellationToken);
    }

    private async Task<List<ValidationMessage>> SendAndValidateForm(Eas.EasWrapper.CheckFormData checkFormData, CancellationToken cancellationToken)
    {
        var checkFormResult = await _easClient.CheckFormV2(checkFormData, cancellationToken);

        if (!ValidCommonValues.Contains(checkFormResult.CommonValue))
        {
            var message = $"Check form common error [CommonValue: {checkFormResult.CommonValue}, CommonText: {checkFormResult.CommonText}]";

            throw new CisValidationException(checkFormResult.CommonValue == 2 ? ErrorCodeMapper.SendAndValidateForm2 : ErrorCodeMapper.SendAndValidateForm1, message);
        }

        var transformationService = _transformationServiceFactory.CreateService(checkFormData.formular_id);

        return transformationService.TransformErrors(checkFormData.data, checkFormResult.Detail?.errors);
    }

    private static int GetFormId(EasFormType type)
    {
        return type switch
        {
            EasFormType.F3601 => 3601001,
            EasFormType.F3602 => 3602001,
            EasFormType.F3700 => 3700001,
            _ => 0
        };
    }
}