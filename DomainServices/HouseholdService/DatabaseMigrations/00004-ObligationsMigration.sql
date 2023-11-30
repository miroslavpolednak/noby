IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DDS].[Obligation]') AND type in (N'U'))
ALTER TABLE [DDS].[Obligation] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [DDS].[Obligation]
GO
DROP TABLE IF EXISTS [DDS].[ObligationHistory]
GO
CREATE TABLE [DDS].[Obligation](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL
 CONSTRAINT [PK_Obligation] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
	SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[ObligationHistory])
)
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[Obligation]
GO
CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[Obligation]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET IDENTITY_INSERT [DDS].[Obligation] ON;
GO

INSERT INTO [DDS].Obligation
(DocumentDataStorageId,[DocumentDataEntityId],[DocumentDataVersion],[CreatedUserId],[CreatedTime],[ModifiedUserId],[Data])
select A.CustomerOnSAObligationId, A.CustomerOnSAId, 1, A.CreatedUserId, A.CreatedTime, A.ModifiedUserId, 
'{"ObligationTypeId":'+isnull(cast(ObligationTypeId as varchar(20)),'null')+',"InstallmentAmount":'+isnull(cast(InstallmentAmount as varchar(20)),'null')+',"LoanPrincipalAmount":'+isnull(cast(LoanPrincipalAmount as varchar(20)),'null')+',"CreditCardLimit":'+isnull(cast(CreditCardLimit as varchar(20)),'null')+',"AmountConsolidated":'+isnull(cast(AmountConsolidated as varchar(20)),'null')+',"ObligationState":'+isnull(cast(ObligationState as varchar(20)),'null')
+',"Creditor":{"CreditorId":"'+isnull(CreditorId,'')+'","Name":"'+isnull(CreditorName,'')+'","IsExternal":'+(case CreditorIsExternal when null then 'null' when 1 then 'true' else 'false' end)
+'},"Correction":{"CorrectionTypeId":'+isnull(cast(CorrectionTypeId as varchar(20)),'null')+',"InstallmentAmountCorrection":'+isnull(cast(InstallmentAmountCorrection as varchar(20)),'null')+',"LoanPrincipalAmountCorrection":'+isnull(cast(LoanPrincipalAmountCorrection as varchar(20)),'null')+',"CreditCardLimitCorrection":'+isnull(cast(CreditCardLimitCorrection as varchar(20)),'null')+'}}'
from dbo.CustomerOnSAObligation A

SET IDENTITY_INSERT [DDS].[Obligation] OFF;
GO

ALTER TABLE [dbo].[CustomerOnSAObligation] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[CustomerOnSAObligationHistory]
GO
