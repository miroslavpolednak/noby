IF COL_LENGTH('dbo.[Case]', 'CustomerPriceSensitivity') IS NULL
BEGIN
	alter table [Case] add CustomerPriceSensitivity decimal(12,2);
END

IF COL_LENGTH('dbo.[Case]', 'CustomerChurnRisk') IS NULL
BEGIN
	alter table [Case] add CustomerChurnRisk decimal(12,2);
END