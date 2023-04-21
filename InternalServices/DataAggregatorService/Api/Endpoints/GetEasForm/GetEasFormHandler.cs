using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms;

namespace CIS.InternalServices.DataAggregatorService.Api.Endpoints.GetEasForm;

internal class GetEasFormHandler : IRequestHandler<GetEasFormRequest, GetEasFormResponse>
{
    private readonly IConfigurationManager _configurationManager;
    private readonly EasFormFactory _easFormFactory;

    public GetEasFormHandler(IConfigurationManager configurationManager, EasFormFactory easFormFactory)
    {
        _configurationManager = configurationManager;
        _easFormFactory = easFormFactory;
    }

    public async Task<GetEasFormResponse> Handle(GetEasFormRequest request, CancellationToken cancellationToken)
    {
        var config = await _configurationManager.LoadEasFormConfiguration(GetEasFormKey(request), cancellationToken);

        var easForm = await _easFormFactory.Create(request.SalesArrangementId, request.UserId, config, cancellationToken);

        var response = new GetEasFormResponse
        {
            Forms = { easForm.BuildForms(request.DynamicFormValues, config.SourceFields) }
        };

        easForm.SetFormResponseSpecificData(response);

        return response;
    }

    private static EasFormKey GetEasFormKey(GetEasFormRequest request)
    {
        if (request.EasFormRequestType == EasFormRequestType.Product)
            return EasFormKey.ForProduct();

        return EasFormKey.ForService(EasFormTypeFactory.GetEasFormType(request.DynamicFormValues[0].DocumentTypeId));
    }
}