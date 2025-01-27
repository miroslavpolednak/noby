#gc.ps1 gRPC Caller config
#Jakub Vana (jakub.vana@mpss.cz/jaku_vana@kb.cz)

#echo $nobyEnv
#echo $nobyEnvNode
#echo $service
#echo $method

$global:headers='Authorization: Basic YTph'

if($nobyEnv -eq "SIT")
{
    echo "Environment SIT does not exists. Using SIT1 instead."
    $nobyEnv="SIT1"
}

if($service -eq "SA")
{
    $service="SalesArrangement"
}


switch($nobyEnv)
{
    "DEV" 
    {
        $portBase=30000
        $serverGroup="DEVFATSIT1"
    }

    "FAT" 
    {
        $portBase=31000
        $serverGroup="DEVFATSIT1"
    }

    "SIT1" 
    {
        $portBase=32000
        $serverGroup="DEVFATSIT1"
    }

    "UAT" 
    {
        $portBase=33000
        $serverGroup="UAT"
    }
}

switch($serverGroup)
{
    "DEVFATSIT1" 
    {
        switch($nobyEnvNode)
        {
            Default 
            {
                echo "Using default node A"
                $nobyEnvNode='A'
                $server="adpra191"
            }
            "A" {$server="adpra191"}
            "B" {$server="adpra192"}
        }
    }
    "UAT" 
    {
        switch($nobyEnvNode)
        {
            Default 
            {
                echo "Using default node A"
                $nobyEnvNode='A'
                $server="adpra193"
            }
            "A" {$server="adpra193"}
            "B" {$server="adpra194"}
        }
    }    
}

switch($service)
{
    "Discovery" 
    {
        $servicePath="CIS.InternalServices.ServiceDiscovery.v1.DiscoveryService"
        $servicePort=0
    }
    "Case" 
    {
        $servicePath="DomainServices.CaseService.v1.CaseService"
        $servicePort=1
    }
    "Codebook" 
    {
        $servicePath="DomainServices.CodebookService"
        $servicePort=3
    }    
    "Customer" 
    {
        $servicePath="DomainServices.CustomerService.V1.CustomerService"
        $servicePort=4
    }    
    "Offer" 
    {
        $servicePath="DomainServices.OfferService.v1.OfferService"
        $servicePort=6
    }
    "Product" 
    {
        $servicePath="DomainServices.ProductService.v1.ProductService"
        $servicePort=7
    }

    "SalesArrangement" 
    {
        $servicePath="DomainServices.SalesArrangementService.v1.SalesArrangementService"
        $servicePort=9
    }
    "DocumentGenerator" 
    {
        $servicePath="CIS.InternalServices.DocumentGeneratorService.V1.DocumentGeneratorService"
        $servicePort=14
    }          
    "Household" 
    {
        $servicePath="DomainServices.HouseholdService.v1.HouseholdService"
        $servicePort=18
    }    
    "CustomerOnSA" 
    {
        $servicePath="DomainServices.HouseholdService.v1.CustomerOnSAService"
        $servicePort=18
    }            

    "User" 
    {
        $servicePath="DomainServices.UserService.v1.UserService"
        $servicePort=10
    }    
}

$port=$portBase+$servicePort
$global:serverPort=$server+":"+$port
$global:fullMethod=$servicePath+"/"+$method

#echo $headers
#echo $serverPort
#echo $fullMethod

