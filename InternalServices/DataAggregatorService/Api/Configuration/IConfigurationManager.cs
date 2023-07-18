using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.RiskLoanApplication;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

internal interface IConfigurationManager
{
    Task<DocumentConfiguration> LoadDocumentConfiguration(DocumentKey documentKey,  CancellationToken cancellationToken);

    Task<EasFormConfiguration> LoadEasFormConfiguration(EasFormKey easFormKey, CancellationToken cancellationToken);
    Task<RiskLoanApplicationConfiguration> LoadRiskLoanApplicationConfiguration(CancellationToken cancellationToken);
}