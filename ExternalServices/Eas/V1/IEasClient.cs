using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.Eas.V1.EasWrapper;

namespace ExternalServices.Eas.V1;

public interface IEasClient
    : IExternalServiceClient
{
    const string Version = "V1";

    /// <summary>
    /// Pusti simulaci SS/Uv
    /// </summary>
    Task<ESBI_SIMULATION_RESULTS> RunSimulation(ESBI_SIMULATION_INPUT_PARAMETERS input, CancellationToken cancellationToken);

    /// <summary>
    /// Vytvori nove ID sporeni/hypo - novy CASE
    /// </summary>
    /// <exception cref="System.Exception">Jakakoliv interni chyba EAS</exception>
    Task<long> GetCaseId(CIS.Foms.Enums.IdentitySchemes mandant, int productTypeId, CancellationToken cancellationToken);

    /// <summary>
    /// Vytvori noveho klienta (rezervace partnerId)
    /// </summary>
    Task<Dto.CreateNewOrGetExisingClientResponse> CreateNewOrGetExisingClient(Dto.ClientDataModel clientData, CancellationToken cancellationToken);

    /// <summary>
    /// Vrací číslo smlouvy podle klienta a případu (case)
    /// </summary>
    Task<string> GetContractNumber(long clientId, int caseId, CancellationToken cancellationToken);

    /// <summary>
    /// Přidání data prvního podpisu
    /// </summary>
    Task<CommonResponse?> AddFirstSignatureDate(int caseId, DateTime firstSignatureDate, CancellationToken cancellationToken);

    /// <summary>
    /// Kontrola formuláře V2
    /// </summary>
    Task<CheckFormV2.Response> CheckFormV2(CheckFormData formData, CancellationToken cancellationToken);

}