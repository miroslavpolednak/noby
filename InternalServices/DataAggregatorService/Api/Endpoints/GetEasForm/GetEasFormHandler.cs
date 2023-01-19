using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms;

namespace CIS.InternalServices.DataAggregatorService.Api.Endpoints.GetEasForm;

internal class GetEasFormHandler : IRequestHandler<GetEasFormRequest, GetEasFormResponse>
{
    private readonly Configuration.ConfigurationManager _configurationManager;
    private readonly EasFormFactory _easFormFactory;

    public GetEasFormHandler(Configuration.ConfigurationManager configurationManager, EasFormFactory easFormFactory)
    {
        _configurationManager = configurationManager;
        _easFormFactory = easFormFactory;
    }

    public async Task<GetEasFormResponse> Handle(GetEasFormRequest request, CancellationToken cancellationToken)
    {
        var config = await _configurationManager.LoadEasFormConfiguration((int)request.EasFormRequestType);

        var easForm = await _easFormFactory.Create(request.SalesArrangementId, config, cancellationToken);

        return new GetEasFormResponse
        {
            Forms = { easForm.BuildForms(config.SourceFields, request.DynamicFormValues) }
        };
    }
}