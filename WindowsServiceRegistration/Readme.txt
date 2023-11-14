How to run the script?

Show help:
- powershell.exe -file windows-services-registration-next.ps1

Show status of 'FAT' services:
- powershell.exe -file windows-services-registration-next.ps1 -env dev

Stop 'FAT' services:
- powershell.exe -file windows-services-registration-next.ps1 -env dev -exec stop

Delete 'FAT' services:
- powershell.exe -file windows-services-registration-next.ps1 -env dev -exec remove

Create 'FAT' services:
- powershell.exe -file windows-services-registration-next.ps1 -env dev -exec create

======================

PS D:\deploy\DS-PROD\_Windows_Service-registration_script_> powershell.exe -file windows-services-registration-next.ps1
PowerShell version: 5.1.17763.3770
NOBY SERVICES REGISTRATION
- accepts args: [env, exec, services]; args are case insensitive
- 'env' [DEV,FAT,SIT1,UAT,PREPROD,PROD], required
- 'exec' [status, create, remote], default: 'status'
- 'services' [Discovery,Notification,DataAggregator,DocumentGenerator,Household,Case,Codebook,Customer,Offer,Product,RiskIntegration,SalesArrangement,User,DocumentArchive,DocumentOnSA], default: all services
Example: -env dev
Example: -env fat -exec create
Example: -env sit1 -exec remove -services Notification,Household,Codebook
PS D:\deploy\DS-PROD\_Windows_Service-registration_script_> powershell.exe -file windows-services-registration-next.ps1 -env PROD -exec create -services ServiceDiscovery
PowerShell version: 5.1.17763.3770
STARTED CREATION OF SERVICES (1).
[1] CIS-PROD-ServiceDiscovery (CREATED)
FINISHED CREATION OF SERVICES (1/1).
PS D:\deploy\DS-PROD\_Windows_Service-registration_script_> powershell.exe -file windows-services-registration-next.ps1 -env PROD -exec create -services CodebookService,NotificationService
PowerShell version: 5.1.17763.3770
STARTED CREATION OF SERVICES (2).
[1] DS-PROD-CodebookService (CREATED)
[2] CIS-PROD-NotificationService (CREATED)
FINISHED CREATION OF SERVICES (2/2).
PS D:\deploy\DS-PROD\_Windows_Service-registration_script_>



