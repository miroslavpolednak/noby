CREATE OR ALTER VIEW vw_DocumentFields AS

SELECT d.DocumentId, d.DocumentVersion, d.AcroFieldName, df.DataServiceId, df.FieldPath, d.StringFormat, d.TextAlign, d.DefaultTextIfNull, ISNULL(v.DocumentVariant, '') as DocumentVariant
FROM DocumentDataField d
INNER JOIN DataField df ON df.DataFieldId = d.DataFieldId
LEFT JOIN DocumentVariant v ON v.DocumentId = d.DocumentId AND v.DocumentVersion = d.DocumentVersion AND v.AcroFieldName = d.AcroFieldName

UNION ALL

SELECT ds.DocumentId, ds.DocumentVersion, ds.AcroFieldName, ds.DataServiceId, ds.FieldPath, ds.StringFormat, ds.TextAlign, ds.DefaultTextIfNull, ISNULL(v.DocumentVariant, '') as DocumentVariant
FROM DocumentSpecialDataField ds
LEFT JOIN DocumentVariant v ON v.DocumentId = ds.DocumentId AND v.DocumentVersion = ds.DocumentVersion AND v.AcroFieldName = ds.AcroFieldName

GO

CREATE OR ALTER VIEW vw_DocumentTables AS

SELECT t.DocumentTableId, t.DocumentId, t.DocumentVersion, df.FieldPath as TableSourcePath, t.AcroFieldPlaceholderName, t.ConcludingParagraph
FROM DocumentTable t
INNER JOIN DataField df ON df.DataFieldId = t.DataFieldId

GO

CREATE OR ALTER VIEW vw_DocumentTableColumns AS

SELECT c.DocumentTableId, c.FieldPath, c.WidthPercentage, c.[Order], c.StringFormat, c.Header
FROM DocumentTableColumn c

GO

CREATE OR ALTER VIEW vw_DocumentDynamicInputParameters AS

SELECT di.DocumentId, di.DocumentVersion, InputParameter, i.TargetDataServiceId, df.DataServiceId as SourceDataServiceId, df.FieldPath as SourceFieldPath 
FROM DocumentDynamicInputParameter di
INNER JOIN DynamicInputParameter i ON i.DynamicInputParameterId = di.DynamicInputParameterId
INNER JOIN DataField df ON df.DataFieldId = i.SourceDataFieldId

GO

CREATE OR ALTER VIEW vw_DocumentDynamicStringFormats AS

SELECT sf.DocumentId, sf.DocumentVersion, sf.AcroFieldName, sf.StringFormat, sf.[Priority], c.DynamicStringFormatId, c.EqualToValue, df.FieldPath 
FROM DocumentDynamicStringFormat sf
INNER JOIN DocumentDynamicStringFormatCondition c ON c.DynamicStringFormatId = sf.DynamicStringFormatId
INNER JOIN DataField df ON df.DataFieldId = c.DataFieldId

GO

CREATE OR ALTER VIEW vw_EasFormFields AS

SELECT ef.EasRequestTypeId, ef.EasFormTypeId, ef.JsonPropertyName, df.DataServiceId, df.FieldPath FROM EasFormDataField ef
INNER JOIN DataField df ON df.DataFieldId = ef.DataFieldId

UNION ALL

SELECT esf.EasRequestTypeId, esf.EasFormTypeId, esf.JsonPropertyName, esf.DataServiceId, esf.FieldPath FROM EasFormSpecialDataField esf

GO

CREATE OR ALTER VIEW vw_EasFormDynamicInputParameters AS

SELECT di.EasRequestTypeId, di.EasFormTypeId, InputParameter, i.TargetDataServiceId, df.DataServiceId as SourceDataServiceId, df.FieldPath as SourceFieldPath 
FROM EasFormDynamicInputParameter di
INNER JOIN DynamicInputParameter i ON i.DynamicInputParameterId = di.DynamicInputParameterId
INNER JOIN DataField df ON df.DataFieldId = i.SourceDataFieldId

GO

CREATE OR ALTER VIEW vw_RiskLoanApplicationFields AS

SELECT laf.JsonPropertyName, df.DataServiceId, df.FieldPath, laf.UseDefaultInsteadOfNull FROM RiskLoanApplicationDataField laf
INNER JOIN DataField df ON df.DataFieldId = laf.DataFieldId

UNION ALL

SELECT lasf.JsonPropertyName, lasf.DataServiceId, lasf.FieldPath, lasf.UseDefaultInsteadOfNull FROM RiskLoanApplicationSpecialDataField lasf