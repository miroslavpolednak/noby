using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms;
using ConfigurationManager = CIS.InternalServices.DataAggregatorService.Api.Configuration.ConfigurationManager;

namespace CIS.InternalServices.DataAggregatorService.Api.Endpoints.GetEasForm;

internal class GetEasFormHandler : IRequestHandler<GetEasFormRequest, GetEasFormResponse>
{
    private readonly ConfigurationManager _configurationManager;
    private readonly EasFormFactory _easFormFactory;

    public GetEasFormHandler(ConfigurationManager configurationManager, EasFormFactory easFormFactory)
    {
        _configurationManager = configurationManager;
        _easFormFactory = easFormFactory;
    }

    public async Task<GetEasFormResponse> Handle(GetEasFormRequest request, CancellationToken cancellationToken)
    {
        var config = await _configurationManager.LoadEasFormConfiguration((int)request.EasFormRequestType, cancellationToken);

        var easForm = await _easFormFactory.Create(request.SalesArrangementId, config, cancellationToken);

        var response = new GetEasFormResponse
        {
            Forms = { easForm.BuildForms(config.SourceFields, request.DynamicFormValues) }
        };

        easForm.SetFormResponseSpecificData(response);

        return response;
    }
}