INSERT INTO DocumentSpecialDataField VALUES (11, '001', 'ZrusimKreditniKarty', 10, 'Customer1Obligation.CreditCardCorrectionConsolidatedCC', '{0:CustomCurrency}', NULL, '--', NULL)
INSERT INTO DocumentSpecialDataField VALUES (12, '001', 'ZrusimKreditniKarty', 10, 'Customer1Obligation.CreditCardCorrectionConsolidatedCC', '{0:CustomCurrency}', NULL, '--', NULL)

INSERT INTO DocumentVariant VALUES (11, '001', 'ZrusimKreditniKarty', 'A')
INSERT INTO DocumentVariant VALUES (11, '001', 'ZrusimKreditniKarty', 'B')

INSERT INTO DocumentVariant VALUES (12, '001', 'ZrusimKreditniKarty', 'A')
INSERT INTO DocumentVariant VALUES (12, '001', 'ZrusimKreditniKarty', 'B')

UPDATE DocumentSpecialDataField 
SET StringFormat = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v~internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a~zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k~jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje. 

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.',
DefaultTextIfNull = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v tomto dokumentu. Potvrzuji, že údaje uvedené tomto dokumentu slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v tomto dokumentu nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a~Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v~tomto dokumentu těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v~ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.'
WHERE DocumentId = 11 AND AcroFieldName = 'ProhlaseniSpoluzadatele'

UPDATE DocumentSpecialDataField 
SET StringFormat = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v~internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a~zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k~jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje. 

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.',
DefaultTextIfNull = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v tomto dokumentu. Potvrzuji, že údaje uvedené tomto dokumentu slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v tomto dokumentu nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a~Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v~tomto dokumentu těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v~ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.'
WHERE DocumentId = 12 AND AcroFieldName = 'ProhlaseniSpoluzadatele'

UPDATE DocumentSpecialDataField 
SET DefaultTextIfNull = 'Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e' + NCHAR(8211) + 'mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat'
WHERE DocumentId = 11 AND AcroFieldName = 'ProhlaseniSpoluzadateleCast2'

UPDATE DocumentSpecialDataField 
SET DefaultTextIfNull = 'Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e' + NCHAR(8211) + 'mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat'
WHERE DocumentId = 12 AND AcroFieldName = 'ProhlaseniSpoluzadateleCast2'

UPDATE DocumentDataField 
SET StringFormat = 'Prohlašuji, že je mi známo, že původní dlužník s Bankou uzavřel smlouvu o úvěru reg. číslo {0} a~že se zněním této smlouvy jsem byl původním dlužníkem seznámen.'
WHERE DocumentId = 12 AND AcroFieldName = 'ProhlaseniPristupujicich'

INSERT INTO DataField VALUES (216, 4, 'User.Info.DealerCompanyName')

INSERT INTO DocumentDataField VALUES (4, '001', 'Zastupce', 216, NULL, NULL, NULL, NULL)

INSERT INTO DocumentVariant VALUES (4, '001', 'Zastupce', 'A')
INSERT INTO DocumentVariant VALUES (4, '001', 'Zastupce', 'C')

INSERT INTO DocumentDynamicInputParameter VALUES (4, '001', 7)

INSERT INTO DocumentDataField VALUES (5, '001', 'Zastupce', 216, NULL, NULL, NULL, NULL)

INSERT INTO DocumentVariant VALUES (5, '001', 'Zastupce', 'A')
INSERT INTO DocumentVariant VALUES (5, '001', 'Zastupce', 'C')

INSERT INTO DocumentDynamicInputParameter VALUES (5, '001', 7)

INSERT INTO DocumentDataField VALUES (16, '001', 'Zastupce', 216, NULL, NULL, NULL, NULL)

INSERT INTO DocumentVariant VALUES (16, '001', 'Zastupce', 'A')
INSERT INTO DocumentVariant VALUES (16, '001', 'Zastupce', 'C')

INSERT INTO DocumentDynamicInputParameter VALUES (16, '001', 7)

UPDATE DocumentDataField 
SET StringFormat = 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů (za osobu se zvláštním vztahem k Bance se považuje např. člen statutárního orgánu Banky a osoba jemu blízká; člen správní rady a dozorčí rady Banky a člen výboru pro audit, rizika, odměňování a~jmenování a osoba jemu blízká; člen bankovní rady České národní banky),'
WHERE DocumentId = 4 AND AcroFieldName IN ('JsemNejsem1Odrazka', 'SpoluzadatelJsemNejsem1Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů (za osobu se zvláštním vztahem k Bance se považuje např. člen statutárního orgánu Banky a osoba jemu blízká; člen správní rady a dozorčí rady Banky a člen výboru pro audit, rizika, odměňování a~jmenování a osoba jemu blízká; člen bankovní rady České národní banky),'
WHERE DocumentId = 4 AND AcroFieldName IN ('JsemNejsem1Odrazka', 'SpoluzadatelJsemNejsem1Odrazka')

UPDATE DocumentDataField 
SET StringFormat = 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o~některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,'
WHERE DocumentId = 4 AND AcroFieldName IN ('JsemNejsem3Odrazka', 'SpoluzadatelJsemNejsem3Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o~některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,'
WHERE DocumentId = 4 AND AcroFieldName IN ('JsemNejsem3Odrazka', 'SpoluzadatelJsemNejsem3Odrazka')

UPDATE DocumentSpecialDataField 
SET StringFormat = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v~internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a~zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k~jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.

Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
DefaultTextIfNull = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v této žádosti. Potvrzuji, že údaje uvedené v této žádosti slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v této žádosti nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a~Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v~této žádosti těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v~ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený'
WHERE DocumentId = 4 AND AcroFieldName = 'ProhlaseniSpoluzadatele'

UPDATE DocumentDataField 
SET StringFormat = 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů (za osobu se zvláštním vztahem k Bance se považuje např. člen statutárního orgánu Banky a osoba jemu blízká; člen správní rady a dozorčí rady Banky a člen výboru pro audit, rizika, odměňování a~jmenování a osoba jemu blízká; člen bankovní rady České národní banky),'
WHERE DocumentId IN (5, 16) AND AcroFieldName IN ('JsemNejsem1Odrazka', 'SpoluzadatelJsemNejsem1Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů (za osobu se zvláštním vztahem k Bance se považuje např. člen statutárního orgánu Banky a osoba jemu blízká; člen správní rady a dozorčí rady Banky a člen výboru pro audit, rizika, odměňování a~jmenování a osoba jemu blízká; člen bankovní rady České národní banky),'
WHERE DocumentId IN (5, 16) AND AcroFieldName IN ('JsemNejsem1Odrazka', 'SpoluzadatelJsemNejsem1Odrazka')

UPDATE DocumentDataField 
SET StringFormat = 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o~některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,'
WHERE DocumentId IN (5, 16) AND AcroFieldName IN ('JsemNejsem3Odrazka', 'SpoluzadatelJsemNejsem3Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o~některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,'
WHERE DocumentId IN (5, 16) AND AcroFieldName IN ('JsemNejsem3Odrazka', 'SpoluzadatelJsemNejsem3Odrazka')

UPDATE DocumentSpecialDataField
SET StringFormat = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v~internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a~zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k~jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.

Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
DefaultTextIfNull = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v této žádosti. Potvrzuji, že údaje uvedené v této žádosti slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v této žádosti nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a~Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v~této žádosti těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v~ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.'
WHERE DocumentId IN (5, 16) AND AcroFieldName = 'ProhlaseniZadatele'

UPDATE DocumentSpecialDataField
SET StringFormat = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v~internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a~zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k~jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.',
DefaultTextIfNull = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v této žádosti. Potvrzuji, že údaje uvedené v této žádosti slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v této žádosti nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a~Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v~této žádosti těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit'
WHERE DocumentId IN (5, 16) AND AcroFieldName = 'ProhlaseniSpoluzadatele'

UPDATE DocumentSpecialDataField
SET StringFormat = 'Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
DefaultTextIfNull = 'přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v~ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.'
WHERE DocumentId IN (5, 16) AND AcroFieldName = 'ProhlaseniSpoluzadateleCast2'