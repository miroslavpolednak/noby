#gc.ps1 gRPC Caller
#Jakub Vana (jakub.vana@mpss.cz/jaku_vana@kb.cz)

$nobyEnv=$args[0]
$nobyEnv=$nobyEnv.Split('.')
$nobyEnvNode=$nobyEnv[1]
$nobyEnv=$nobyEnv[0]
$serviceMethod=$args[1]
$serviceMethod=$serviceMethod.Split('.')
$service=$serviceMethod[0]
$method=$serviceMethod[1]
$dataFile=$args[2]

& ~\.gc\conf.ps1
#echo "$headers" 
#echo "$serverPort" 
#echo "$fullMethod"

$data=Get-Content $dataFile

#Replace mergefields by commnad line arguments
for ($i=3;$i -lt $args.Length; $i++)
{
    $replace=$args[$i]
    $replace=$replace.Split("=")

    $mf=$replace[0]
    $value=$replace[1]
    $repString='\${'+$mf+'(\|.*)?}'
    #$repString='\${'+$mf+'}'
    $data=$data -replace ($repString,$value)
    #echo $data
}

#Replace mergefields by defaults
$data=$data -replace ('\${.*\|(.*)}','$1')

echo "Data in:"
echo $data

echo "Data out:"
echo $data | grpcurl -insecure -d '@' -H "$headers" "$serverPort" "$fullMethod"



