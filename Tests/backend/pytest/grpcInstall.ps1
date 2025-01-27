
rm -r grpc
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


mkdir -p OfferService
Push-Location
cd OfferService
python -m grpc_tools.protoc -I..\..\..\..\..\CIS\gRPC.CisTypes\Protos\ -I..\..\..\..\..\DomainServices\OfferService\Contracts\ -I..\..\..\..\..\DomainServices\OfferService\Contracts\Mortgage --python_out=. --grpc_python_out=. ..\..\..\..\..\DomainServices\OfferService\Contracts\Mortgage\*proto
python -m grpc_tools.protoc -I..\..\..\..\..\CIS\gRPC.CisTypes\Protos\ -I..\..\..\..\..\DomainServices\OfferService\Contracts\ -I..\..\..\..\..\DomainServices\OfferService\Contracts\Mortgage --python_out=. --grpc_python_out=. ..\..\..\..\..\DomainServices\OfferService\Contracts\*proto
Pop-Location


Pop-Location
