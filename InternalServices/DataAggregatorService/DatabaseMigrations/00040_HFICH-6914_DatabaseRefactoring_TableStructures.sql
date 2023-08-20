CREATE TABLE [dbo].[DataField_New]
(
	[DataFieldId] [int] NOT NULL,
	[DataServiceId] [int] NOT NULL,
	[FieldPath] [nvarchar](250) NOT NULL,

	CONSTRAINT PK_DataField_New PRIMARY KEY (DataFieldId),
	CONSTRAINT FK_DataField_DataService_New FOREIGN KEY (DataServiceId) REFERENCES DataService (DataServiceId) ON UPDATE CASCADE,
	CONSTRAINT UC_DataField UNIQUE (DataServiceId, FieldPath)
)

GO

INSERT INTO DataField_New
SELECT DataFieldId, DataServiceId, FieldPath FROM DataField

GO

CREATE TABLE [dbo].[DocumentDataField_New]
(
	[DocumentId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](5) NOT NULL,
    [AcroFieldName] [nvarchar](100) NOT NULL,
	[DataFieldId] [int] NOT NULL,
	[StringFormat] [nvarchar](max) NULL,
	[TextAlign] [tinyint] NULL,
	[DefaultTextIfNull] [nvarchar](max) NULL,

	CONSTRAINT PK_DocumentDataField_New PRIMARY KEY (DocumentId, DocumentVersion, AcroFieldName),
	CONSTRAINT FK_DocumentDataField_Document_New FOREIGN KEY (DocumentId) REFERENCES Document (DocumentId) ON UPDATE CASCADE,
	CONSTRAINT FK_DocumentDataField_DataField_New FOREIGN KEY (DataFieldId) REFERENCES DataField_New (DataFieldId) ON UPDATE CASCADE
)

GO

INSERT INTO DocumentDataField_New
SELECT DocumentId, DocumentVersion, AcroFieldName, dd.DataFieldId, COALESCE(dd.StringFormat, df.DefaultStringFormat, NULL) as StringFormat, TextAlign, DefaultTextIfNull FROM DocumentDataField dd
LEFT JOIN DataField df ON df.DataFieldId = dd.DataFieldId


GO

CREATE TABLE [dbo].[DocumentSpecialDataField_New]
(
	[DocumentId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](5) NOT NULL,
	[AcroFieldName] [nvarchar](100) NOT NULL,
	[DataServiceId] [int] NOT NULL,
	[FieldPath] [nvarchar](250) NOT NULL,
	[StringFormat] [nvarchar](max) NULL,
	[TextAlign] [tinyint] NULL,
	[DefaultTextIfNull] [nvarchar](max) NULL,

	CONSTRAINT PK_DocumentSpecialDataField_New PRIMARY KEY (DocumentId, DocumentVersion, AcroFieldName),
	CONSTRAINT FK_DocumentSpecialDataField_Document_New FOREIGN KEY (DocumentId) REFERENCES Document (DocumentId) ON UPDATE CASCADE,
	CONSTRAINT FK_DocumentSpecialDataField_DataService_New FOREIGN KEY (DataServiceId) REFERENCES DataService (DataServiceId) On UPDATE CASCADE
)

GO

INSERT INTO DocumentSpecialDataField_New
SELECT DocumentId, '001' as DocumentVersion, AcroFieldName, DataServiceId, FieldPath, StringFormat, TextAlign, DefaultTextIfNull FROM DocumentSpecialDataField

GO

CREATE TABLE [dbo].[DocumentVariant_New]
(
	[DocumentId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](5) NOT NULL,
	[AcroFieldName] [nvarchar](100) NOT NULL,
	[DocumentVariant] [nvarchar](5) NOT NULL,

	CONSTRAINT PK_DocumentVariantt_New PRIMARY KEY (DocumentId, DocumentVersion, AcroFieldName, DocumentVariant),
	CONSTRAINT FK_DocumentVariant_Document_New FOREIGN KEY (DocumentId) REFERENCES Document (DocumentId) ON UPDATE CASCADE
)

GO

INSERT INTO DocumentVariant_New
SELECT df.DocumentId, df.DocumentVersion, df.AcroFieldName, dfv.DocumentVariant FROM DocumentDataFieldVariant dfv
INNER JOIN DocumentDataField df ON df.DocumentDataFieldId = dfv.DocumentDataFieldId

INSERT INTO DocumentVariant_New
SELECT DocumentId, '001' as DocumentVersion, AcroFieldName, DocumentVariant FROM DocumentSpecialDataFieldVariant

GO 

CREATE TABLE [dbo].[DocumentTable_New]
(
	[DocumentTableId] [int] NOT NULL,
	[DocumentId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](5) NOT NULL,
	[DataFieldId] [int] NOT NULL,
	[AcroFieldPlaceholderName] [nvarchar](100) NOT NULL,
	[ConcludingParagraph] [nvarchar](max) NULL

	CONSTRAINT PK_DocumentTable_New PRIMARY KEY (DocumentTableId),
	CONSTRAINT FK_DocumentTable_Document_New FOREIGN KEY (DocumentId) REFERENCES Document (DocumentId) ON UPDATE CASCADE,
	CONSTRAINT FK_DocumentTable_DataField_New FOREIGN KEY (DataFieldId) REFERENCES DataField_New (DataFieldId) ON UPDATE CASCADE,
	CONSTRAINT UC_DocumentTable UNIQUE (DocumentId, DocumentVersion, AcroFieldPlaceholderName)
)

GO

INSERT INTO DocumentTable_New
SELECT * FROM DocumentTable

GO

CREATE TABLE [dbo].[DocumentTableColumn_New]
(
	[DocumentTableId] [int] NOT NULL,
	[FieldPath] [nvarchar](250) NOT NULL,
	[WidthPercentage] [real] NOT NULL,
	[Order] [int] NOT NULL,
	[StringFormat] [nvarchar](500) NULL,
	[Header] [nvarchar](100) NOT NULL,

	CONSTRAINT PK_DocumentTableColumn_New PRIMARY KEY (DocumentTableId, FieldPath),
	CONSTRAINT FK_DocumentTableColumn_DocumentTable_New FOREIGN KEY (DocumentTableId) REFERENCES DocumentTable_New (DocumentTableId)
)

GO

INSERT INTO DocumentTableColumn_New
SELECT * FROM DocumentTableColumn

GO

CREATE TABLE [dbo].[DocumentDynamicStringFormat]
(
	[DynamicStringFormatId] [int] NOT NULL,
	[DocumentId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](5) NOT NULL,
    [AcroFieldName] [nvarchar](100) NOT NULL,
	[StringFormat] [nvarchar](max) NOT NULL,
	[Priority] [int] NOT NULL,

	CONSTRAINT PK_DocumentDynamicStringFormat PRIMARY KEY (DynamicStringFormatId),
	CONSTRAINT FK_DocumentDynamicStringFormat_Document_New FOREIGN KEY (DocumentId) REFERENCES Document (DocumentId) ON UPDATE CASCADE
)

GO

INSERT INTO DocumentDynamicStringFormat
SELECT DynamicStringFormatId, DocumentId, DocumentVersion, AcroFieldName, [Format], [Priority] FROM DynamicStringFormat sf
INNER JOIN DocumentDataField df ON df.DocumentDataFieldId = sf.DocumentDataFieldId

GO

CREATE TABLE [dbo].[DocumentDynamicStringFormatCondition]
(
	[DynamicStringFormatId] [int] NOT NULL,
	[EqualToValue] [nvarchar](100) NULL,
	[DataFieldId] [int] NOT NULL,

	CONSTRAINT PK_DocumentDynamicStringFormatCondition PRIMARY KEY (DynamicStringFormatId, DataFieldId),
	CONSTRAINT FK_DocumentDynamicStringFormatCondition_DocumentDynamicStringFormat FOREIGN KEY (DynamicStringFormatId) REFERENCES DocumentDynamicStringFormat (DynamicStringFormatId),
	CONSTRAINT FK_DocumentDynamicStringFormatCondition_DataField FOREIGN KEY (DataFieldId) REFERENCES DataField_New (DataFieldId) ON UPDATE CASCADE
)

GO

INSERT INTO DocumentDynamicStringFormatCondition
SELECT * FROM DynamicStringFormatCondition

GO

CREATE TABLE [dbo].[EasFormDataField_New]
(
	[EasRequestTypeId] [int] NOT NULL,
	[EasFormTypeId] [int] NOT NULL,
	[JsonPropertyName] [nvarchar](300) NOT NULL,
	[DataFieldId] [int] NOT NULL,

	CONSTRAINT PK_EasFormDataField_New PRIMARY KEY (EasRequestTypeId, EasFormTypeId, JsonPropertyName),
	CONSTRAINT FK_EasFormDataField_EasRequestType_New FOREIGN KEY (EasRequestTypeId) REFERENCES EasRequestType (EasRequestTypeId) ON UPDATE CASCADE,
	CONSTRAINT FK_EasFormDataField_EasFormType_New FOREIGN KEY (EasFormTypeId) REFERENCES EasFormType (EasFormTypeId) ON UPDATE CASCADE,
	CONSTRAINT FK_EasFormDataField_DataField FOREIGN KEY (DataFieldId) REFERENCES DataField_New (DataFieldId) ON UPDATE CASCADE
)

GO

INSERT INTO EasFormDataField_New
SELECT EasRequestTypeId, EasFormTypeId, JsonPropertyName, DataFieldId FROM EasFormDataField

GO

CREATE TABLE [dbo].[EasFormSpecialDataField_New]
(
	[EasRequestTypeId] [int] NOT NULL,
	[EasFormTypeId] [int] NOT NULL,
	[JsonPropertyName] [nvarchar](300) NOT NULL,
	[DataServiceId] [int] NOT NULL,
	[FieldPath] [nvarchar](250) NOT NULL,

	CONSTRAINT PK_EasFormSpecialDataField_New PRIMARY KEY (EasRequestTypeId, EasFormTypeId, JsonPropertyName),
	CONSTRAINT FK_EasFormSpecialDataField_EasRequestType_New FOREIGN KEY (EasRequestTypeId) REFERENCES EasRequestType (EasRequestTypeId) ON UPDATE CASCADE,
	CONSTRAINT FK_EasFormSpecialDataField_EasFormType_New FOREIGN KEY (EasFormTypeId) REFERENCES EasFormType (EasFormTypeId) ON UPDATE CASCADE,
	CONSTRAINT FK_EasFormSpecialDataField_DataService_New FOREIGN KEY (DataServiceId) REFERENCES DataService (DataServiceId) ON UPDATE CASCADE
)

GO

INSERT INTO EasFormSpecialDataField_New
SELECT EasRequestTypeId, EasFormTypeId, JsonPropertyName, DataServiceId, FieldPath FROM EasFormSpecialDataField

GO

CREATE TABLE [dbo].[RiskLoanApplicationDataField_New]
(
	[JsonPropertyName] [nvarchar](250) NOT NULL,
	[DataFieldId] [int] NOT NULL,
	[UseDefaultInsteadOfNull] [bit] NOT NULL DEFAULT 0,

	CONSTRAINT PK_RiskLoanApplicationDataField_New PRIMARY KEY (JsonPropertyName),
	CONSTRAINT FK_RiskLoanApplicationDataField_DataField_New FOREIGN KEY (DataFieldId) REFERENCES DataField_new (DataFieldId) ON UPDATE CASCADE
)

GO

INSERT INTO RiskLoanApplicationDataField_New
SELECT JsonPropertyName, DataFieldId, UseDefaultInsteadOfNull FROM RiskLoanApplicationDataField

GO

CREATE TABLE [dbo].[RiskLoanApplicationSpecialDataField_New]
(
	[JsonPropertyName] [nvarchar](250) NOT NULL,
	[DataServiceId] [int] NOT NULL,
	[FieldPath] [nvarchar](250) NOT NULL,
	[UseDefaultInsteadOfNull] [bit] NOT NULL DEFAULT 0,

	CONSTRAINT PK_RiskLoanApplicationSpecialDataField_New PRIMARY KEY (JsonPropertyName),
	CONSTRAINT FK_RiskLoanApplicationSpecialDataField_DataService_New FOREIGN KEY (DataServiceId) REFERENCES DataService (DataServiceId) ON UPDATE CASCADE
)

GO

INSERT INTO RiskLoanApplicationSpecialDataField_New
SELECT JsonPropertyName, DataServiceId, FieldPath, UseDefaultInsteadOfNull FROM RiskLoanApplicationSpecialDataField

GO

CREATE TABLE [dbo].[DynamicInputParameter]
(
	[DynamicInputParameterId] INT NOT NULL,
	[InputParameter] [nvarchar](50) NOT NULL,
	[TargetDataServiceId] [int] NOT NULL,
	[SourceDataFieldId] [int] NOT NULL,

	CONSTRAINT PK_DynamicInputParameter PRIMARY KEY (DynamicInputParameterId),
	CONSTRAINT UC_DynamicInputParameter UNIQUE (InputParameter, TargetDataServiceId, SourceDataFieldId),
	CONSTRAINT FK_DynamicInputParameter_DataService_New FOREIGN KEY (TargetDataServiceId) REFERENCES DataService (DataServiceId) ON UPDATE CASCADE,
	CONSTRAINT FK_DynamicInputParameter_DataField_New FOREIGN KEY (SourceDataFieldId) REFERENCES DataField_New (DataFieldId)
)

GO

INSERT INTO DynamicInputParameter
SELECT ROW_NUMBER() OVER (ORDER BY t.InputParameterName), * FROM
(SELECT i.InputParameterName, dip.TargetDataServiceId, dip.SourceDataFieldId FROM DocumentDynamicInputParameter dip
INNER JOIN InputParameter i ON i.InputParameterId = dip.InputParameterId
UNION ALL
SELECT i.InputParameterName, eip.TargetDataServiceId, eip.SourceDataFieldId FROM EasFormDynamicInputParameter eip
INNER JOIN InputParameter i ON i.InputParameterId = eip.InputParameterId) t 
GROUP BY InputParameterName, TargetDataServiceId, SourceDataFieldId

GO

CREATE TABLE [dbo].[DocumentDynamicInputParameter_New]
(
	[DocumentId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](5) NOT NULL,
	[DynamicInputParameterId] [int] NOT NULL

	CONSTRAINT PK_DocumentDynamicInputParameter_New PRIMARY KEY (DocumentId, DocumentVersion, DynamicInputParameterId),
	CONSTRAINT FK_DocumentDynamicInputParameter_DynamicInputParameter_New FOREIGN KEY (DynamicInputParameterId) REFERENCES DynamicInputParameter (DynamicInputParameterId) ON UPDATE CASCADE
)

GO

INSERT INTO DocumentDynamicInputParameter_New
SELECT old.DocumentId, old.DocumentVersion, newInput.DynamicInputParameterId FROM DocumentDynamicInputParameter old
INNER JOIN InputParameter i ON old.InputParameterId = i.InputParameterId
INNER JOIN DynamicInputParameter newInput ON newInput.InputParameter = i.InputParameterName AND newInput.SourceDataFieldId = old.SourceDataFieldId AND newInput.TargetDataServiceId = old.TargetDataServiceId

GO


CREATE TABLE [dbo].[EasFormDynamicInputParameter_New]
(
	[EasRequestTypeId] [int] NOT NULL,
	[EasFormTypeId] [int] NOT NULL,
	[DynamicInputParameterId] [int] NOT NULL

	CONSTRAINT PK_EasFormDynamicInputParameter_New PRIMARY KEY (EasRequestTypeId, EasFormTypeId, DynamicInputParameterId)
	CONSTRAINT FK_EasFormDynamicInputParameter_DynamicInputParameter_New FOREIGN KEY (DynamicInputParameterId) REFERENCES DynamicInputParameter (DynamicInputParameterId) ON UPDATE CASCADE
)

GO


INSERT INTO EasFormDynamicInputParameter_New
SELECT old.EasRequestTypeId, old.EasFormTypeId, newInput.DynamicInputParameterId FROM EasFormDynamicInputParameter old
INNER JOIN InputParameter i ON old.InputParameterId = i.InputParameterId
INNER JOIN DynamicInputParameter newInput ON newInput.InputParameter = i.InputParameterName AND newInput.SourceDataFieldId = old.SourceDataFieldId AND newInput.TargetDataServiceId = old.TargetDataServiceId

GO

DROP TABLE RiskLoanApplicationSpecialDataField
DROP TABLE RiskLoanApplicationDataField

DROP TABLE EasFormSpecialDataField
DROP TABLE EasFormDataField
DROP TABLE EasFormDynamicInputParameter

DROP TABLE DynamicStringFormatCondition
DROP TABLE DynamicStringFormat
DROP TABLE DocumentTableColumn
DROP TABLE DocumentTable
DROP TABLE DocumentSpecialDataFieldVariant
DROP TABLE DocumentSpecialDataField
DROP TABLE DocumentDataFieldVariant
DROP TABLE DocumentDataField
DROP TABLE DocumentDynamicInputParameter

DROP TABLE InputParameter
DROP TABLE DataField

GO

EXEC sp_rename 'DataField_New', 'DataField'
EXEC sp_rename 'DocumentDataField_New', 'DocumentDataField'
EXEC sp_rename 'DocumentDynamicInputParameter_New', 'DocumentDynamicInputParameter'
EXEC sp_rename 'DocumentSpecialDataField_New', 'DocumentSpecialDataField'
EXEC sp_rename 'DocumentTable_New', 'DocumentTable'
EXEC sp_rename 'DocumentTableColumn_New', 'DocumentTableColumn'
EXEC sp_rename 'DocumentVariant_New', 'DocumentVariant'
EXEC sp_rename 'EasFormDataField_New', 'EasFormDataField'
EXEC sp_rename 'EasFormDynamicInputParameter_New', 'EasFormDynamicInputParameter'
EXEC sp_rename 'EasFormSpecialDataField_New', 'EasFormSpecialDataField'
EXEC sp_rename 'FK_DataField_DataService_New', 'FK_DataField_DataService'
EXEC sp_rename 'FK_DocumentDataField_DataField_New', 'FK_DocumentDataField_DataField'
EXEC sp_rename 'FK_DocumentDataField_Document_New', 'FK_DocumentDataField_Document'
EXEC sp_rename 'FK_DocumentDynamicInputParameter_DynamicInputParameter_New', 'FK_DocumentDynamicInputParameter_DynamicInputParameter'
EXEC sp_rename 'FK_DocumentDynamicStringFormat_Document_New', 'FK_DocumentDynamicStringFormat_Document'
EXEC sp_rename 'FK_DocumentSpecialDataField_DataService_New', 'FK_DocumentSpecialDataField_DataService'
EXEC sp_rename 'FK_DocumentSpecialDataField_Document_New', 'FK_DocumentSpecialDataField_Document'
EXEC sp_rename 'FK_DocumentTable_DataField_New', 'FK_DocumentTable_DataField'
EXEC sp_rename 'FK_DocumentTable_Document_New', 'FK_DocumentTable_Document'
EXEC sp_rename 'FK_DocumentTableColumn_DocumentTable_New', 'FK_DocumentTableColumn_DocumentTable'
EXEC sp_rename 'FK_DocumentVariant_Document_New', 'FK_DocumentVariant_Document'
EXEC sp_rename 'FK_DynamicInputParameter_DataField_New', 'FK_DynamicInputParameter_DataField'
EXEC sp_rename 'FK_DynamicInputParameter_DataService_New', 'FK_DynamicInputParameter_DataService'
EXEC sp_rename 'FK_EasFormDataField_EasFormType_New', 'FK_EasFormDataField_EasFormType'
EXEC sp_rename 'FK_EasFormDataField_EasRequestType_New', 'FK_EasFormDataField_EasRequestType'
EXEC sp_rename 'FK_EasFormDynamicInputParameter_DynamicInputParameter_New', 'FK_EasFormDynamicInputParameter_DynamicInputParameter'
EXEC sp_rename 'FK_EasFormSpecialDataField_DataService_New', 'FK_EasFormSpecialDataField_DataService'
EXEC sp_rename 'FK_EasFormSpecialDataField_EasFormType_New', 'FK_EasFormSpecialDataField_EasFormType'
EXEC sp_rename 'FK_EasFormSpecialDataField_EasRequestType_New', 'FK_EasFormSpecialDataField_EasRequestType'
EXEC sp_rename 'FK_RiskLoanApplicationDataField_DataField_New', 'FK_RiskLoanApplicationDataField_DataField'
EXEC sp_rename 'FK_RiskLoanApplicationSpecialDataField_DataService_New', 'FK_RiskLoanApplicationSpecialDataField_DataService'
EXEC sp_rename 'PK_DataField_New', 'PK_DataField'
EXEC sp_rename 'PK_DocumentDataField_New', 'PK_DocumentDataField'
EXEC sp_rename 'PK_DocumentDynamicInputParameter_New', 'PK_DocumentDynamicInputParameter'
EXEC sp_rename 'PK_DocumentSpecialDataField_New', 'PK_DocumentSpecialDataField'
EXEC sp_rename 'PK_DocumentTable_New', 'PK_DocumentTable'
EXEC sp_rename 'PK_DocumentTableColumn_New', 'PK_DocumentTableColumn'
EXEC sp_rename 'PK_DocumentVariantt_New', 'PK_DocumentVariantt'
EXEC sp_rename 'PK_EasFormDataField_New', 'PK_EasFormDataField'
EXEC sp_rename 'PK_EasFormDynamicInputParameter_New', 'PK_EasFormDynamicInputParameter'
EXEC sp_rename 'PK_EasFormSpecialDataField_New', 'PK_EasFormSpecialDataField'
EXEC sp_rename 'PK_RiskLoanApplicationDataField_New', 'PK_RiskLoanApplicationDataField'
EXEC sp_rename 'PK_RiskLoanApplicationSpecialDataField_New', 'PK_RiskLoanApplicationSpecialDataField'
EXEC sp_rename 'RiskLoanApplicationDataField_New', 'RiskLoanApplicationDataField'
EXEC sp_rename 'RiskLoanApplicationSpecialDataField_New', 'RiskLoanApplicationSpecialDataField'