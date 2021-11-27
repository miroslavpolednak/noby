using ExternalServices.Eas.R21.EasWrapper;

namespace ExternalServices.Eas.R21;

public interface IEasClient
{
    Versions Version { get; }

    /// <summary>
    /// Vytvori nove UverId pro dane ID sporeni
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[long] -> nove UverId (ProductInstanceId)
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
    /// </returns>
    Task<IServiceCallResult> GetCaseId(CIS.Core.MandantTypes mandant, int productInstanceType);
}
