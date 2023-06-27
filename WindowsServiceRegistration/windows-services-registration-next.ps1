param([string]$env, [string]$exec, [string]$services)


class SVC {

    hidden [string] $svc_name
    hidden [bool] $is_internal = $false;
    hidden [SVC[]] $depends_on = @();
    hidden [string] $env

    [string] $folder_app
    [string] $folder_backup
    [string] $folder_deploy

    SVC([string] $svc_name, [string] $env)
    {
        $this.svc_name = $svc_name;
        $this.env = $env;
    }

    [SVC] Internal([bool]$is_internal) {
        $this.is_internal = $is_internal;
        return $this;
    }

    [SVC] DependsOn([SVC[]]$services) {
        $this.depends_on = $services;
        return $this;
    }

    [string] ToString() {
        [string] $internal = if ($this.is_internal -eq $true) {'*'} Else {' '};

        [string] $result = "[{0}] {1} {2}" -f $this.env, $this.svc_name, $internal

        if ($this.depends_on.count -gt 0) {
            $services = $this.depends_on | % {$_.svc_name }
            $dependencies = $services -join ','
            $result += (" ({0})" -f $dependencies); 
        }
        
        return $result;
    }

    static [string] ToShortServiceName([string] $name) {
        if ([string]::IsNullOrEmpty($name) -eq $true) {
            return $name;
        }
        return $name -ireplace "Service", "";
    }


    [string] GetWinSvcName() {
        $svc_prefix = If ($this.is_internal -eq $true) {'CIS'} Else {'DS'};
        return "{0}-{1}-{2}" -f $svc_prefix, $this.env, $this.svc_name
    }

    [string] GetWinSvcDependsOn() {
        $depends_on_list = $this.depends_on | % { $_.GetWinSvcName() }
        return $depends_on_list -join ',';
    }

    [string] GetBinaryPathName() {
        $folder = If ($this.is_internal -eq $true) {'CIS.InternalServices'} Else {'DomainServices'};
        return "d:\app\DS-{0}\{1}\{2}.{1}.Api.exe winsvc" -f $this.env, $this.svc_name, $folder
    }

    [string[]] CreateSubfolders() {
        [string[]] $folders = @('app', 'backup', 'deploy');
        [string[]] $subfolders = $folders | % { "d:\{0}\DS-{1}\{2}" -f $_, $this.env, $this.svc_name }
               
        foreach ($path in $subfolders) {
            If (!(test-path -PathType container $path))
            {
                New-Item -ItemType Directory -Path $path
            }
        }

        return $subfolders;
    }

    [string] GetStatus() {

        [string] $status = $null;

        $win_svc_name = $this.GetWinSvcName();

        # get service
        $winsvc = Get-Service $win_svc_name -ErrorAction SilentlyContinue

        if ($winsvc) {
            # write-host $winsvc.Status
            $status = $winsvc.Status;
        }

        return $status;
    }

    [void] Start() {
        $win_svc_name = $this.GetWinSvcName();
        SC.exe START $win_svc_name;
    }

    [void] Stop() {
        $win_svc_name = $this.GetWinSvcName();
        SC.exe STOP $win_svc_name;
    }

    [bool] Create() {

        $win_svc_name = $this.GetWinSvcName();

        $depends_on_list = $this.depends_on | % { $_.GetWinSvcName() }
        $depends_on_str = $depends_on_list -join ',';

        $params = @{
            Name = $win_svc_name
            BinaryPathName = $this.GetBinaryPathName()
            DependsOn = $this.GetWinSvcDependsOn()
            DisplayName = $win_svc_name
            StartupType = "Automatic"
            Description = $this.svc_name
          }
        
        # check if service exists
        $svc = Get-Service $win_svc_name -ErrorAction SilentlyContinue

        if ($svc) {
            return $false;
        }

        $this.CreateSubfolders();

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

        # create
        New-Service @params

        # start
        $this.Start();

        # set for TFS
        Start-Process -FilePath "D:\sw-install\SubInACL\subinacl.exe" -ArgumentList @("/service $win_svc_name", "/grant=vsskb\kb2mp_devops=LQSETOP")

        return $true;
    }

    [bool] Remove() {

        $win_svc_name = $this.GetWinSvcName();

        # check if service exists
        $svc = Get-Service $win_svc_name -ErrorAction SilentlyContinue

        if ($svc.Length -eq 0) {
            return $false;
        }

        # remove
        # note: in PowerShell 6 can be used 'Remove-Service -Name $win_svc_name'

        # service STOP
        $this.Stop();

        # service DELETE
        SC.exe DELETE $win_svc_name;

        return $true;
    }

}

function GetEnvironments() {
    return @("DEV", "FAT", "SIT1", "UAT", "PREPROD");
}

function GetServices([string]$env) {

    $service_discovery = [SVC]::New('ServiceDiscovery', $env).Internal($true);

    [SVC[]] $services = @(
        $service_discovery
        , [SVC]::New('NotificationService', $env).DependsOn(@($service_discovery)).Internal($true)
        , [SVC]::New('DataAggregatorService', $env).DependsOn(@($service_discovery)).Internal($true)
        , [SVC]::New('DocumentGeneratorService', $env).DependsOn(@($service_discovery)).Internal($true)
        , [SVC]::New('HouseholdService', $env).DependsOn(@($service_discovery))
        , [SVC]::New('CaseService', $env).DependsOn(@($service_discovery))
        , [SVC]::New('CodebookService', $env).DependsOn(@($service_discovery))
        , [SVC]::New('CustomerService', $env).DependsOn(@($service_discovery))
        , [SVC]::New('OfferService', $env).DependsOn(@($service_discovery))
        , [SVC]::New('ProductService', $env).DependsOn(@($service_discovery))
        , [SVC]::New('RiskIntegrationService', $env).DependsOn(@($service_discovery))
        , [SVC]::New('SalesArrangementService', $env).DependsOn(@($service_discovery))
        , [SVC]::New('UserService', $env).DependsOn(@($service_discovery))
        , [SVC]::New('DocumentArchiveService', $env).DependsOn(@($service_discovery))
        , [SVC]::New('DocumentOnSAService', $env).DependsOn(@($service_discovery))
    );

    return $services;
}

function ShowHelp([string] $env) {

    if ([string]::IsNullOrEmpty($env) -eq $false) {
        return;
    }

    [string[]]$envs = GetEnvironments($null);
    $envs_str = $envs -join ",";
    $row_env = "- 'env' [{0}], required" -f $envs_str;
    
    [SVC[]] $services = GetServices($env);
    $services_str = ($services | % {[SVC]::ToShortServiceName($_.svc_name)}) -join ",";
    $row_services = "- 'services' [{0}], default: all services" -f $services_str;

    $row_exec = "- 'exec' [status, create, remote], default: 'status'";
    

    $rows = @(
        'NOBY SERVICES REGISTRATION',
        '- accepts args: [env, exec, services]; args are case insensitive',
        $row_env,
        $row_exec,
        $row_services,
        'Example: -env dev',
        'Example: -env fat -exec create',
        'Example: -env sit1 -exec remove -services Notification,Household,Codebook'
    );

    foreach ($r in $rows) {
        Write-Host $r -ForegroundColor DarkYellow;
    }

    [Environment]::Exit(1);
}

function CheckEnv([string] $env) {

    if ([string]::IsNullOrEmpty($env) -eq $true) {
        Write-Host "Param 'env' must be provided!" -ForegroundColor Red
        [Environment]::Exit(1);
    }
    
    [string[]] $envs = GetEnvironments($null);
    
    if ($envs.Contains($env.ToUpperInvariant()) -eq $false)
    {
        $envs_str = $envs -join ", "
        $message = "Invalid param 'env' [{0}]! Supported environments: {1}." -f $env, $envs_str
        Write-Host $message -ForegroundColor Red
        [Environment]::Exit(1)
    }

    return $env.ToUpperInvariant()
}

function CheckExec([string] $exec) {

    if ([string]::IsNullOrEmpty($exec) -eq $true) {
        return "STATUS";
    }
    
    [string[]] $execs = "STATUS", "START", "STOP", "CREATE", "REMOVE"
    
    if ($execs.Contains($exec.ToUpperInvariant()) -eq $false)
    {
        $execs_str = $execs -join ", "
        $message = "Invalid param 'exec' [{0}]! Supported: {1}." -f $exec, $execs_str
        Write-Host $message -ForegroundColor Red
        [Environment]::Exit(1)
    }

    return $exec.ToUpperInvariant()
}

function CheckServices([string] $services_str) {

    [SVC[]] $services_all = GetServices($env);

    if ([string]::IsNullOrEmpty($exec) -eq $services_str) {
        return $services_all;
    }

    # build dictionary of services by short name
    $services_by_shortname = @{};
    foreach ($svc in $services_all) {
        $short_name = [SVC]::ToShortServiceName($svc.svc_name).ToUpperInvariant();
        if (! ($short_name -in $services_by_shortname.Keys)) {
            $services_by_shortname.Add($short_name, $svc);
        }
    }

    # parse required services
    [string[]] $service_names_req = $services_str -split ",";
    [string[]] $service_names_invalid = @();
    [SVC[]] $services = @();

    foreach ($svc_name_req in $service_names_req) {
        $short_name = [SVC]::ToShortServiceName($svc_name_req).ToUpperInvariant();

        if ($short_name -in $services_by_shortname.Keys) {
            $services += $services_by_shortname[$short_name];
        } else {
            $service_names_invalid += $svc_name_req;
        }
    }

    # exit if invalid services found
    if ($service_names_invalid.Count -gt 0)
    {
        $names_invalid_str = $service_names_invalid -join ",";
        $names_all = ($services_all | % {[SVC]::ToShortServiceName($_.svc_name)}) -join ",";
        $message = "Invalid param 'services' [{0}]! Supported: {1}." -f $names_invalid_str, $names_all
        
        Write-Host $message -ForegroundColor Red
        [Environment]::Exit(1)
    }

    return $services;
}

function ServicesCreate([SVC[]] $services) {

    $message = "STARTED CREATION OF SERVICES ({0})." -f $services.Count;
    Write-Host $message -ForegroundColor Blue;

    [SVC[]] $services_success = @();
    [SVC[]] $services_failure = @();

    foreach ($svc in $services) {

        $index = [array]::indexof($services, $svc) + 1;

        try {
            $created = $svc.create();
            $created_str = If ($created -eq $true) {'CREATED'} Else {'exists'};
            $message = "[{0}] {1} ({2})" -f $index, $svc.GetWinSvcName(), $created_str;
            Write-Host $message -ForegroundColor Green
            $services_success += $svc; 
        } catch {
            $services_failure += $svc;
            $message = "[{0}] {1} (FAILED: {2})" -f $index, $svc.GetWinSvcName(), $_.Exception.Message;
            Write-Host $message -ForegroundColor Red
            Write-Host $_.ScriptStackTrace
        }
    
    }

    $message = "FINISHED CREATION OF SERVICES ({0}/{1})." -f $services_success.Count, $services.Count;
    Write-Host $message -ForegroundColor Blue;
}

function ServicesRemove([SVC[]] $services) {

    [array]::Reverse($services)

    $message = "STARTED REMOVING OF SERVICES ({0})." -f $services.Count;
    Write-Host $message -ForegroundColor Blue;

    [SVC[]] $services_success = @();
    [SVC[]] $services_failure = @();

    foreach ($svc in $services) {

        $index = [array]::indexof($services, $svc) + 1;

        try {
            $removed = $svc.remove();
            $removed_str = If ($removed -eq $true) {'REMOVED'} Else {'not found'};
            $message = "[{0}] {1} ({2})" -f $index, $svc.GetWinSvcName(), $removed_str;
            Write-Host $message -ForegroundColor Green
            $services_success += $svc; 
        } catch {
            $services_failure += $svc;
            $message = "[{0}] {1} (FAILED: {2})" -f $index, $svc.GetWinSvcName(), $_.Exception.Message;
            Write-Host $message -ForegroundColor Red
            Write-Host $_.ScriptStackTrace
        }
    
    }

    $message = "FINISHED REMOVING OF SERVICES ({0}/{1})." -f $services_success.Count, $services.Count;
    Write-Host $message -ForegroundColor Blue;
}

function ServicesStatus([SVC[]] $services) {

    $message = "STARTED LOOKING FOR STATUS OF SERVICES ({0})." -f $services.Count;
    Write-Host $message -ForegroundColor Blue;

    [SVC[]] $services_success = @();
    [SVC[]] $services_failure = @();

    foreach ($svc in $services) {

        $index = [array]::indexof($services, $svc) + 1;

        try {
            $status = $svc.GetStatus();
            $status_str = If ($status -ne $null) {$status} Else {'not found'};
            $message = "[{0}] {1} ({2})" -f $index, $svc.GetWinSvcName(), $status_str;
            Write-Host $message -ForegroundColor Green
            $services_success += $svc; 
        } catch {
            $services_failure += $svc;
            $message = "[{0}] {1} (FAILED: {2})" -f $index, $svc.GetWinSvcName(), $_.Exception.Message;
            Write-Host $message -ForegroundColor Red
            Write-Host $_.ScriptStackTrace
        }
    
    }

    $message = "FINISHED LOOKING FOR STATUS OF SERVICES ({0}/{1})." -f $services_success.Count, $services.Count;
    Write-Host $message -ForegroundColor Blue;
}

function ServicesStart([SVC[]] $services) {

    $message = "STARTED STARTING OF SERVICES ({0})." -f $services.Count;
    Write-Host $message -ForegroundColor Blue;

    [SVC[]] $services_success = @();
    [SVC[]] $services_failure = @();

    foreach ($svc in $services) {

        $index = [array]::indexof($services, $svc) + 1;

        try {
            $svc.Start();
            $message = "[{0}] {1} (STARTED)" -f $index, $svc.GetWinSvcName();
            Write-Host $message -ForegroundColor Green
            $services_success += $svc; 
        } catch {
            $services_failure += $svc;
            $message = "[{0}] {1} (FAILED: {2})" -f $index, $svc.GetWinSvcName(), $_.Exception.Message;
            Write-Host $message -ForegroundColor Red
            Write-Host $_.ScriptStackTrace
        }
    
    }

    $message = "FINISHED STARTING OF SERVICES ({0}/{1})." -f $services_success.Count, $services.Count;
    Write-Host $message -ForegroundColor Blue;
}

function ServicesStop([SVC[]] $services) {

    $message = "STARTED STOPPING OF SERVICES ({0})." -f $services.Count;
    Write-Host $message -ForegroundColor Blue;

    [SVC[]] $services_success = @();
    [SVC[]] $services_failure = @();

    foreach ($svc in $services) {

        $index = [array]::indexof($services, $svc) + 1;

        try {
            $svc.Stop();
            $message = "[{0}] {1} (STOPPED)" -f $index, $svc.GetWinSvcName();
            Write-Host $message -ForegroundColor Green
            $services_success += $svc; 
        } catch {
            $services_failure += $svc;
            $message = "[{0}] {1} (FAILED: {2})" -f $index, $svc.GetWinSvcName(), $_.Exception.Message;
            Write-Host $message -ForegroundColor Red
            Write-Host $_.ScriptStackTrace
        }
    
    }

    $message = "FINISHED STOPPING OF SERVICES ({0}/{1})." -f $services_success.Count, $services.Count;
    Write-Host $message -ForegroundColor Blue;
}


# PS version
$version = "PowerShell version: {0}" -f (Get-Host).version 
Write-Host $version -ForegroundColor DarkGray

# Validation
ShowHelp($env);
$env = CheckEnv($env);
$exec = CheckExec($exec);     
[SVC[]] $services = CheckServices($services);

# Process
switch ($exec)    
{
    "STATUS" {ServicesStatus($services)}
    "START" {ServicesStart($services)}
    "STOP" {ServicesStop($services)}
    "CREATE" {ServicesCreate($services)}    
    "REMOVE" {ServicesRemove($services)}   
}
