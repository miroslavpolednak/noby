IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'DDS' )
BEGIN
EXEC sp_executesql N'CREATE SCHEMA DDS'
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DDS].[Income]') AND type in (N'U'))
ALTER TABLE [DDS].[Income] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [DDS].[Income]
GO
DROP TABLE IF EXISTS [DDS].[IncomeHistory]
GO
CREATE TABLE [DDS].[Income](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [int] NOT NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL
 CONSTRAINT [PK_Income] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
	SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[IncomeHistory])
)
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[Income]
GO
CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[Income]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET IDENTITY_INSERT [DDS].[Income] ON;
GO

INSERT INTO [DDS].[Income]
(DocumentDataStorageId,[DocumentDataEntityId],[DocumentDataVersion],[CreatedUserId],[CreatedTime],[ModifiedUserId],[Data])
select CustomerOnSAIncomeId, CustomerOnSAId, 1, CreatedUserId, CreatedTime, ModifiedUserId, 
	'{"IncomeTypeId":'+cast([IncomeTypeId] as varchar(10))+',"Sum":'+cast(isnull(Sum,0) as varchar(20))+',"CurrencyCode":"'+isnull(CurrencyCode,'')+'","IncomeSource":"'+isnull(IncomeSource,'')+'","HasProofOfIncome":'+(case when HasProofOfIncome=1 then 'true' else'false' end)
	+ (case [IncomeTypeId]
	when 2 then ',"Entrepreneur":'+Data
	when 4 then ',"Other":'+Data
	else ''
	end)
	+'}'
	from dbo.CustomerOnSAIncome
	where IncomeTypeId!=1
GO

INSERT INTO [DDS].[Income]
(DocumentDataStorageId,[DocumentDataEntityId],[DocumentDataVersion],[CreatedUserId],[CreatedTime],[ModifiedUserId],[Data])
select A.CustomerOnSAIncomeId, A.CustomerOnSAId, 1, A.CreatedUserId, A.CreatedTime, A.ModifiedUserId, 
	'{"IncomeTypeId":'+cast([IncomeTypeId] as varchar(10))+',"Sum":'+cast(isnull(Sum,0) as varchar(20))+',"CurrencyCode":"'+isnull(CurrencyCode,'')+'","IncomeSource":"'+isnull(IncomeSource,'')+'","HasProofOfIncome":'+(case when A.HasProofOfIncome=1 then 'true' else'false' end)+',"Employement":'
	+'{"ForeignIncomeTypeId":'+isnull(cast(D.ForeignIncomeTypeId as varchar(20)), 'null')+',"HasProofOfIncome":'+(case when D.HasProofOfIncome=1 then 'true' else 'false' end)+',"Employer":{"Name":"'+isnull(Employer_Name,'')+'","BirthNumber":"'+isnull(Employer_BirthNumber,'')+'","Cin":"'+isnull(Employer_Cin,'')+'","CountryId":'+isnull(cast(Employer_CountryId as varchar(10)),'null')+'},"Job":{"GrossAnnualIncome":'+isnull(cast(Job_GrossAnnualIncome as varchar(20)),'null')+',"JobDescription":"'+isnull(Job_JobDescription,'')+'","IsInProbationaryPeriod":'+(case when Job_IsInProbationaryPeriod=1 then 'true' else 'false' end)+',"IsInTrialPeriod":'+(case when Job_IsInTrialPeriod=1 then 'true' else 'false' end)+',"EmploymentTypeId":'+isnull(cast(Job_EmploymentTypeId as varchar(20)),'null')+',"CurrentWorkContractSince":'+(case when Job_CurrentWorkContractSince_y is null then 'null' else '"'+cast(Job_CurrentWorkContractSince_y as varchar(4))+'-'+FORMAT(Job_CurrentWorkContractSince_m, 'D2')+'-'+FORMAT(Job_CurrentWorkContractSince_d, 'D2')+'T00:00:00"' end)+',"CurrentWorkContractTo":'+(case when Job_CurrentWorkContractTo_y is null then 'null' else '"'+cast(Job_CurrentWorkContractTo_y as varchar(4))+'-'+FORMAT(Job_CurrentWorkContractTo_m, 'D2')+'-'+FORMAT(Job_CurrentWorkContractTo_d, 'D2')+'T00:00:00"' end)+',"FirstWorkContractSince":'+(case when Job_FirstWorkContractSince_y is null then 'null' else '"'+cast(Job_FirstWorkContractSince_y as varchar(4))+'-'+FORMAT(Job_FirstWorkContractSince_m, 'D2')+'-'+FORMAT(Job_FirstWorkContractSince_d, 'D2')+'T00:00:00"' end)+'},"HasWageDeduction":'+(case when HasWageDeduction=1 then 'true' else 'false' end)+',"WageDeduction":{"DeductionDecision":'+isnull(cast(WageDeduction_DeductionDecision as varchar(20)), 'null')+',"DeductionPayments":'+isnull(cast(WageDeduction_DeductionPayments as varchar(20)),'null')+',"DeductionOther":'+isnull(cast(WageDeduction_DeductionOther as varchar(20)),'null')+'},"IncomeConfirmation":{"IsIssuedByExternalAccountant":'+(case when IncomeConfirmation_IsIssuedByExternalAccountant=1 then 'true' else 'false' end)+',"ConfirmationDate":'+(case when Job_FirstWorkContractSince_y is null then 'null' else '"'+cast(Job_FirstWorkContractSince_y as varchar(4))+'-'+FORMAT(Job_FirstWorkContractSince_m, 'D2')+'-'+FORMAT(Job_FirstWorkContractSince_d, 'D2')+'T00:00:00"' end)+',"ConfirmationPerson":"'+isnull(IncomeConfirmation_ConfirmationPerson,'')+'","ConfirmationContact":"'+isnull(IncomeConfirmation_ConfirmationContact,'')+'"}}'
	+'}'
	from dbo.CustomerOnSAIncome A CROSS APPLY OPENJSON (A.Data) 
	with (
		ForeignIncomeTypeId int '$.ForeignIncomeTypeId', 
		HasProofOfIncome bit '$.HasProofOfIncome',
		HasWageDeduction bit '$.HasWageDeduction',
		Employer_Name nvarchar(250) '$.Employer.Name',
		Employer_BirthNumber nvarchar(20) '$.Employer.BirthNumber',
		Employer_Cin nvarchar(20) '$.Employer.Cin',
		Employer_CountryId int '$.Employer.CountryId',
		Job_GrossAnnualIncome int '$.Job.GrossAnnualIncome.Units',
		Job_JobDescription nvarchar(250) '$.Job.JobDescription',
		Job_IsInProbationaryPeriod bit '$.Job.IsInProbationaryPeriod',
		Job_IsInTrialPeriod bit '$.Job.IsInTrialPeriod',
		Job_EmploymentTypeId int '$.Job.EmploymentTypeId',
		Job_CurrentWorkContractSince_d int '$.Job.CurrentWorkContractSince.Day',
		Job_CurrentWorkContractSince_m int '$.Job.CurrentWorkContractSince.Month',
		Job_CurrentWorkContractSince_y int '$.Job.CurrentWorkContractSince.Year',
		Job_CurrentWorkContractTo_d int '$.Job.CurrentWorkContractTo.Day',
		Job_CurrentWorkContractTo_m int '$.Job.CurrentWorkContractTo.Month',
		Job_CurrentWorkContractTo_y int '$.Job.CurrentWorkContractTo.Year',
		Job_FirstWorkContractSince_d int '$.Job.FirstWorkContractSince.Day',
		Job_FirstWorkContractSince_m int '$.Job.FirstWorkContractSince.Month',
		Job_FirstWorkContractSince_y int '$.Job.FirstWorkContractSince.Year',
		WageDeduction_DeductionDecision int '$.WageDeduction.DeductionDecision.Units',
		WageDeduction_DeductionPayments int '$.WageDeduction.DeductionPayments.Units',
		WageDeduction_DeductionOther int '$.WageDeduction.DeductionOther.Units',
		IncomeConfirmation_IsIssuedByExternalAccountant bit '$.IncomeConfirmation.IsIssuedByExternalAccountant',
		IncomeConfirmation_ConfirmationPerson nvarchar(250) '$.IncomeConfirmation.ConfirmationPerson',
		IncomeConfirmation_ConfirmationContact nvarchar(250) '$.IncomeConfirmation.ConfirmationContact',
		IncomeConfirmation_ConfirmationDate_d int '$.IncomeConfirmation.ConfirmationDate.Day',
		IncomeConfirmation_ConfirmationDate_m int '$.IncomeConfirmation.ConfirmationDate.Month',
		IncomeConfirmation_ConfirmationDate_y int '$.IncomeConfirmation.ConfirmationDate.Year'
	) as D
	where A.IncomeTypeId=1

SET IDENTITY_INSERT [DDS].[Income] OFF;
GO

ALTER TABLE [dbo].[CustomerOnSAIncome] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[CustomerOnSAIncomeHistory]
GO
