INSERT INTO DocumentSpecialDataField VALUES (5, '001', 'Zmocnenec', 1, 'AgentName', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (5, '001', 'ZpusobPodpisu', 1, 'SignatureType', NULL, NULL, NULL)

INSERT INTO DocumentDataField VALUES (16, '001', 'Zmocnenec', 207, '--', NULL, NULL)
INSERT INTO DocumentDataField VALUES (16, '001', 'ZpusobPodpisu', 207, 'způsobem uvedeným  žadateli/spolužadateli v žádosti', NULL, NULL)

INSERT INTO DocumentVariant VALUES (5, '001', 'Zmocnenec', 'A')
INSERT INTO DocumentVariant VALUES (5, '001', 'Zmocnenec', 'B')
INSERT INTO DocumentVariant VALUES (5, '001', 'Zmocnenec', 'C')
INSERT INTO DocumentVariant VALUES (5, '001', 'Zmocnenec', 'D')

INSERT INTO DocumentVariant VALUES (16, '001', 'Zmocnenec', 'A')
INSERT INTO DocumentVariant VALUES (16, '001', 'Zmocnenec', 'B')
INSERT INTO DocumentVariant VALUES (16, '001', 'Zmocnenec', 'C')
INSERT INTO DocumentVariant VALUES (16, '001', 'Zmocnenec', 'D')

INSERT INTO DocumentVariant VALUES (5, '001', 'ZpusobPodpisu', 'A')
INSERT INTO DocumentVariant VALUES (5, '001', 'ZpusobPodpisu', 'B')
INSERT INTO DocumentVariant VALUES (5, '001', 'ZpusobPodpisu', 'C')
INSERT INTO DocumentVariant VALUES (5, '001', 'ZpusobPodpisu', 'D')

INSERT INTO DocumentVariant VALUES (16, '001', 'ZpusobPodpisu', 'A')
INSERT INTO DocumentVariant VALUES (16, '001', 'ZpusobPodpisu', 'B')
INSERT INTO DocumentVariant VALUES (16, '001', 'ZpusobPodpisu', 'C')
INSERT INTO DocumentVariant VALUES (16, '001', 'ZpusobPodpisu', 'D')


DELETE FROM DocumentDataField WHERE DocumentId = 13 AND DocumentVersion = '001' AND AcroFieldName = 'DuvodNeposkytnuti'

INSERT INTO DocumentSpecialDataField VALUES (13, '001', 'DuvodNeposkytnuti', 5, 'TaxResidencyCountriesMissingTinReason', NULL, NULL, NULL)