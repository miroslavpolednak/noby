ALTER TABLE [RiskLoanApplicationDataField] ADD UseDefaultInsteadOfNull BIT NOT NULL DEFAULT 0
ALTER TABLE [RiskLoanApplicationSpecialDataField] ADD UseDefaultInsteadOfNull BIT NOT NULL DEFAULT 0

GO

UPDATE RiskLoanApplicationSpecialDataField SET UseDefaultInsteadOfNull = 1 WHERE JsonPropertyName = 'Households[].PropertySettlementId'
UPDATE RiskLoanApplicationSpecialDataField SET UseDefaultInsteadOfNull = 1 WHERE JsonPropertyName = 'Households[].ChildrenOverTenYearsCount'
UPDATE RiskLoanApplicationSpecialDataField SET UseDefaultInsteadOfNull = 1 WHERE JsonPropertyName = 'Households[].ChildrenUpToTenYearsCount'