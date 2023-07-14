UPDATE RiskLoanApplicationSpecialDataField SET FieldPath = 'Households[].Customers[].Incomes.Employments[].Income.Employer.CountryId' WHERE JsonPropertyName = 'Households[].Customers[].Income.EmploymentIncomes[].CountryId'
UPDATE RiskLoanApplicationSpecialDataField SET FieldPath = 'Households[].Customers[].Incomes.Employments[].Income.Job.JobDescription' WHERE JsonPropertyName = 'Households[].Customers[].Income.EmploymentIncomes[].JobDescription'

INSERT INTO RiskLoanApplicationSpecialDataField VALUES ('Product.InvestmentAmount', 3, 'InvestmentAmount');
INSERT INTO RiskLoanApplicationSpecialDataField VALUES ('Product.MarketingActions[]', 3, 'MarketingActions[]');
INSERT INTO RiskLoanApplicationSpecialDataField VALUES ('Product.Collaterals[].AppraisedValue.Amount', 3, 'Collaterals[].Amount');
INSERT INTO RiskLoanApplicationSpecialDataField VALUES ('AppendixCode', 2, 'AppendixCode');
INSERT INTO RiskLoanApplicationSpecialDataField VALUES ('LoanApplicationDataVersion', 2, 'LoanApplicationVersion');