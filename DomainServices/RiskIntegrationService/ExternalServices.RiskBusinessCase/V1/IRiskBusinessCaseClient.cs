﻿using DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V1;

public interface IRiskBusinessCaseClient
    : IRiskBusinessCaseClientBase
{
    /// <summary>
    /// Vytvoření nového Risk Business Case
    /// </summary>
    Task<Contracts.RiskBusinessCase> CreateCase(Create request, CancellationToken cancellationToken);

    /// <summary>
    /// Vyhodnocení úvěrové žádosti
    /// </summary>
    Task<Identified> CreateCaseAssessment(string riskBusinessCaseId, LoanApplicationAssessmentCreate request, CancellationToken cancellationToken);

    /// <summary>
    /// Vyhodnocení úvěrové žádosti - asynchronní
    /// </summary>
    Task<RiskBusinessCaseCommitCommandInstance> CreateCaseAssessmentAsynchronous(string riskBusinessCaseId, RiskBusinessCaseCommitCommand request, CancellationToken cancellationToken);

    /// <summary>
    /// Dokončení úvěrové žádosti
    /// </summary>
    Task<RiskBusinessCaseCommit> CommitCase(string riskBusinessCaseId, RiskBusinessCaseCommitCreate request, CancellationToken cancellationToken);

    const string Version = "V1";
}
