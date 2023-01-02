using Grpc.Core;
using DomainServices.SalesArrangementService.Contracts;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregator.Configuration.EasForm;
using CIS.InternalServices.DataAggregator.EasForms;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class ValidateSalesArrangementHandler
    : IRequestHandler<Dto.ValidateSalesArrangementMediatrRequest, ValidateSalesArrangementResponse>
{
    private static readonly int[] ValidCommonValues = { 0, 6 };

    private readonly Services.ValidationTransformationServiceFactory _transformationServiceFactory;
    private readonly Eas.IEasClient _easClient;
    private readonly Forms.FormsService _formsService;

    public ValidateSalesArrangementHandler(
        Services.ValidationTransformationServiceFactory transformationServiceFactory,
        Eas.IEasClient easClient,
        Forms.FormsService formsService)
    {
        _transformationServiceFactory = transformationServiceFactory;
        _easClient = easClient;
        _formsService = formsService;
    }

    public async Task<ValidateSalesArrangementResponse> Handle(Dto.ValidateSalesArrangementMediatrRequest request, CancellationToken cancellationToken)
    {
        var category = await _formsService.LoadSalesArrangementCategory(request.SalesArrangementId, cancellationToken);

        var easForm = await GetEasForm(request.SalesArrangementId, category, cancellationToken);

        return await CheckForms(easForm);
    }

    private async Task<IEasForm<IEasFormData>> GetEasForm(int salesArrangementId, SalesArrangementCategories category, CancellationToken cancellationToken)
    {
        return category switch
        {
            SalesArrangementCategories.ProductRequest => await ProcessProductRequest(salesArrangementId, cancellationToken),
            SalesArrangementCategories.ServiceRequest => await _formsService.LoadServiceForm(salesArrangementId),
            _ => throw new NotImplementedException()
        };
    }

    private async Task<IEasForm<IEasFormData>> ProcessProductRequest(int salesArrangementId, CancellationToken cancellationToken)
    {
        var productForm = await _formsService.LoadProductForm(salesArrangementId);

        await _formsService.UpdateContractNumber(productForm.FormData, cancellationToken);

        return productForm;
    }

    private async Task<ValidateSalesArrangementResponse> CheckForms(IEasForm<IEasFormData> easForm)
    {
        var response = new ValidateSalesArrangementResponse();

        foreach (var form in easForm.BuildForms(Enumerable.Empty<DynamicFormValues>()))
        {
            var checkFormData = new Eas.EasWrapper.CheckFormData
            {
                //formular_id = 3601001,
                //cislo_smlouvy = formData.Arrangement.ContractNumber,
                formular_id = GetFormId(form.FormType),
                cislo_smlouvy = easForm.FormData.SalesArrangement.ContractNumber,
                // dokument_id = "9876543210",                      // ??? dokument_id je nepovinné, to neposílej
                dokument_id = easForm.FormData.MockValues.MockDocumentId,           // TODO: dočasný mock - odstranit až si to Assecco odladí
                datum_prijeti = DateTime.Now.Date,                         // ??? datum prijeti dej v D1.2 aktuální datum
                data = form.Json,
            };

            var checkFormResult = await _easClient.CheckFormV2(checkFormData);

            if (!ValidCommonValues.Contains(checkFormResult.CommonValue))
            {
                var message = $"Check form common error [CommonValue: {checkFormResult.CommonValue}, CommonText: {checkFormResult.CommonText}]";
                if (checkFormResult.CommonValue == 2)
                {
                    throw new CisValidationException(18041, message);
                }
                else
                {
                    throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, message, 18040);
                }
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

