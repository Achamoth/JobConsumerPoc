using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace JobConsumerPoc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IBus _bus;

        public TestController(ILogger<TestController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpPost(Name = "PublishTestMessage")]
        public async Task PublishTestMessage(TestMessage message)
        {
            var sendEndpoint = await _bus.GetSendEndpoint(new Uri("queue:JobConsumerPocTestMessage"));
            await sendEndpoint.Send(message);
        }
    }

    public class TestMessage
    {
        public string Test { get; set; }
    }
}
