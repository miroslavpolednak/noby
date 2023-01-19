param([string]$env)

function CheckEnv([string] $env) {
    [string[]] $envs = "DEV", "FAT", "SIT1", "UAT", "PREPROD"
    
    if ($envs.Contains($env.ToUpperInvariant()) -eq $false)
    {
        $s_envs = $envs -join ", "
        throw [System.ArgumentException]::new("Invalid param 'env' [$env]! Supported environments: $s_envs .", "env")
    }

    return $env.ToUpperInvariant()
}

function WinSvcCreate([string] $service, [bool] $isInternal) {

    $svc_name = If ($isInternal -eq $true) {"CIS-$env-$service"} Else {"DS-$env-$service"}

    # check if service exists
    $svc = Get-Service $svc_name -ErrorAction SilentlyContinue

    if ($svc) {
        echo "$svc_name - exists"
    }
    else {
        
        # ---------------------------------------------------
        # create subfolders
        # ---------------------------------------------------
        [string[]] $subfolders = "d:\app\DS-$env\$service", "d:\backup\DS-$env\$service", "d:\deploy\DS-$env\$service"

        $subfolders.ForEach({
            $path = $_

            If(!(test-path -PathType container $path))
            {
                New-Item -ItemType Directory -Path $path
            }
        })

        # ---------------------------------------------------
        # create windows service
        # ---------------------------------------------------
        $svc_bin_path = If ($isInternal -eq $true) {"d:\app\DS-$env\$service\CIS.InternalServices.$service.Api.exe winsvc"} Else {"d:\app\DS-$env\$service\DomainServices.$service.Api.exe winsvc"}

        New-Service -name $svc_name -binaryPathName $svc_bin_path -displayName $svc_name -startupType Automatic

        # set for TFS
        Start-Process -FilePath "D:\sw-install\SubInACL\subinacl.exe" -ArgumentList @("/service $svc_name", "/grant=vsskb\kb2mp_devops=LQSETOP")

        Start-Service -Name $svc_name

        echo "$svc_name - created"
    }
}

$env = CheckEnv($env)

# services
[string[]] $services = "ServiceDiscovery", "NotificationService", "DataAggregatorService", "DocumentGeneratorService", "HouseholdService", "CaseService", "CodebookService", "CustomerService", "OfferService", "ProductService", "RiskIntegrationService", "SalesArrangementService", "UserService", "DocumentArchiveService"

# internalservices
[string[]] $internal = "ServiceDiscovery", "NotificationService", "DataAggregatorService", "DocumentGeneratorService"

$services.ForEach({
    $isInternal = $internal.IndexOf($_) -ge 0

    # echo $_
    echo $isInternal

    WinSvcCreate $_ $isInternal
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
