Update [dbo].[ServiceDiscovery] Set
    ServiceName = 'ES:C4MCreditWorthiness:V3',
    ServiceUrl = Case EnvironmentName 
                    When 'PREPROD' Then 'https://stage.risk-loan-assessment.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-3/api'
                    Else 'https://uat.risk-loan-assessment.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-3/api' End
    Where ServiceName = 'ES:C4MCreditWorthiness:V1'

  Update [dbo].[ServiceDiscovery] Set
    ServiceName = 'ES:C4MCustomerExposure:V3',
    ServiceUrl = Case EnvironmentName 
                    When 'PREPROD' Then 'https://stage.risk-loan-assessment.kbcloud/laa-loan-application-exposure-service-3/api'
                    Else 'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-exposure-service-3/api' End
    Where ServiceName = 'ES:C4MCustomersExposure:V1'

  Update [dbo].[ServiceDiscovery] Set
    ServiceName = 'ES:C4MLoanApplication:V3',
    ServiceUrl = Case EnvironmentName 
                    When 'PREPROD' Then 'https://stage.risk-loan-assessment.kbcloud/la-loan-application-hf-adapter-service-3/api'
                    Else 'https://uat.risk-loan-assessment.kbcloud/la-loan-application-hf-adapter-service-3/api' End
    Where ServiceName = 'ES:C4MLoanApplication:V1'

  Update [dbo].[ServiceDiscovery] Set
    ServiceName = 'ES:C4MLoanApplicationAssessment:V3',
    ServiceUrl = Case EnvironmentName 
                    When 'PREPROD' Then 'https://stage.risk-loan-assessment.kbcloud/laa-loan-application-assessment-service-3/api'
                    Else 'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-assessment-service-3/api' End
    Where ServiceName = 'ES:C4MLoanApplicationAssessment:V1'

  Update [dbo].[ServiceDiscovery] Set
    ServiceName = 'ES:C4MRiskBusinessCase:V3',
    ServiceUrl = Case EnvironmentName 
                    When 'PREPROD' Then 'https://stage.risk-loan-assessment.kbcloud/laa-risk-business-case-service-3/api'
                    Else 'https://uat.risk-loan-assessment.kbcloud/laa-risk-business-case-service-3/api' End
    Where ServiceName = 'ES:C4MRiskBusinessCase:V1'

  Update [dbo].[ServiceDiscovery] Set
    ServiceName = 'ES:C4MRiskCharacteristics:V2',
    ServiceUrl = Case EnvironmentName 
                    When 'PREPROD' Then 'https://stage.risk-loan-assessment.kbcloud/laa-loan-application-risk-chars-calculation-service-2/api'
                    Else 'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-risk-chars-calculation-service-2/api' End
    Where ServiceName = 'ES:C4MRiskCharakteristics:V1'