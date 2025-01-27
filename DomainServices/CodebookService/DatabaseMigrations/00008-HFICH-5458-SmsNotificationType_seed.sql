﻿DELETE FROM [dbo].[SmsNotificationType]
GO       

INSERT [dbo].[SmsNotificationType] ([Id], [Code], [Description], [SmsText], [McsCode], [IsAuditLogEnabled]) VALUES
    (1, N'INSIGN_PROCESS', N'Notifikace, která sdružuje všechny notifikace, které insign odesílá v průběhu podpisového případu (link na podepisovaný dokument).', NULL, N'MCS_HF_INSIGN_001', 1),
    
    (2, N'EPODPISY_SIGNED_DOCUMENT_KB', N'Notifikace, kterou notifikujeme nezávisle na podpisovém toolu uživatele o úspěšném podepsání dokumentu a posíláme heslo.', NULL, N'MCS_HF_EPODPISY_001', 0),
    (3, N'EPODPISY_SIGNED_DOCUMENT_MP', N'Notifikace, kterou notifikujeme nezávisle na podpisovém toolu uživatele o úspěšném podepsání dokumentu a posíláme heslo.', NULL, N'MCS_HF_EPODPISY_002', 0),

    (4, N'SB_NOTIFICATIONS_AUDITED_KB', N'Auditované notifikace, která sdružuje všechny notifikace, které StarBuild odesílá jako součást své business logiky pro KB.', NULL, N'MCS_HF_STARBUILD_001', 1),
    (5, N'SB_NOTIFICATIONS_KB', N'Neauditované notifikace, která sdružuje všechny notifikace, které StarBuild odesílá jako součást své business logiky pro KB.', NULL, N'MCS_HF_STARBUILD_001', 0),
    (6, N'SB_NOTIFICATIONS_AUDITED_MP', N'Auditované notifikace, která sdružuje všechny notifikace, které StarBuild odesílá jako součást své business logiky pro MP.', NULL, N'MCS_HF_STARBUILD_002', 1),
    (7, N'SB_NOTIFICATIONS_MP', N'Neauditované notifikace, která sdružuje všechny notifikace, které StarBuild odesílá jako součást své business logiky pro MP.', NULL, N'MCS_HF_STARBUILD_002', 0),
    (8, N'ARCHIVATOR_RETENCE_OZNAMENI_KB', N'Notifikace, kterou posíláme v případě, že je oznámení KB předáno elektronickou cestou jako legal požadavek.', N'Dobrý den, ve svém online bankovnictví najdete dokument se všemi podrobnostmi k nové úrokové sazbě, na které jsme se společně domluvili. Přejeme vám hezký den, Vaše Komerční banka.', N'MCS_HF_ARCHIVATOR_001', 1),
    (9, N'ARCHIVATOR_RETENCE_OZNAMENI_MP', N'Notifikace, kterou posíláme v případě, že je oznámení MP předáno elektronickou cestou jako legal požadavek.', N'Dobrý den, ve svém online bankovnictví najdete dokument se všemi podrobnostmi k nové úrokové sazbě, na které jsme se společně domluvili. Přejeme vám hezký den, Vaše Komerční banka.', N'MCS_HF_ARCHIVATOR_002', 1)
GO