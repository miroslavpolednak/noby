using ExternalServices.Eas.R21.EasWrapper;
using CIS.Infrastructure.Logging;

namespace ExternalServices.Eas.R21;

internal sealed class RealEasClient
    : Shared.BaseClient<RealEasClient>, IEasClient
{
    public async Task<long> GetCaseId(CIS.Foms.Enums.IdentitySchemes mandant, int productTypeId)
    {
        return await callMethod<long>(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();
            var request = new CaseIdRequest
            {
                mandant = (int)mandant,
                productCode = productTypeId
            };

            _logger.LogSerializedObject("CaseIdRequest", request);
            var result = await client.Get_CaseIdAsync(request);
            _logger.LogSerializedObject("CaseIdResponse", result);

            //TODO jak ma vypadat chyba vracena z EAS?
            if (result.commonResult?.return_val != 0)
            {
                var message = $"An error occurred when calling the EAS Get_CaseId function – {result.commonResult?.return_val ?? 0}: {result.commonResult?.return_text ?? "Unknown error"}";

                _logger.LogInformation(message);

                throw new CIS.Core.Exceptions.CisValidationException(9102, message);
            }

            return result.caseId;
        });
    }

    public async Task<ESBI_SIMULATION_RESULTS> RunSimulation(ESBI_SIMULATION_INPUT_PARAMETERS input)
    {
        return await callMethod<ESBI_SIMULATION_RESULTS>(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            _logger.LogSerializedObject("ESBI_SIMULATION_INPUT_PARAMETERS", input);
            var result = await client.SimulationAsync(input);
            _logger.LogSerializedObject("ESBI_SIMULATION_RESULTS", result);

            if (result.SIM_error != 0)
            {
                var message = $"Error occured during call external service EAS [{result.SIM_error} : {result.SIM_error_text}]";
                _logger.LogWarning(message);

                throw new CIS.Core.Exceptions.CisValidationException(9103, message);
            }

            return result;
        });
    }

    public async Task<Dto.CreateNewOrGetExisingClientResponse> CreateNewOrGetExisingClient(Dto.ClientDataModel clientData)
    {
        return await callMethod<Dto.CreateNewOrGetExisingClientResponse>(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            var request = new S_KLIENTDATA[] { clientData.MapToEas() };

            _logger.LogSerializedObject("GetKlientData_NewKlient request", request);
            var result = await client.GetKlientData_NewKlientAsync(request);

            if (result.GetKlientData_NewKlientResult is null || !result.GetKlientData_NewKlientResult.Any())
                throw new CIS.Core.Exceptions.CisValidationException(9104, "EAS GetKlientData_NewKlientResult is empty");

            var r = result.GetKlientData_NewKlientResult[0];
            _logger.LogSerializedObject("GetKlientData_NewKlientResponse", r);

            if (r.return_val != 0)
            {
                var message = $"Incorrect inputs to EAS NewKlient {r.return_val}: {r.return_info}";
                _logger.LogInformation(message);
                throw new CIS.Core.Exceptions.CisValidationException(9105, message);
            }

            var differentProps = ModelExtensions.FindDifferentProps(request[0], r);
            if (differentProps.Length > 0)
            {
                var message = $"Detected differences between input and output data during call EAS NewKlient [{String.Join(",",differentProps)}]";
                _logger.LogInformation(message);
                _auditLogger.Log(message);
            }

            return new Dto.CreateNewOrGetExisingClientResponse { Id = r.klient_id, BirthNumber = r.rodne_cislo_ico };
        });
    }

    public async Task<string> GetContractNumber(long clientId, int caseId)
    {
        _logger.LogDebug("Run inputs: {clientId}, {caseId}", clientId, caseId);

        return await callMethod<string>(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            var request = new ContractNrRequest(Convert.ToInt32(clientId), caseId);

            _logger.LogSerializedObject("ContractNrRequest", request);
            var result = await client.Get_ContractNumberAsync(request);
            _logger.LogSerializedObject("ContractNrResponse", result);

            return result.contractNumber;
        });
    }

    public async Task<CommonResponse?> AddFirstSignatureDate(int caseId, DateTime firstSignatureDate)
    {
        _logger.LogDebug("AddFirstSignatureDate inputs: CaseId: {caseId}, LoanId: {loanId}, FirstSignatureDate: {firstSignatureDate}", caseId, caseId, firstSignatureDate);

        return await callMethod<CommonResponse?>(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            var response = await client.Add_FirstSignatureDateAsync(caseId, caseId, firstSignatureDate);
            _logger.LogSerializedObject("AddFirstSignatureDate outputs: ", response.commonResult);

            return new CommonResponse(response.commonResult);

        });
    }

    public async Task<CheckFormV2.Response> CheckFormV2(CheckFormData formData)
    {
        return await callMethod(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            var request = new CheckFormV2Request(formData);

            _logger.LogSerializedObject("CheckFormV2Request", new { FormData = formData });
            var response = await client.CheckForm_V2Async(request);
            _logger.LogSerializedObject("CheckFormV2Response", new { CommonResult = response.commonResult, FormularData = response.formularData });

            //Console.WriteLine("REQ");
            //(new System.Xml.Serialization.XmlSerializer(request.GetType())).Serialize(Console.Out, request);
            //Console.WriteLine("RES");
            //(new System.Xml.Serialization.XmlSerializer(response.GetType())).Serialize(Console.Out, response);

            return new R21.CheckFormV2.Response(response.commonResult, response.formularData);
        });
    }

    public RealEasClient(CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<IEasClient> configuration, ILogger<RealEasClient> logger, CIS.Infrastructure.Telemetry.IAuditLogger auditLogger)
        : base(configuration, logger, auditLogger)
    {
    }

    private EAS_WS_SB_ServicesClient createClient()
        => new EAS_WS_SB_ServicesClient(createHttpBinding(), createEndpoint());
}
