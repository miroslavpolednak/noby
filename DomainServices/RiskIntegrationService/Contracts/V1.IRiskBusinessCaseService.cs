﻿namespace DomainServices.RiskIntegrationService.v1;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.v1.RiskBusinessCaseService")]
public interface IRiskBusinessCaseService
{
    ValueTask<Contracts.RiskBusinessCase.CreateCaseResponse> CreateCase(Contracts.RiskBusinessCase.CreateCaseRequest request, CancellationToken cancellationToken = default);

    ValueTask<Contracts.RiskBusinessCase.CaseCommitmentResponse> CaseCommitment(Contracts.RiskBusinessCase.CaseCommitmentRequest request, CancellationToken cancellationToken = default);
}
