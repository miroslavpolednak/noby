param([string]$env)

function CheckEnv([string] $env) {
    [string[]] $envs = "DEV", "FAT", "SIT1", "UAT"
    
    if ($envs.Contains($env.ToUpperInvariant()) -eq $false)
    {
        $s_envs = $envs -join ", "
        throw [System.ArgumentException]::new("Invalid param 'env' [$env]! Supported environments: $s_envs .", "env")
    }

    return $env.ToUpperInvariant()
}

function WinSvcCreate([string] $service) {
    $svc_name = "DS-$env-$service"

    # check if service exists
    $svc = Get-Service $svc_name -ErrorAction SilentlyContinue

    if ($svc) {
        echo "$svc_name - exists"
    }
    else {
        # create if not exists
        $svc_bin_path = "d:\app\DS-$env\$service\DomainServices.$service.Api.exe winsvc"
        New-Service -name $svc_name -binaryPathName $svc_bin_path -displayName $svc_name -startupType Automatic
        # sc create DS-FAT-HouseholdService binPath= "d:\app\DS-FAT\HouseholdService\DomainServices.HouseholdService.Api.exe winsvc" start=auto error=critical obj=LocalSystem
        # how to set error=critical obj=LocalSystem ???

        # set for TFS
        Start-Process -FilePath "D:\sw-install\SubInACL\subinacl.exe" -ArgumentList @("/service $svc_name", "/grant=vsskb\kb2mp_devops=LQSETOP")

        Start-Service -Name $svc_name

        echo "$svc_name - created"
    }
}

$env = CheckEnv($env)

# domain services
[string[]] $domain_services = "ServiceDiscovery", "HouseholdService", "CaseService", "CodebookService", "CustomerService", "OfferService", "ProductService", "RiskIntegrationService", "SalesArrangementService", "UserService"

$domain_services.ForEach({
    WinSvcCreate($_)
})

<#
    Kromě toho, že Windows službu na serveru zaregistruješ, musíš nastavit na serveru i odpovídající oprávnění  (LQSETOP) pro technický účet vsskb\kb2mp_devops, pod kterým běží TFS agent a nasazuje nové verze na serveru. Toto oprávnění je nezbytné pro to, aby TFS agent mohl zastavovat a startovat Windows službu.
    Oprávnění  na služby nastavíš takto:

    Spustit CMD „Run as Admin“, vlézt do adresáře a spustit nástroj subinacl s těmito parametry:

    cd D:\sw-install\SubInACL\
    # doménové služby:
    subinacl.exe /service DS-* /grant=vsskb\kb2mp_devops=LQSETOP
    # infrastrukturní služby:
    subinacl.exe /service CIS-* /grant=vsskb\kb2mp_devops=LQSETOP 
#>
