UPDATE EasFormSpecialDataField
SET FieldPath = 'HouseholdData.Customers[].IncomesEmployment[].IncomeConfirmationContact'
WHERE JsonPropertyName = 'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_potvrzeni_kontakt'

UPDATE RiskLoanApplicationSpecialDataField 
SET FieldPath = 'Households[].Customers[].Incomes.Employments[].IncomeConfirmationContact'
WHERE JsonPropertyName = 'Households[].Customers[].Income.EmploymentIncomes[].ConfirmationContactPhone'