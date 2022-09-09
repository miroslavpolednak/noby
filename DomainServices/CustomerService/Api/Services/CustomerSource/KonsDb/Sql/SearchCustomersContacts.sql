SELECT PartnerId
FROM dbo.PartnerKontakty
WHERE PlatnostOd < GETDATE()
	AND (
		PlatnostDo IS NULL
		OR PlatnostDo > GetDate()
		)
	AND ([Value] = @email OR [Value] = @phoneNumber)
