using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1.Contracts;

namespace DomainServices.CaseService.ExternalServices.SbWebApi.V1;

internal sealed class RealSbWebApiClient : ISbWebApiClient
{
    private readonly HttpClient _httpClient;

    public RealSbWebApiClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task CancelTask(Dto.CancelTask.CancelTaskRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        // vytvoreni EAS requestu
        var easRequest = new WFS_Request_CaseStateChanged
        {
            Header = RequestHelper.MapEasHeader(request.Login)
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/managetask/canceltask", easRequest, cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<WFS_Event_Response>(httpResponse, x => x.Result, cancellationToken);

        switch (responseObject.Result?.Return_val ?? -1)
        {
            case 0:
                return;
            case 2:
                throw new CIS.Core.Exceptions.CisNotFoundException(0, responseObject.Result?.Return_text ?? "");
            default:
                throw new CisExtServiceServerErrorException(StartupExtensions.ServiceName, _httpClient.BaseAddress + "/wfs/managetask/canceltask", responseObject.Result?.Return_text ?? "");
        }
    }
    
    public async Task<Dto.CreateTask.CreateTaskResponse> CreateTask(Dto.CreateTask.CreateTaskRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var easRequest = new WFS_Request_CreateTask
        {
            Header = RequestHelper.MapEasHeader(request.Login),
            Message = new WFS_Manage_CreateTask
            {
                Task_type = request.TaskTypeId,
                Parent_task_set = request.ProcessId,
                Metadata = request.Metadata.Select(t => new WFS_MetadataItem
                {
                    Mtdt_def = t.Key,
                    Mtdt_val = t.Value
                }).ToList()
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/managetask/createtask", easRequest, cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<WFS_Manage_CreateTask_Response>(httpResponse, x => x.Result, cancellationToken);

        if ((responseObject.Result?.Return_val ?? -1) == 0)
        {
            return new Dto.CreateTask.CreateTaskResponse
            {
                TaskIdSB = responseObject.Task_id.GetValueOrDefault(),
                //TaskId = responseObject
            };
        }
        else
        {
            throw new CisExtServiceServerErrorException(StartupExtensions.ServiceName, _httpClient.BaseAddress + "/wfs/managetask/createtask", responseObject.Result?.Return_text ?? "");
        }
    }

    public async Task<Dto.CaseStateChanged.CaseStateChangedResponse> CaseStateChanged(Dto.CaseStateChanged.CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        // vytvoreni EAS requestu
        var easRequest = new WFS_Request_CaseStateChanged
        {
            Header = RequestHelper.MapEasHeader(request.Login),
            Message = new()
            {
                Client_benefits = 0,
                Case_id = request.CaseId,
                Uver_id = request.CaseId,
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

        var responseObject = await RequestHelper.ProcessResponse<WFS_Event_Response>(httpResponse, x => x.Result, cancellationToken);

        return new Dto.CaseStateChanged.CaseStateChangedResponse
        {
            RequestId = responseObject.Request_id
        };
    }

    public async Task<FindTasksResponse> FindTasksByCaseId(FindByCaseIdRequest request, CancellationToken cancellationToken = default)
    {
        var easRequest = new WFS_Request_ByCaseId
        {
            Header = RequestHelper.MapEasHeader(request.Login),
            Message = new WFS_Find_ByCaseId
            {
                Case_id = request.CaseId,
                Search_pattern = request.SearchPattern,
                Task_state = request.TaskStates
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/findtasks/bycaseid", easRequest, cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<WFS_Find_Response>(httpResponse, x => x.Result, cancellationToken);

        return new FindTasksResponse
        {
            ItemsFound = responseObject.Items_found ?? 0,
            Tasks = RequestHelper.MapTasksToDictionary(responseObject.Tasks)
        };
    }

    public async Task<FindTasksResponse> FindTasksByTaskId(FindByTaskIdRequest request, CancellationToken cancellationToken = default)
    {
        var easRequest = new WFS_Request_ByTaskId
        {
            Header = RequestHelper.MapEasHeader(request.Login),
            Message = new WFS_Find_ByTaskId
            {
                Task_id = request.TaskSbId,
                Task_state = request.TaskStates
            }
        };

        var httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/findtasks/bytaskid", easRequest, cancellationToken);

        var responseObject = await RequestHelper.ProcessResponse<WFS_Find_Response>(httpResponse, x => x.Result, cancellationToken);

        return new FindTasksResponse
        {
            ItemsFound = responseObject.Items_found ?? 0,
            Tasks = RequestHelper.MapTasksToDictionary(responseObject.Tasks)
        };
    }
}
