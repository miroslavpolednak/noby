﻿using CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Configuration;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;

namespace CIS.InternalServices.DataAggregatorService.Api.Endpoints.GetRiskLoanApplicationData;

internal class GetRiskLoanApplicationDataHandler : IRequestHandler<GetRiskLoanApplicationDataRequest, GetRiskLoanApplicationDataResponse>
{
    private readonly IServiceConfigurationManager _configurationManager;
    private readonly DataServicesLoader _dataServicesLoader;
    private readonly RiskLoanApplicationData _data;

    public GetRiskLoanApplicationDataHandler(IServiceConfigurationManager configurationManager, DataServicesLoader dataServicesLoader, RiskLoanApplicationData data)
    {
        _configurationManager = configurationManager;
        _dataServicesLoader = dataServicesLoader;
        _data = data;
    }

    public async Task<GetRiskLoanApplicationDataResponse> Handle(GetRiskLoanApplicationDataRequest request, CancellationToken cancellationToken)
    {
        var config = await _configurationManager.LoadRiskLoanApplicationConfiguration(cancellationToken);

        var inputParameters = new InputParameters
        {
            SalesArrangementId = request.SalesArrangementId,
            OfferId = request.OfferId,
            CaseId = request.CaseId
        };

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, _data, cancellationToken);

        var jsonObject = new JsonBuilder<RiskLoanApplicationJsonValueSource>();

        foreach (var sourceField in config.SourceFields) 
            jsonObject.Add(sourceField.JsonPropertyName, sourceField);

        return new GetRiskLoanApplicationDataResponse
        {
            Json = jsonObject.Serialize(_data)
        };
    }
}