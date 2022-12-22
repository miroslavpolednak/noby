using Grpc.Core;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.SalesArrangementService.Api.Handlers.Forms;
using CIS.Foms.Enums;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class ValidateSalesArrangementHandler
    : IRequestHandler<Dto.ValidateSalesArrangementMediatrRequest, ValidateSalesArrangementResponse>
{

    #region Construction

    private readonly Services.ValidationTransformationServiceFactory _transformationServiceFactory;
    private readonly FormsService _formsService;
    private readonly ILogger<ValidateSalesArrangementHandler> _logger;
    private readonly Eas.IEasClient _easClient;

    public ValidateSalesArrangementHandler(
        Services.ValidationTransformationServiceFactory transformationServiceFactory,
        FormsService formsService,
        ILogger<ValidateSalesArrangementHandler> logger,
        Eas.IEasClient easClient)
    {
        _transformationServiceFactory = transformationServiceFactory;
        _formsService = formsService;
        _logger = logger;
        _easClient = easClient;
    }

    #endregion

    private static int[] ValidCommonValues = new int[] { 0, 6 };

    public async Task<ValidateSalesArrangementResponse> Handle(Dto.ValidateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        // load arrangement
        var arrangement = await _formsService.LoadArrangement(request.SalesArrangementId, cancellation);

        // build forms
        var forms = await GetForms(arrangement, cancellation);

        // check forms
        return await CheckForms(forms, arrangement.ContractNumber);
    }

    private async Task<List<Form>> GetForms(Contracts.SalesArrangement arrangement, CancellationToken cancellation)
    {
        var arrangementCategory = await _formsService.LoadArrangementCategory(arrangement, cancellation);

        List<Form>? forms;

        switch (arrangementCategory)
        {
            case SalesArrangementCategories.ProductRequest:
                forms = await ProcessProductRequest(arrangement, cancellation);
                break;

            case SalesArrangementCategories.ServiceRequest:
                forms = await ProcessServiceRequest(arrangement, cancellation);
                break;

            default:
                forms = new List<Form>();
                break;
        }

        return forms;
    }

    private async Task<List<Form>> ProcessProductRequest(Contracts.SalesArrangement arrangement, CancellationToken cancellation)
    {
        var formData = await _formsService.LoadProductFormData(arrangement, cancellation);
        FormsService.CheckFormData(formData);
        await _formsService.SetContractNumber(formData, cancellation);
        var builder = new JsonBuilder();
        return builder.BuildForms(formData);  // ??? HH jaké FormId použít pro CheckForm 
    }

    private async Task<List<Form>> ProcessServiceRequest(Contracts.SalesArrangement arrangement, CancellationToken cancellation)
    {
        var formData = await _formsService.LoadServiceFormData(arrangement, cancellation);
        FormsService.CheckFormData(formData);
        var builder = new JsonBuilder();
        return builder.BuildForms(formData);  // ??? HH jaké FormId použít pro CheckForm 
    }

    private async Task<ValidateSalesArrangementResponse> CheckForms(List<Form> forms, string contractNumber)
    {
        int GetFormularId(EFormType type) {
            switch (type)
            {
                case EFormType.F3601: return 3601001;
                case EFormType.F3602: return 3602001;
                case EFormType.F3700: return 3700001;
            }
            return 0;
        }

        var actualDate = DateTime.Now.Date;

        var response = new ValidateSalesArrangementResponse { };

        for (int i = 0; i < forms.Count; i++)
        {
            var form = forms[i];
            
            var checkFormData = new Eas.EasWrapper.CheckFormData()
            {
                //formular_id = 3601001,
                //cislo_smlouvy = formData.Arrangement.ContractNumber,
                formular_id = GetFormularId(form.FormType),
                cislo_smlouvy = contractNumber,
                // dokument_id = "9876543210",                      // ??? dokument_id je nepovinné, to neposílej
                dokument_id = JsonBuilder.MockDokumentId,           // TODO: dočasný mock - odstranit až si to Assecco odladí
                datum_prijeti = actualDate,                         // ??? datum prijeti dej v D1.2 aktuální datum
                data = form.JSON,
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
            response.ValidationMessages.AddRange(transformationService.TransformErrors(form, checkFormResult.Detail?.errors));
        }

        return response;
    }
}

