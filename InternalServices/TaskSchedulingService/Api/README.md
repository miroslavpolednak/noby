## grpcurl tests
grpcurl -insecure 127.0.0.1:30030 list
grpcurl -insecure 127.0.0.1:30030 grpc.health.v1.Health/Check

grpcurl -insecure -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30030 CIS.InternalServices.TaskSchedulingService.v1.TaskSchedulingService/GetJobs
grpcurl -insecure -d "{\"JobId\":\"D2166A1D-94A6-45A5-BA10-F54AA51B7538\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30030 CIS.InternalServices.TaskSchedulingService.v1.TaskSchedulingService/ExecuteJob

grpcurl -insecure -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30023 CIS.InternalServices.TaskSchedulingService.v1.TaskSchedulingService/GetJobs
grpcurl -insecure -d "{\"JobId\":\"f54733b7-cbbd-4834-8be3-2f005da87896\",\"JobData\":\"[\\\"CB_CmTrTinFormat\\\",\\\"CB_CmTrTinCountry\\\",\\\"CB_SourceOfEarningsVsProfessionCategory\\\",\\\"CB_HyporetenceResponse\\\",\\\"CB_IdentificationMethodType\\\",\\\"CB_CmEpProfessionCategory\\\",\\\"CB_StandardMethodOfArrAcceptanceByNPType\\\",\\\"MAP_CB_CmEpProfessionCategory_CB_CmEpProfession\\\",\\\"CB_CmCoPhoneIdc\\\"]\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30023 CIS.InternalServices.TaskSchedulingService.v1.TaskSchedulingService/ExecuteJob
grpcurl -insecure -d "{\"PageSize\":10,\"Page\":1,\"ScheduleJobStatusId\":\"dba14dc0-a403-4e28-abb9-c9375bc6004f\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30023 CIS.InternalServices.TaskSchedulingService.v1.TaskSchedulingService/GetJobStatuses

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\TaskSchedulingService\Api\CIS.InternalServices.TaskSchedulingService.Api.csproj"