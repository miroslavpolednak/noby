using System.Text.Encodings.Web;
using System.Text.Json;
using ExternalServices.SbWebApi.Dto.CompleteTask;
using ExternalServices.SbWebApi.Dto.UpdateTask;
using ExternalServices.SbWebApi.V1.Contracts;
using DomainServices.UserService.Clients;
using System.Globalization;
using System.Text.Json.Serialization;
using ExternalServices.SbWebApi.Dto.Refinancing;

namespace ExternalServices.SbWebApi.V1;

internal sealed class RealSbWebApiClient
    : ISbWebApiClient
{
    public async Task<(decimal InterestRate, int? NewFixationTime)> GetRefixationInterestRate(long caseId, DateTime interestRateValidTo, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/api/refixationservices/getinterestrate?uver_id={caseId}&date={interestRateValidTo:yyyy-MM-dd}&fixation_time", cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<GetInterestRate_response>(httpResponse, x => x.Result, cancellationToken: cancellationToken);

        return (Convert.ToDecimal(responseObject.Interest_rate, CultureInfo.InvariantCulture), responseObject.Fixation_time_new);
    }

    public async Task<IList<IReadOnlyDictionary<string, string>>> FindTasksByContractNumber(Dto.FindTasks.FindByContractNumberRequest request, CancellationToken cancellationToken = default)
    {
        var easRequest = new WFS_Request_ByContractNo
        {
            Header = RequestHelper.MapEasHeader(await getLogin(cancellationToken)),
            Message = new WFS_Find_ByContractNo
            {
                Contract_no = request.ContractNumber,
                Task_state = request.TaskStates,
                Search_pattern = request.SearchPattern
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/findtasks/bycontractno", easRequest, cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<WFS_Find_Response>(httpResponse, x => x.Result, new List<(int ReturnVal, int ErrorCode)> { (2, ErrorCodeMapper.ContractNumberSbNotFound) }, cancellationToken);

        return RequestHelper.MapTasksToDictionary(responseObject.Tasks);
    }

    public async Task CancelTask(int taskIdSB, CancellationToken cancellationToken = default(CancellationToken))
    {
        // vytvoreni EAS requestu
        var easRequest = new WFS_Request_CancelTask
        {
            Header = RequestHelper.MapEasHeader(await getLogin(cancellationToken)),
            Message = new WFS_Manage_CancelTask
            {
                Task_id = taskIdSB
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/managetask/canceltask", easRequest, cancellationToken);
        await RequestHelper.ProcessResponse<WFS_CommonResponse>(httpResponse, x => x.Result, new List<(int ReturnVal, int ErrorCode)>
        {
            (2, ErrorCodeMapper.TaskIdNotFound)
        }, cancellationToken);
    }

    public async Task<Dto.CreateTask.CreateTaskResponse> CreateTask(Dto.CreateTask.CreateTaskRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var easRequest = new WFS_Request_CreateTask
        {
            Header = RequestHelper.MapEasHeader(await getLogin(cancellationToken)),
            Message = new WFS_Manage_CreateTask
            {
                Parent_task_set = request.ProcessId,
                Task_type_noby = request.TaskTypeId,
                Metadata = request.Metadata.Select(t => new WFS_MetadataItem
                {
                    Mtdt_def = t.Key,
                    Mtdt_val = t.Value
                }).ToList()
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/managetask/createtask", easRequest, _jsonSerializerOptions, cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<WFS_Manage_CreateTask_Response>(httpResponse,
                                                                                                 x => x.Result,
                                                                                                 returnVal2ErrorCodesMapping: [(6792, ErrorCodeMapper.RefinancingError)],
                                                                                                 cancellationToken: cancellationToken);

        return new Dto.CreateTask.CreateTaskResponse
        {
            TaskIdSB = responseObject.Task_id.GetValueOrDefault(),
            TaskId = responseObject.Process_id.GetValueOrDefault()
        };
    }

    public async Task<Dto.CaseStateChanged.CaseStateChangedResponse> CaseStateChanged(Dto.CaseStateChanged.CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        // vytvoreni EAS requestu
        var easRequest = new WFS_Request_CaseStateChanged
        {
            Header = RequestHelper.MapEasHeader(await getLogin(cancellationToken)),
            Message = new()
            {
                Client_benefits = request.IsEmployeeBonusRequested.HasValue ? (request.IsEmployeeBonusRequested.Value ? 1 : 0) : null,
                Case_id = Convert.ToInt32(request.CaseId),//IT anal nevi co s tim
                Uver_id = Convert.ToInt32(request.CaseId),//IT anal nevi co s tim
                Loan_no = request.ContractNumber,
                Jmeno_prijmeni = request.ClientFullName,
                Case_state_id = request.CaseStateId,
                Case_state = request.CaseStateName,
                Product_type = request.ProductTypeId,
                Owner_cpm = request.OwnerUserCpm,
                Owner_icp = request.OwnerUserIcp,
                Mandant = (int)request.Mandant,
                Risk_business_case_id = request.RiskBusinessCaseId
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/eventreport/casestatechanged", easRequest, cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<WFS_Event_Response>(httpResponse, x => x.Result, cancellationToken: cancellationToken);

        return new Dto.CaseStateChanged.CaseStateChangedResponse
        {
            RequestId = responseObject.Request_id
        };
    }

    public async Task CompleteTask(CompleteTaskRequest request, CancellationToken cancellationToken = default)
    {
        var sbRequest = new WFS_Request_CompleteTask
        {
            Header = RequestHelper.MapEasHeader(await getLogin(cancellationToken)),
            Message = new WFS_Manage_CompleteTask
            {
                Task_id = request.TaskIdSb,
                Metadata = request.Metadata.Select(t => new WFS_MetadataItem
                {
                    Mtdt_def = t.Key,
                    Mtdt_val = t.Value
                }).ToList()
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/managetask/completetask", sbRequest, _jsonSerializerOptions, cancellationToken);

        await RequestHelper.ProcessResponse<WFS_CommonResponse>(httpResponse, x => x.Result, new List<(int ReturnVal, int ErrorCode)> { (2, ErrorCodeMapper.TaskIdNotFound) }, cancellationToken);
    }

    public async Task<IList<IReadOnlyDictionary<string, string>>> FindTasksByCaseId(Dto.FindTasks.FindByCaseIdRequest request, CancellationToken cancellationToken = default)
    {
        var easRequest = new WFS_Request_ByCaseId
        {
            Header = RequestHelper.MapEasHeader(await getLogin(cancellationToken)),
            Message = new WFS_Find_ByCaseId
            {
                Case_id = Convert.ToInt32(request.CaseId),//IT anal neni schopna rict co s tim
                Search_pattern = request.SearchPattern,
                Task_state = request.TaskStates
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/findtasks/bycaseid", easRequest, cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<WFS_Find_Response>(httpResponse, x => x.Result, cancellationToken: cancellationToken);

        return RequestHelper.MapTasksToDictionary(responseObject.Tasks);
    }

    public async Task<IList<IReadOnlyDictionary<string, string>>> FindTasksByTaskId(Dto.FindTasks.FindByTaskIdRequest request, CancellationToken cancellationToken = default)
    {
        var easRequest = new WFS_Request_ByTaskId
        {
            Header = RequestHelper.MapEasHeader(await getLogin(cancellationToken)),
            Message = new WFS_Find_ByTaskId
            {
                Task_id = request.TaskIdSb,
                Task_state = request.TaskStates
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/findtasks/bytaskid", easRequest, cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<WFS_Find_Response>(httpResponse, x => x.Result, new List<(int ReturnVal, int ErrorCode)> { (2, ErrorCodeMapper.TaskIdNotFound) }, cancellationToken);

        return RequestHelper.MapTasksToDictionary(responseObject.Tasks);
    }

    private async Task<string> getLogin(CancellationToken cancellationToken)
    {
        int? userId = _userAccessor?.User?.Id;

        if (userId.HasValue)
        {
            // get current user's login
            var userInstance = await _userService.GetUser(userId.Value, cancellationToken);

            if (string.IsNullOrEmpty(userInstance.UserInfo.Cpm) || string.IsNullOrEmpty(userInstance.UserInfo.Icp))
            {
                var s = userInstance.UserIdentifiers
                    .Where(t => t.IdentityScheme is SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.Kbad or SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.Mpad)
                    .FirstOrDefault()?.Identity ?? "anonymous";
                var idx = s.IndexOf('\\');
                return idx > 0 ? s[(idx + 1)..] : s;
            }
            else
            {
                return $"{userInstance.UserInfo.Cpm}_{userInstance.UserInfo.Icp}";
            }
        }
        else
        {
            return "anonymous";
        }
    }

    public async Task UpdateTask(UpdateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var sbRequest = new WFS_Request_UpdateTask
        {
            Header = RequestHelper.MapEasHeader(await getLogin(cancellationToken)),
            Message = new WFS_Manage_UpdateTask
            {
                Task_id = request.TaskIdSb,
                Metadata = request.Metadata.Where(m => m.Value is not null).Select(t => new WFS_MetadataItem
                {
                    Mtdt_def = t.Key,
                    Mtdt_val = t.Value
                }).ToList()
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/managetask/updatetask", sbRequest, cancellationToken);

        await RequestHelper.ProcessResponse<WFS_CommonResponse>(httpResponse, x => x.Result, new List<(int ReturnVal, int ErrorCode)> { (2, ErrorCodeMapper.TaskIdNotFound) }, cancellationToken);
    }

    public async Task<string?> GenerateRetentionDocument(GenerateRetentionDocumentRequest request, CancellationToken cancellationToken = default)
    {
        var sbRequest = new RetentionAppendix_request
        {
            Case_id = (int?)request.CaseId,
            Interest_rate = (double?)request.InterestRate,
            Date_from = request.DateFrom,
            Payment_amount = (double?)request.PaymentAmount,
            Print_signature_form = request.SignatureTypeDetailId,
            Cpm = request.Cpm,
            Icp = request.Icp,
            Deadline_for_signature = request.SignatureDeadline,
            Individual_pricing = request.IndividualPricing,
            Fee = (double?)request.Fee
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/api/refixationservices/retentionappendix", sbRequest, cancellationToken);
        var responseObject = await RequestHelper.ProcessResponse<RetentionAppendix_response>(httpResponse, x => x.Result, cancellationToken: cancellationToken);
        return responseObject?.Ea_number;
    }

    public async Task<string?> GenerateHedgeAppendixDocument(GenerateRefixationDocumentRequest request, CancellationToken cancellationToken = default)
    {
        var sbRequest = new HedgeRefixationAppendix_request
        {
            Case_id = (int?)request.CaseId,
            Interest_rate = (double?)request.InterestRateProvided,
            Date_from = request.FixedRateValidTo,
            Fixation_time = request.FixedRatePeriod,
            Payment_amount = (double?)request.PaymentAmount,
            Print_signature_form = request.SignatureTypeDetailId,
            Cpm = request.Cpm,
            Icp = request.Icp,
            Deadline_for_signature = request.SignatureDeadline,
            Individual_pricing = request.IndividualPricing
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/api/refixationservices/hedgrefixationappendix", sbRequest, cancellationToken);
        var responseObject = await RequestHelper.ProcessResponse<HedgeRefixationAppendix_response>(httpResponse, x => x.Result, cancellationToken: cancellationToken);

        return responseObject.Ea_number;
    }

    public async Task<string?> GenerateInterestRateNotificationDocument(GenerateInterestRateNotificationDocumentRequest documentRequest, CancellationToken cancellationToken = default)
    {
        var sbRequest = new InterestRateNotification_request
        {
            Case_id = (int?)documentRequest.CaseId,
            Interest_rate = (double?)documentRequest.InterestRateProvided,
            Date_from = documentRequest.FixedRateValidTo,
            Fixation_time = documentRequest.FixedRatePeriod,
            Payment_amount = (double?)documentRequest.PaymentAmount,
            Print_signature_form = documentRequest.SignatureTypeDetailId,
            Cpm = documentRequest.Cpm,
            Icp = documentRequest.Icp
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/api/refixationservices/interestratenotification", sbRequest, cancellationToken);
        var responseObject = await RequestHelper.ProcessResponse<InterestRateNotification_response>(httpResponse, x => x.Result, cancellationToken: cancellationToken);

        return responseObject.Ea_number;
    }

    public async Task<ICollection<string>?> GenerateCalculationDocuments(GenerateCalculationDocumentsRequest request, CancellationToken cancellationToken = default)
    {
        var sbRequest = new CalculationDocuments_request
        {
            Case_id = (int?)request.CaseId,
            Full_repayment = request.IsExtraPaymentFullyRepaid,
            Calculation_date = DateTime.UtcNow,
            Extra_payment_date = request.ExtraPaymentDate,
            Client_id = request.ClientKbId.ToString(CultureInfo.InvariantCulture),
            Extra_payment_sum = (double?)request.ExtraPaymentAmount,
            Uvn_amount = (double?)request.FeeAmount,
            Principal_amount = (double?)request.PrincipalAmount,
            Interest_amount = (double?)request.InterestAmount,
            Other_unpaid_fees = (double?)request.OtherUnpaidFees,
            Interest_on_late = (double?)request.InterestOnLate,
            Interest_covid = (double?)request.InterestCovid,
            Loan_overdue = request.IsLoanOverdue,
            Payment_reduction = request.IsInstallmentReduced,
            New_maturity_date = request.NewMaturityDate,
            New_payment_amount = (double?)request.NewPaymentAmount,
            Print_signature_form = request.HandoverTypeDetailCode
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/api/refixationservices/calculationdocuments", sbRequest, cancellationToken);
        var responseObject = await RequestHelper.ProcessResponse<CalculationDocuments_response>(httpResponse, x => x.Result, cancellationToken: cancellationToken);

        return responseObject.Ea_numbers;
    }

    private readonly HttpClient _httpClient;
    private readonly IUserServiceClient _userService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public RealSbWebApiClient(HttpClient httpClient, IUserServiceClient userService, CIS.Core.Security.ICurrentUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
        _userService = userService;
        _httpClient = httpClient;
    }
}
