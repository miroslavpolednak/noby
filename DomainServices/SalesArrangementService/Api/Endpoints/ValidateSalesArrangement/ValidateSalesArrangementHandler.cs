using DomainServices.SalesArrangementService.Contracts;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;
using ExternalServices.Eas.V1;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement;

internal class ValidateSalesArrangementHandler
    : IRequestHandler<ValidateSalesArrangementRequest, ValidateSalesArrangementResponse>
{
    private static readonly int[] ValidCommonValues = { 0, 6 };

    private readonly Services.ValidationTransformationServiceFactory _transformationServiceFactory;
    private readonly IEasClient _easClient;
    private readonly Services.Forms.FormsService _formsService;

    public ValidateSalesArrangementHandler(
        Services.ValidationTransformationServiceFactory transformationServiceFactory,
        IEasClient easClient,
        Services.Forms.FormsService formsService)
    {
        _transformationServiceFactory = transformationServiceFactory;
        _easClient = easClient;
        _formsService = formsService;
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
            SalesArrangementCategories.ServiceRequest => await _formsService.LoadServiceForm(salesArrangement.SalesArrangementId, Enumerable.Empty<DynamicFormValues>(), cancellationToken),
            _ => throw new NotImplementedException()
        };
    }

    private async Task<GetEasFormResponse> ProcessProductRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        //await _formsService.UpdateContractNumber(salesArrangement, cancellationToken);

        var response = await _formsService.LoadProductForm(salesArrangement, Enumerable.Empty<DynamicFormValues>(), cancellationToken);

        return response;
    }

    private async Task<ValidateSalesArrangementResponse> CheckForms(GetEasFormResponse easForm, CancellationToken cancellationToken)
    {
        var response = new ValidateSalesArrangementResponse();

        foreach (var form in easForm.Forms)
        {
            var checkFormData = new Eas.EasWrapper.CheckFormData
            {
                //formular_id = 3601001,
                //cislo_smlouvy = formData.Arrangement.ContractNumber,
                formular_id = GetFormId(form.EasFormType),
                cislo_smlouvy = easForm.ContractNumber,
                // dokument_id = "9876543210",                      // ??? dokument_id je nepovinné, to neposílej
                dokument_id = "9876543210",    // TODO: dočasný mock - odstranit až si to Assecco odladí
                datum_prijeti = DateTime.Now.Date,                         // ??? datum prijeti dej v D1.2 aktuální datum
                data = form.Json,
            };

            var checkFormResult = await _easClient.CheckFormV2(checkFormData, cancellationToken);

            if (!ValidCommonValues.Contains(checkFormResult.CommonValue))
            {
                var message = $"Check form common error [CommonValue: {checkFormResult.CommonValue}, CommonText: {checkFormResult.CommonText}]";
                throw new CisValidationException(checkFormResult.CommonValue == 2 ? 18041 : 18040, message);
            }

            var transformationService = _transformationServiceFactory.CreateService(checkFormData.formular_id);
            response.ValidationMessages.AddRange(transformationService.TransformErrors(form.Json, checkFormResult.Detail?.errors));
        }

        return response;

        static int GetFormId(EasFormType type)
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
}

