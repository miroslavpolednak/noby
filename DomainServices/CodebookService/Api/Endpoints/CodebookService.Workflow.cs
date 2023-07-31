using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CodebookService.Api.Endpoints;

internal partial class CodebookService
{
    public override Task<WorkflowConsultationMatrixResponse> WorkflowConsultationMatrix(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var xxdResult = _db.GetList<(int Kod, string Text)>(nameof(WorkflowConsultationMatrixResponse), 1);
            var matrix = _db.GetList<(int TaskSubtypeId, int ProcessTypeId, int ProcessPhaseId, bool IsConsultation)>(nameof(WorkflowConsultationMatrixResponse), 2);

            return (new WorkflowConsultationMatrixResponse()).AddItems(
                xxdResult.Select(t =>
                {
                    var item = new Contracts.v1.WorkflowConsultationMatrixResponse.Types.WorkflowConsultationMatrixItem
                    {
                        TaskSubtypeId = t.Kod,
                        TaskSubtypeName = t.Text
                    };
                    item.IsValidFor.AddRange(matrix
                        .Where(x => x.TaskSubtypeId == t.Kod)
                        .Select(x => new Contracts.v1.WorkflowConsultationMatrixResponse.Types.WorkflowConsultationMatrixItem.Types.WorkflowConsultationMatrixItemValidity
                        {
                            ProcessPhaseId = x.ProcessPhaseId,
                            ProcessTypeId = x.ProcessTypeId
                        })
                        .ToList());
                    return item;
                })
            );
        });

    public override Task<GenericCodebookResponse> WorkflowPriceExceptionDecisionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> WorkflowProcessType(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> WorkflowTaskCategories(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.WorkflowTaskCategory>();

    public override Task<GenericCodebookResponse> WorkflowTaskConsultationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> WorkflowTaskSigningResponseTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<WorkflowTaskStatesResponse> WorkflowTaskStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetList<WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem>(nameof(WorkflowTaskStates), 1);
            var extensions = _db.GetDynamicList(nameof(WorkflowTaskStates), 2);

            items.ForEach(item =>
            {
                byte? flag = extensions.FirstOrDefault(t => t.WorkflowTaskStateId == item.Id)?.Flag;
                item.Flag = flag.HasValue ? (WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag)flag : WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag.None;
            });
            return (new WorkflowTaskStatesResponse()).AddItems(items);
        });

    public override Task<WorkflowTaskStatesNobyResponse> WorkflowTaskStatesNoby(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<WorkflowTaskStatesNobyResponse, WorkflowTaskStatesNobyResponse.Types.WorkflowTaskStatesNobyItem>();

    public override Task<GenericCodebookResponse> WorkflowTaskTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();
}
