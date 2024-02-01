ALTER TABLE SalesArrangement ADD FirstLoanAssessmentDate DATE NULL

GO

UPDATE SalesArrangement 
SET FirstLoanAssessmentDate = DATEADD(DAY, -90, RiskBusinessCaseExpirationDate) 
WHERE RiskBusinessCaseExpirationDate IS NOT NULL