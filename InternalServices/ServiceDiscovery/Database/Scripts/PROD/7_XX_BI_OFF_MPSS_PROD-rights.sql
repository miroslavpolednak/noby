USE [master]
CREATE LOGIN [VSSKB\XX_BI_OFF_MPSS_PROD] FROM WINDOWS WITH DEFAULT_DATABASE=[master]

GO

USE [OfferService]
CREATE USER [VSSKB\XX_BI_OFF_MPSS_PROD] FOR LOGIN [VSSKB\XX_BI_OFF_MPSS_PROD]
GRANT ALTER ON SCHEMA::[bdp] TO [VSSKB\XX_BI_OFF_MPSS_PROD]
GRANT CONTROL ON SCHEMA::[bdp] TO [VSSKB\XX_BI_OFF_MPSS_PROD]