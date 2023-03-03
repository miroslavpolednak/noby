using System.ComponentModel.DataAnnotations;
using Avro.Specific;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.notificationreport;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Mock.Mcs.Controllers;

[ApiController]
[Route("[controller]")]
public class ResultController : ControllerBase
{
    private readonly ITopicProducer<ISpecificRecord> _producer;
    private readonly ILogger<ResultController> _logger;

    public ResultController(
        ITopicProducer<ISpecificRecord> producer,
        ILogger<ResultController> logger
    )
    {
        _producer = producer;
        _logger = logger;
    }

    [HttpPost]
    public async Task Produce([FromQuery] int count, [FromQuery] int delayMs)
    {
        for (int i = 0; i < count; i++)
        {
            await _producer.Produce(new NotificationReport
            {
                id = Guid.NewGuid().ToString(),
                channel = new Channel
                {
                    id = "SMS"
                },
                state = "SENT",
                exactlyOn = DateTime.Now,
                notificationErrors = new List<NotificationError>()
            });
            
            await Task.Delay(TimeSpan.FromMilliseconds(delayMs));
        }
    }
}