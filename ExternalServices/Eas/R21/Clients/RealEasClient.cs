using ExternalServices.Eas.R21.EasWrapper;

namespace ExternalServices.Eas.R21;

internal sealed class RealEasClient 
    : Shared.BaseClient<RealEasClient>, IEasClient
{
    public async Task<IServiceCallResult> GetSavingsLoanId(long caseId)
    {
        _logger.LogDebug("Run inputs: {caseId}", caseId);

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

            var result = await client.Get_CaseIdAsync(new CaseIdRequest
            {
                mandant = (int)mandant,
                productCode = productTypeId
            });

            //TODO jak ma vypadat chyba vracena z EAS?
            if (result.commonResult?.return_val != 0)
                return new ErrorServiceCallResult(0, result.commonResult?.return_text ?? "Unknown error");
            else
                return new SuccessfulServiceCallResult<long>(result.caseId);
        });
    }

    public async Task<IServiceCallResult> RunSimulation(ESBI_SIMULATION_INPUT_PARAMETERS input)
    {
        _logger.LogDebug("Run inputs: {input}", System.Text.Json.JsonSerializer.Serialize(input));

        return await callMethod(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();
        
            var result = await client.SimulationAsync(input);

            if (result.SIM_error != 0)
                _logger.LogInformation("Incorrect inputs to EAS Simulation {error}: {errorText}", result.SIM_error, result.SIM_error_text);
            else
                _logger.LogDebug("Run outputs: {output}", System.Text.Json.JsonSerializer.Serialize(result));

            return new SuccessfulServiceCallResult<ESBI_SIMULATION_RESULTS>(result);
        });
    }

    public async Task<IServiceCallResult> CreateNewOrGetExisingClient(Dto.ClientDataModel clientData)
    {
        _logger.LogDebug("Run inputs: {input}", System.Text.Json.JsonSerializer.Serialize(clientData));

        return await callMethod(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();

            var result = await client.GetKlientData_NewKlientAsync(new S_KLIENTDATA[] { clientData.MapToEas() });

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
                _logger.LogDebug("Run outputs: {output}", System.Text.Json.JsonSerializer.Serialize(result));

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
        _logger.LogDebug("Run inputs: {clientId}, {caseId}", caseId);

        return await callMethod(async () =>
        {
            using EAS_WS_SB_ServicesClient client = createClient();
            var result = await client.Get_ContractNumberAsync(new ContractNrRequest(clientId, caseId));
            return new SuccessfulServiceCallResult<string>(result.contractNumber);
        });
    }

    public RealEasClient(EasConfiguration configuration, ILogger<RealEasClient> logger)
        : base(Versions.R21, configuration, logger)
    {
    }

    private EAS_WS_SB_ServicesClient createClient()
        => new EAS_WS_SB_ServicesClient(createHttpBinding(), createEndpoint());
}
