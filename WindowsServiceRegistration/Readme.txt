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