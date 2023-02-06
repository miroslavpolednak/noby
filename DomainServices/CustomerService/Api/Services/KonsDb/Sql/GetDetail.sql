SELECT Id PartnerId,
	Titul DegreeBefore,
	Jmeno FirstName,
	Prijmeni LastName,
	RodneCisloIco BirthNumber,
	DatumNarozeni BirthDate,
	MistoNarozeni PlaceOfBirth,
	Pohlavi GenderId,
	PEP IsPoliticallyExposed,
	StatniPrislusnostId CitizenshipCountryId,
	PrukazTotoznosti IdentificationDocumentNumber,
	TypDokladu IdentificationDocumentTypeId,
	PrukazVydalDatum IdentificationDocumentIssuedOn,
	PrukazVydal IdentificationDocumentIssuedBy,
	PrukazStatVydaniId IdentificationDocumentIssuingCountryId,
	PreukazPlatnostDo IdentificationDocumentValidTo,
	Ulice Street,
	CisloDomu4 StreetNumber,
	CisloDomu2 HouseNumber,
	Psc PostCode,
	Misto City,
	VypisyUlice MailingStreet,
	VypisyCisloDomu4 MailingStreetNumber,
	VypisyCisloDomu2 MailingHouseNumber,
	VypisyPsc MailingPostCode,
	VypisyMisto MailingCity,
	KBId KbId,
	b.KontaktId ContactId,
	b.PrimarniKontakt IsPrimaryContact,
	b.TypKontaktu ContactType,
	b.[Value]
FROM dbo.PARTNER a
LEFT JOIN (
	SELECT Id AS KontaktId,
		PrimarniKontakt,
		TypKontaktu,
		[Value],
		PartnerId
	FROM dbo.PartnerKontakty
	WHERE PartnerId = @partnerId
		AND PlatnostOd < GetDate()
		AND (
			PlatnostDo IS NULL
			OR PlatnostDo < GetDate()
			)
	) b ON a.Id = b.PartnerId
WHERE a.Id = @partnerId
