powershell.exe -file windows-services-registration.ps1 -env fat

<#
    - odstranění WinSvc:
    SC DELETE "<service name>" (např: SC DELETE "DS-DEV-CaseService")
#>