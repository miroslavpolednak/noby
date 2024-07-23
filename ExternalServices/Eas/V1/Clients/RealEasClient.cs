﻿using CIS.Core.Extensions;
using SharedTypes.Enums;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using ExternalServices.Eas.Dto;
using ExternalServices.Eas.V1.CheckFormV2;
using ExternalServices.Eas.V1.EasWrapper;
using System.ServiceModel.Channels;
using CIS.Core.Exceptions;
using CIS.Infrastructure.ExternalServicesHelpers.Soap;

namespace ExternalServices.Eas.V1;

internal sealed class RealEasClient : SoapClientBase<EAS_WS_SB_ServicesClient, IEAS_WS_SB_Services>, IEasClient
{
    private readonly ILogger<RealEasClient> _logger;

    public RealEasClient(
        ILogger<RealEasClient> logger,
        IExternalServiceConfiguration<IEasClient> configuration)
        : base(configuration, logger)
    {
        _logger = logger;
    }

    protected override string ServiceName => StartupExtensions.ServiceName;

    public async Task<CommonResponse?> AddFirstSignatureDate(int caseId, DateTime firstSignatureDate, CancellationToken cancellationToken)
    {
        return await callMethod<CommonResponse?>(async () =>
        {

            var response = await Client.Add_FirstSignatureDateAsync(caseId, caseId, firstSignatureDate.Date)
            .WithCancellation(cancellationToken);

            return new CommonResponse(response.commonResult);
        });
    }

    public async Task<Response> CheckFormV2(CheckFormData formData, CancellationToken cancellationToken)
    {
        return await callMethod(async () =>
        {

            var request = new CheckFormV2Request(formData);
            var response = await Client.CheckForm_V2Async(request).WithCancellation(cancellationToken);
            return new Response(response.commonResult, response.formularData);
        });
    }

    public async Task<CreateNewOrGetExisingClientResponse> CreateNewOrGetExisingClient(ClientDataModel clientData, CancellationToken cancellationToken)
    {
        return await callMethod(async () =>
        {
            var request = new S_KLIENTDATA[] { clientData.MapToEas() };
            var result = await Client.GetKlientData_NewKlientAsync(request).WithCancellation(cancellationToken);

            if (result.GetKlientData_NewKlientResult is null || result.GetKlientData_NewKlientResult.Length == 0)
                throw new CIS.Core.Exceptions.CisValidationException(9104, "EAS GetKlientData_NewKlientResult is empty");

            var r = result.GetKlientData_NewKlientResult[0];

            if (r.return_val != 0)
            {
                throw new CIS.Core.Exceptions.CisValidationException(9105, $"Incorrect inputs to EAS NewKlient {r.return_val}: {r.return_info}");
            }

            var differentProps = ModelExtensions.FindDifferentProps(request[0], r);
            if (differentProps.Length > 0)
            {
                var message = $"Detected differences between input and output data during call EAS NewKlient [{String.Join(",", differentProps)}]";
                _logger.ExternalServiceResponseError(message);
            }

            return new CreateNewOrGetExisingClientResponse { Id = r.klient_id, BirthNumber = r.rodne_cislo_ico };
        });
    }

    public async Task<long> GetCaseId(IdentitySchemes mandant, int productTypeId, CancellationToken cancellationToken)
    {
        return await callMethod(async () =>
        {
            var request = new CaseIdRequest
            {
                mandant = (int)mandant,
                productCode = productTypeId
            };

            var result = await Client.Get_CaseIdAsync(request).WithCancellation(cancellationToken);

            //TODO jak ma vypadat chyba vracena z EAS?
            if (result.commonResult?.return_val != 0)
            {
                throw new CIS.Core.Exceptions.CisValidationException(9102, $"An error occurred when calling the EAS Get_CaseId function – {result.commonResult?.return_val ?? 0}: {result.commonResult?.return_text ?? "Unknown error"}");
            }

            return result.caseId;
        });
    }

    public async Task<string> GetContractNumber(long clientId, int caseId, CancellationToken cancellationToken)
    {
        return await callMethod(async () =>
        {
            var request = new ContractNrRequest(Convert.ToInt32(clientId), caseId);
            var result = await Client.Get_ContractNumberAsync(request).WithCancellation(cancellationToken);
            return result.contractNumber;
        });
    }

    public async Task<BuildingSavingsResponse> SimulateBuildingSavings(BuildingSavingsRequest request, CancellationToken cancellationToken)
    {
        return await callMethod(async () =>
        {
            var result = await Client.SimulationAsync(request.ToEasRequest()).WithCancellation(cancellationToken);

            if (result.SIM_error != 0)
            {
                Logger.ExternalServiceResponseError($"Error occured during call external service EAS [{result.SIM_error} : {result.SIM_error_text}]");
                throw new CisValidationException(0, result.SIM_error_text);
            }
            
            return BuildingSavingsResponse.CreateInstance(result);
        });
    }

    protected override Binding CreateBinding()
    {
        var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        if (Configuration.RequestTimeout.HasValue)
        {
            basicHttpBinding.SendTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout.Value);
            basicHttpBinding.CloseTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout.Value);
        }
        basicHttpBinding.MaxReceivedMessageSize = 1500000;
        basicHttpBinding.ReaderQuotas.MaxArrayLength = 1500000;

        return basicHttpBinding;
    }
}
