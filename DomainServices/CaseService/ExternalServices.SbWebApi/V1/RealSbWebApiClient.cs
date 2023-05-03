using System.Globalization;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1.Contracts;
using DomainServices.UserService.Clients;

namespace DomainServices.CaseService.ExternalServices.SbWebApi.V1;

internal sealed class RealSbWebApiClient 
    : ISbWebApiClient
{
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
        await RequestHelper.ProcessResponse<WFS_CommonResponse>(httpResponse, x => x.Result, new List<(int ReturnVal, int ErrorCode)> { (2, ErrorCodeMapper.TaskIdNotFound) }, cancellationToken);
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

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/managetask/createtask", easRequest, cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<WFS_Manage_CreateTask_Response>(httpResponse, x => x.Result, cancellationToken: cancellationToken);

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
                Client_benefits = 0,
                Case_id = Convert.ToInt32(request.CaseId),//IT anal neni schopna rict co s tim
                Uver_id = Convert.ToInt32(request.CaseId),//IT anal neni schopna rict co s tim
                Loan_no = request.ContractNumber,
                Jmeno_prijmeni = request.ClientFullName,
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

    public async Task CompleteTask(Dto.CompleteTaskRequest request, CancellationToken cancellationToken = default)
    {
        var sbRequest = new WFS_Manage_CompleteTask
        {
            Task_id = request.TaskIdSb,
            Metadata = new List<WFS_MetadataItem>
            {
                new() { Mtdt_def = "ukol_mandant", Mtdt_val = "2", },
                new() { Mtdt_def = "ukol_uver_id", Mtdt_val = request.CaseId.ToString(CultureInfo.InvariantCulture), },
                new() { Mtdt_def = "ukol_dozadani_odpoved_oz", Mtdt_val = request.TaskUserResponse ?? string.Empty },
                new() { Mtdt_def = "wfl_refobj_dokumenty", Mtdt_val = string.Join(",", request.TaskDocumentIds) }
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/managetask/completetask", sbRequest, cancellationToken);

        await RequestHelper.ProcessResponse<CommonResult>(httpResponse, x => x, new List<(int ReturnVal, int ErrorCode)> { (2, ErrorCodeMapper.TaskIdNotFound) }, cancellationToken);
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

            if (string.IsNullOrEmpty(userInstance.CPM) || string.IsNullOrEmpty(userInstance.ICP))
            {
                return userInstance.UserIdentifiers.FirstOrDefault()?.Identity ?? "anonymous";
            }
            else
            {
                return $"CPM: {userInstance.CPM} ICP: {userInstance.ICP}";
            }
        }
        else
        {
            return "anonymous";
        }
    }

    private readonly HttpClient _httpClient;
    private readonly IUserServiceClient _userService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public RealSbWebApiClient(HttpClient httpClient, IUserServiceClient userService, CIS.Core.Security.ICurrentUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
        _userService = userService;
        _httpClient = httpClient;
    }
}
