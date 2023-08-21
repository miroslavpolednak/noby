namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

internal interface IConfigurationManager
{
    Task<Document.DocumentConfiguration> LoadDocumentConfiguration(Document.DocumentKey documentKey,  CancellationToken cancellationToken);

    Task<EasForm.EasFormConfiguration> LoadEasFormConfiguration(EasForm.EasFormKey easFormKey, CancellationToken cancellationToken);
    Task<ConfigurationBase<RiskLoanApplication.RiskLoanApplicationSourceField>> LoadRiskLoanApplicationConfiguration(CancellationToken cancellationToken);
}