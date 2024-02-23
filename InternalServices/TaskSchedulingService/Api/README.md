## grpcurl tests
grpcurl -insecure 127.0.0.1:30030 list
grpcurl -insecure 127.0.0.1:30030 grpc.health.v1.Health/Check

grpcurl -insecure -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30030 CIS.InternalServices.TaskSchedulingService.v1.TaskSchedulingService/GetJobs
grpcurl -insecure -d "{\"JobId\":\"D2166A1D-94A6-45A5-BA10-F54AA51B7538\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30030 CIS.InternalServices.TaskSchedulingService.v1.TaskSchedulingService/ExecuteJob

grpcurl -insecure -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30030 CIS.InternalServices.TaskSchedulingService.v1.TaskSchedulingService/GetJobs
grpcurl -insecure -d "{\"JobId\":\"A61AA97D-05C4-4D8F-B488-2EE35B5D2A9C\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30030 CIS.InternalServices.TaskSchedulingService.v1.TaskSchedulingService/ExecuteJob

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\TaskSchedulingService\Api\CIS.InternalServices.TaskSchedulingService.Api.csproj"