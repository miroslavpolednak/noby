using ExternalServices.Eas.R21.EasWrapper;
using CIS.Infrastructure.Logging;

namespace ExternalServices.Eas.R21;

internal sealed class RealEasClient 
    : Shared.BaseClient<RealEasClient>, IEasClient
{
    public async Task<IServiceCallResult> GetSavingsLoanId(long caseId)
    {
        return await callMethod(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();
            
            //TODO az bude metoda, tak zavolat 
            /*if (result.SIM_error != 0)
                _logger.LogInformation("Unable to create MktItem instance in Starbuild: {error}: {errorText}", result.SIM_error, result.SIM_error_text);*/

            return new SuccessfulServiceCallResult<long>(caseId);
        });
    }

    public async Task<IServiceCallResult> GetCaseId(CIS.Foms.Enums.IdentitySchemes mandant, int productTypeId)
    {
        return await callMethod(async () =>
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
                return new ErrorServiceCallResult(0, result.commonResult?.return_text ?? "Unknown error");
            else
                return new SuccessfulServiceCallResult<long>(result.caseId);
        });
    }

    public async Task<IServiceCallResult> RunSimulation(ESBI_SIMULATION_INPUT_PARAMETERS input)
    {
        return await callMethod(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            _logger.LogSerializedObject("ESBI_SIMULATION_INPUT_PARAMETERS", input);
            var result = await client.SimulationAsync(input);
            _logger.LogSerializedObject("ESBI_SIMULATION_RESULTS", result);

            if (result.SIM_error != 0)
            {
                var message = $"Error occured during call external service EAS [{result.SIM_error} : {result.SIM_error_text}]";
                _logger.LogWarning(message);
                return new ErrorServiceCallResult(99999, message); //TODO: error code
            }

            return new SuccessfulServiceCallResult<ESBI_SIMULATION_RESULTS>(result);
        });
    }

    public async Task<IServiceCallResult> CreateNewOrGetExisingClient(Dto.ClientDataModel clientData)
    {
        return await callMethod(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            var request = new S_KLIENTDATA[] { clientData.MapToEas() };

            _logger.LogSerializedObject("S_KLIENTDATA[]", request);
            var result = await client.GetKlientData_NewKlientAsync(request);
            
            if (result.GetKlientData_NewKlientResult is null || !result.GetKlientData_NewKlientResult.Any())
                return new ErrorServiceCallResult(0, "EAS GetKlientData_NewKlientResult is empty");

            var r = result.GetKlientData_NewKlientResult[0];
            if (r.return_val != 0)
            {
                _logger.LogInformation("Incorrect inputs to EAS NewKlient {error}: {errorText}", r.return_val, r.return_info);

                return new ErrorServiceCallResult(r.return_val, r.return_info);
            }
            else
            {
                _logger.LogSerializedObject("GetKlientData_NewKlientResponse", r);

                return new SuccessfulServiceCallResult<Dto.CreateNewOrGetExisingClientResponse>(new Dto.CreateNewOrGetExisingClientResponse
                {
                    Id = r.klient_id,
                    BirthNumber = r.rodne_cislo_ico
                });
            }
        });
    }

    public async Task<IServiceCallResult> GetContractNumber(int clientId, int caseId)
    {
        _logger.LogDebug("Run inputs: {clientId}, {caseId}", clientId, caseId);

        return await callMethod(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            var request = new ContractNrRequest(clientId, caseId);

            _logger.LogSerializedObject("ContractNrRequest", request);
            var result = await client.Get_ContractNumberAsync(request);
            _logger.LogSerializedObject("ContractNrResponse", result);

            return new SuccessfulServiceCallResult<string>(result.contractNumber);
        });
    }

    public async Task<IServiceCallResult> AddFirstSignatureDate(int caseId, int loanId, DateTime firstSignatureDate)
    {
        _logger.LogDebug("Run inputs: {caseId}, {loanId}, {firstSignatureDate}", caseId, loanId, firstSignatureDate);

        return await callMethod(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            var response = await client.Add_FirstSignatureDateAsync(caseId, loanId, firstSignatureDate);
            _logger.LogSerializedObject("AddFirstSignatureDate response", response);

            return new SuccessfulServiceCallResult();
        });
    }

    public async Task<IServiceCallResult> CheckForm(S_FORMULAR formular)
    {
        return await callMethod(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            var request = new CheckFormRequest(formular);

            _logger.LogSerializedObject("CheckFormRequest", request);
            var response = await client.CheckFormAsync(request);
            _logger.LogSerializedObject("CheckFormResponse", response);

            return new SuccessfulServiceCallResult<int>(response.CheckFormResult);
        });
    }

    public RealEasClient(EasConfiguration configuration, ILogger<RealEasClient> logger)
        : base(Versions.R21, configuration, logger)
    {
    }

    private EAS_WS_SB_ServicesClient createClient()
        => new EAS_WS_SB_ServicesClient(createHttpBinding(), createEndpoint());
}
