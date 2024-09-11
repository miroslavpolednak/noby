using DomainServices.CaseService.Api;
using DomainServices.CaseService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using ExternalServices.SbWebApi.V1;
using Google.Protobuf.WellKnownTypes;
using Moq;
using System.Globalization;

namespace DomainServices.CaseService.Tests.EndpointsTests;

public class CompleteTaskHandlerTests
{
    private readonly Mock<ISbWebApiClient> _sbWebApiClient;
    private readonly Mock<IDocumentOnSAServiceClient> _documentOnSAService;

    public CompleteTaskHandlerTests()
    {
        _sbWebApiClient = new Mock<ISbWebApiClient>();
        _documentOnSAService = new Mock<IDocumentOnSAServiceClient>();
    }

    [Theory]
    [InlineData(999, 1, "ukol_dozadani_odpoved_oz", "testovaci", "a,b")]
    [InlineData(998, 6, "ukol_podpis_odpoved_text", "testovaci", null)]
    [InlineData(997, 9, "ukol_retence_pozadavek", "testovaci", "c")]
    public async Task Handle_Should_ReturnEmpty(
        int taskId, 
        int taskTypeId, 
        string taskTypeStr, 
        string? taskUserResponse,
        string? taskDocumentIds)
    {
        var request = new CompleteTaskRequest
        {
            CaseId = 123456,
            TaskIdSb = taskId,
            TaskTypeId = taskTypeId,
            CompletionTypeId = 1,
            TaskUserResponse = taskUserResponse,
            TaskResponseTypeId = 1
        };
        if (!string.IsNullOrEmpty(taskDocumentIds))
        {
            request.TaskDocumentIds.AddRange(taskDocumentIds.Split(','));
        }

        var result = await getResult(request);

        // correct response
        result.Should().BeOfType<Empty>();

        // correct payload do SB
        string? userResponseTranslated = request.TaskUserResponse?.ReplacePipesToSb() ?? "";

        _sbWebApiClient.Verify(v => 
            v.CompleteTask(
                It.Is<ExternalServices.SbWebApi.Dto.CompleteTask.CompleteTaskRequest>(t => 
                    t.TaskIdSb == taskId
                    && t.Metadata["ukol_uver_id"] == "123456"
                    && t.Metadata["wfl_refobj_dokumenty"] == string.Join(",", request.TaskDocumentIds)
                    && (t.Metadata.ContainsKey(taskTypeStr) && t.Metadata[taskTypeStr] == userResponseTranslated)), 
                It.IsAny<CancellationToken>()), 
                Times.Once);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(6, true)]
    public async Task Handle_Should_SendSigningAttrs(int taskTypeId, bool mustContainKeys)
    {
        var request = new CompleteTaskRequest
        {
            CaseId = 123456,
            TaskIdSb = 999,
            TaskTypeId = taskTypeId,
            CompletionTypeId = 1,
            TaskResponseTypeId = 1
        };
        
        var result = await getResult(request);

        // pro podepisovani
        if (mustContainKeys)
        {
            _sbWebApiClient.Verify(v =>
                v.CompleteTask(
                    It.Is<ExternalServices.SbWebApi.Dto.CompleteTask.CompleteTaskRequest>(t =>
                        t.Metadata["ukol_podpis_odpoved_typ"] == request.TaskResponseTypeId.Value.ToString(CultureInfo.InvariantCulture)
                        && t.Metadata["ukol_podpis_zpusob_ukonceni"] == request.CompletionTypeId.Value.ToString(CultureInfo.InvariantCulture)),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
        }
        else
        {
            _sbWebApiClient.Verify(v =>
                v.CompleteTask(
                    It.Is<ExternalServices.SbWebApi.Dto.CompleteTask.CompleteTaskRequest>(t =>
                        !t.Metadata.ContainsKey("ukol_podpis_odpoved_typ")
                        && !t.Metadata.ContainsKey("ukol_podpis_zpusob_ukonceni")),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
        }
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(9, true)]
    public async Task Handle_Should_SendRetentionAttrs(int taskTypeId, bool mustContainKeys)
    {
        var request = new CompleteTaskRequest
        {
            CaseId = 123456,
            TaskIdSb = 999,
            TaskTypeId = taskTypeId,
            CompletionTypeId = 1,
            TaskResponseTypeId = 1
        };

        var result = await getResult(request);

        // pro podepisovani
        if (mustContainKeys)
        {
            _sbWebApiClient.Verify(v =>
                v.CompleteTask(
                    It.Is<ExternalServices.SbWebApi.Dto.CompleteTask.CompleteTaskRequest>(t => t.Metadata["ukol_retence_priprava_zpusob_uk"] == request.CompletionTypeId.Value.ToString(CultureInfo.InvariantCulture)),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
        }
        else
        {
            _sbWebApiClient.Verify(v =>
                v.CompleteTask(
                    It.Is<ExternalServices.SbWebApi.Dto.CompleteTask.CompleteTaskRequest>(t => !t.Metadata.ContainsKey("ukol_retence_priprava_zpusob_uk")),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
        }
    }

    [Fact]
    public async Task Handle_Should_CallDocumentOnSa()
    {
        var request = new CompleteTaskRequest
        {
            CaseId = 123456,
            TaskIdSb = 999,
            TaskTypeId = 6,
            CompletionTypeId = 2
        };

        var result = await getResult(request);

        _documentOnSAService
            .Verify(v =>
                v.SetProcessingDateInSbQueues(It.Is<long>(x => x == request.TaskId), It.Is<long>(x => x == request.CaseId), It.IsAny<CancellationToken>()),
                Times.Once);
    }

    [Theory]
    [InlineData(6, 1)]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(9, 2)]
    public async Task Handle_ShouldNot_CallDocumentOnSa(int taskTypeId, int? completionTypeId)
    {
        var request = new CompleteTaskRequest
        {
            CaseId = 123456,
            TaskIdSb = 999,
            TaskTypeId = taskTypeId,
            CompletionTypeId = completionTypeId
        };

        var result = await getResult(request);

        _documentOnSAService
            .Verify(v =>
                v.SetProcessingDateInSbQueues(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()),
                Times.Never);
    }

    private async Task<Empty?> getResult(CompleteTaskRequest request)
    {
        _sbWebApiClient
            .Setup(x => x.CompleteTask(It.IsAny<ExternalServices.SbWebApi.Dto.CompleteTask.CompleteTaskRequest>(), It.IsAny<CancellationToken>()));

        _documentOnSAService
            .Setup(x => x.SetProcessingDateInSbQueues(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()));

        var handler = new Api.Endpoints.v1.CompleteTask.CompleteTaskHandler(_sbWebApiClient.Object, _documentOnSAService.Object);
        return await handler.Handle(request, default);
    }
}
