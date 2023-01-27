using ExternalServices.Eas.R21.EasWrapper;

namespace ExternalServices.Eas.R21;

internal sealed class MockEasClient 
    : IEasClient
{
    public Task<ESBI_SIMULATION_RESULTS> RunSimulation(ESBI_SIMULATION_INPUT_PARAMETERS input)
    {
        return Task.FromResult<ESBI_SIMULATION_RESULTS>(new ESBI_SIMULATION_RESULTS
        {
            //savings
            SS_doba_sporeni_v_mesicich = 180,
            SS_datum_ukonceni_smlouvy = "1.5.2035",
            SS_urokova_sazba = 3.5M,
            SS_celkem_nasporeno = 58947,
            SS_vklady_celkem = 45050,
            SS_uhrady_a_poplatky_celkem = 5780,
            SS_uroky_celkem = 8552,
            SS_urokove_zvyhodneni = 500,
            SS_statni_podpora_celkem = 12000,
            SS_evidovana_statni_podpora = 1000,
            SS_predpokladany_datum_prideleni = "1.2.2025",
            SS_zostatok_vkladu = 400,
            SS_uroky_z_benefitu = 780,
            SS_uroky_z_benefitu_dan = 300,
            // loan
            USS_datum_prideleni = "1.2.2025",
            USS_datum_posledni_splatky = "1.2.2025",
            USS_vyse_uveru = 300000,
            PU_urokova_sazba = 5.4M,
            USS_mesicni_splatka = 3000,
            USS_poplatky_za_vedeni_uctu_celkem = 1500,
            USS_uroky_celkem = 24500,
            RPSN_pu_uss = 6
        });
    }

    public Task<long> GetCaseId(CIS.Foms.Enums.IdentitySchemes mandant, int productTypeId)
    {
        Random random = new Random();
        return Task.FromResult<long>(random.NextInt64(1, 999));
    }

    public Task<Dto.CreateNewOrGetExisingClientResponse> CreateNewOrGetExisingClient(Dto.ClientDataModel clientData)
    {
        return Task.FromResult(new Dto.CreateNewOrGetExisingClientResponse() { Id = 123 });
    }

    public Task<string> GetContractNumber(long clientId, int caseId)
    {
        return Task.FromResult<string>($"{clientId}_{caseId}");
    }

    public Task<CommonResponse?> AddFirstSignatureDate(int caseId, DateTime firstSignatureDate)
    {
        return null;
    }

    public Task<CheckFormV2.Response> CheckFormV2(CheckFormData formData)
    {
        return null;
    }
}
