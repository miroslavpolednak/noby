using System.ComponentModel.DataAnnotations;
using Avro.Specific;
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
    public string Produce()
    {
        throw new NotImplementedException();
    }
}