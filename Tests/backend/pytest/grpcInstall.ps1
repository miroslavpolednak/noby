
rm grpc
mkdir -p grpc

Push-Location
cd grpc

mkdir -p CisTypes
Push-Location
cd CisTypes
python -m grpc_tools.protoc -I..\..\..\..\..\CIS\gRPC.CisTypes\Protos\   --python_out=. --grpc_python_out=. ..\..\..\..\..\CIS\gRPC.CisTypes\Protos\*proto
Pop-Location

mkdir -p CaseService
Push-Location
cd CaseService
python -m grpc_tools.protoc -I..\..\..\..\..\CIS\gRPC.CisTypes\Protos\ -I..\..\..\..\..\DomainServices\CaseService\Contracts\  --python_out=. --grpc_python_out=. ..\..\..\..\..\DomainServices\CaseService\Contracts\*proto
Pop-Location


Pop-Location
