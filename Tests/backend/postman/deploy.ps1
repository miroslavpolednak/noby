

#TODO - konfiguraco vlozit do konfiguracniho souboru, ktery umistim do home
#$postmanTemplateGitVersion="253ee9ed12f"
#$postmanTemplateGitVersion='b064a5f59fc'
#$postmanTemplateGitVersion='9d6defbf0ae';
#$postmanTemplateGitVersion='b064a5f59fc';

$postmanTemplateGitVersion='a742ddfc8ad';


$installDirectory=$home+"/tests/be/postman"
$protobufVersion='3.18'


rm ~/tmp-be-tests -r -Force
mkdir ~/tmp-be-tests

if ($args[0] -eq 'clean')
{
    rm $installDirectory/$postmanTemplateGitVersion -r -Force
}

if (!(Test-Path -Path $installDirectory/$postmanTemplateGitVersion)) 
{
    Push-Location
    cd ~/tmp-be-tests
    git clone https://$env:UserName@git.kb.cz/scm/tcoe/project-template-postman.git
    Push-Location
    cd project-template-postman
    git checkout $postmanTemplateGitVersion
    mkdir $installDirectory/$postmanTemplateGitVersion
    cp -r * $installDirectory/$postmanTemplateGitVersion
    Push-Location
    cd $installDirectory/$postmanTemplateGitVersion
    npm install
    cp templateConfig/runner_conf.json templateConfig/runner_conf.json.original
    Pop-Location
    Pop-Location
    Pop-Location

}

cp NOBY.postman_collection.json $installDirectory/$postmanTemplateGitVersion/postmanCollections

mkdir $installDirectory/$postmanTemplateGitVersion/dataSources
cp NOBY.testdata.xlsx $installDirectory/$postmanTemplateGitVersion/dataSources

rm $installDirectory/$postmanTemplateGitVersion/templateConfig/grpc_config.json
cp grpc_config.json $installDirectory/$postmanTemplateGitVersion/templateConfig


#Je treba rucne kvuli KB Proxy - nelze wgetem
while(!(Test-Path -Path "~/Downloads/protobuf-$protobufVersion.x.zip"))
{
    [console]::beep(1000,1000)
    echo "Please download https://github.com/protocolbuffers/protobuf/archive/refs/heads/$protobufVersion.x.zip to ~/Downloads" 
    Read-Host -Prompt 'Press enter when ready'
}

mkdir ~/tmp-be-tests/protobuf
Push-Location
cd ~/tmp-be-tests/protobuf
cp ~/Downloads/protobuf-$protobufVersion.x.zip .
unzip protobuf-$protobufVersion.x.zip

cd protobuf-$protobufVersion.x/src

rm $installDirectory/$postmanTemplateGitVersion/protofiles -r -Force
mkdir $installDirectory/$postmanTemplateGitVersion/protofiles/google

cp *.proto $installDirectory/$postmanTemplateGitVersion/protofiles
cp google/*.proto $installDirectory/$postmanTemplateGitVersion/protofiles/google

$workingDirectory=pwd
$workingDirectory=$workingDirectory.Path
$workingDirectoryLength=$workingDirectory.length

$protoDirs=Get-ChildItem google -recurse -directory | Select-Object FullName;
foreach($protoDir in $protoDirs)
{
    #Write-Host ($protoDir | Format-List | Out-String)
    $protoDir=$protoDir.FullName
    $protoDir=$protoDir.substring($workingDirectoryLength+1)
    echo $protoDir
    mkdir $installDirectory/$postmanTemplateGitVersion/protofiles/$protoDir
    cp $protoDir/*.proto $installDirectory/$postmanTemplateGitVersion/protofiles/$protoDir
}
Pop-Location

cp ../../../CIS/gRPC.CisTypes/Protos/* $installDirectory/$postmanTemplateGitVersion/protofiles

cp ../../../DomainServices/OfferService/Contracts/*.proto $installDirectory/$postmanTemplateGitVersion/protofiles
cp ../../../DomainServices/CustomerService/Contracts/*.proto $installDirectory/$postmanTemplateGitVersion/protofiles


Push-Location
cd $installDirectory/$postmanTemplateGitVersion/templateConfig
rm runner_conf.json
(Get-Content -path .\runner_conf.json.original).replace('database_oracledb.postman_collection.json','NOBY.postman_collection.json').replace('"__iterationData": "source_excel.xlsx"','"iterationData": "NOBY.testdata.xlsx"').replace('"reportPerSuite": true','"reportPerSuite": false') | Set-Content runner_conf.json
Pop-Location


#--------------------------------------------------------

$me=$MyInvocation.MyCommand.Name

$me=Get-Content -Path $me
$print=$false;

$componentTests=@{};
rm $installDirectory/tests/component -r -Force
mkdir $installDirectory/tests/component

$componentTests['01-OfferService']=@{};
    $componentTests['01-OfferService']['$expressGrpcConfigKey']='fomsOfferService';

$componentTests['02-CustomerService']=@{};
    $componentTests['02-CustomerService']['$expressGrpcConfigKey']='fomsCustomerService';

foreach($service in $componentTests.Keys)
{
    $script=@();
    foreach($line in $me)
    {

        if($line -eq '#---ComponentTestScriptEnd---')
        {
            $print=$false;
        }
        if($print)
        {
            $script = $script + $line;
        }
        if($line -eq '#---ComponentTestScriptStart---')
        {
            $print=$true;
        }
    }

    $script=($script).replace('$installDirectory',$installDirectory).replace('$postmanTemplateGitVersion',$postmanTemplateGitVersion).replace('$service',$service)
    foreach($maergeField in $componentTests[$service].Keys)
    {
        $script=($script).replace($maergeField,$componentTests[$service][$maergeField])
    }
    mkdir $installDirectory/tests/component/$service
    ($script) | Set-Content ($installDirectory+"/tests/component/"+$service+"/test.ps1");
}

[console]::beep(1000,1000)
return;
#Test scripts

#---ComponentTestScriptStart---

Push-Location

if($args[0] -eq $null)
{
    $passwd=Read-Host -Prompt '$service password';
}
else 
{
    $passwd=$args[0];
}

cd $installDirectory/$postmanTemplateGitVersion/testRunners
node.exe .\runTemplate.js --otherPwd.$expressGrpcConfigKey="$passwd" --folders $service --type $service
Pop-Location

[console]::beep(3000,500)

#---ComponentTestScriptEnd---






