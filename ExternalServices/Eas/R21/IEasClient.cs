using ExternalServices.Eas.R21.EasWrapper;

namespace ExternalServices.Eas.R21;

public interface IEasClient
{
    Versions Version { get; }

    /// <summary>
    /// Vytvori nove UverId pro dane ID sporeni
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[long] -> nove UverId (ProductId)
    /// </returns>
    Task<IServiceCallResult> GetSavingsLoanId(long caseId);

    /// <summary>
    /// Pusti simulaci SS/Uv
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[ESBI_SIMULATION_RESULTS]
    /// </returns>
    Task<IServiceCallResult> RunSimulation(ESBI_SIMULATION_INPUT_PARAMETERS input);

    /// <summary>
    /// Vytvori nove ID sporeni/hypo - novy CASE
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[long] -> nove SporeniId (CaseId)
    /// ErrorServiceCallResult(10009, $"EAS Endpoint '{}' unavailable")
    /// ErrorServiceCallResult(10008, $"EAS Endpoint '{}' not found)
    /// </returns>
    /// <exception cref="System.Exception">Jakakoliv interni chyba EAS</exception>
    Task<IServiceCallResult> GetCaseId(CIS.Foms.Enums.IdentitySchemes mandant, int productTypeId);

    /// <summary>
    /// Vytvori noveho klienta (rezervace partnerId)
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[S_KLIENTDATA]
    /// </returns>
    Task<IServiceCallResult> NewKlient(S_KLIENTDATA client);
}
