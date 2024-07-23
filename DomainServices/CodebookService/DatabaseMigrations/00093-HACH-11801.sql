alter table [dbo].[ProfessionCategoryExtension] drop column [IncomeMainTypeAMLIds];
go
delete from dbo.SqlQuery where SqlQueryId in ('CountryCodePhoneIdc', 'IdentificationSubjectMethods', 'TinFormatsByCountry', 'TinNoFillReasonsByCountry');
go
drop table if exists dbo.CountryCodePhoneIdc
go
drop table if exists dbo.TinFormatsByCountry
go
drop table if exists dbo.TinNoFillReasonsByCountry
go
