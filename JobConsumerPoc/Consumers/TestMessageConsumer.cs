using JobConsumerPoc.Controllers;
using MassTransit;
using ISession = NHibernate.ISession;

namespace JobConsumerPoc.Consumers
{
    public class TestMessageConsumer : IConsumer<TestMessage>
    {
        private readonly ISession _session;
        private readonly IRequestClient<JobMessage> _requestClient;
        private readonly IBus _bus;
        private readonly ILogger<TestMessageConsumer> _logger;

        public TestMessageConsumer(
            ISession session,
            IRequestClient<JobMessage> requestClient,
            IBus bus,
            ILogger<TestMessageConsumer> logger)
        {
            _session = session;
            _requestClient = requestClient;
            _bus = bus;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TestMessage> context)
        {
            var principal = Thread.CurrentPrincipal;
            _logger.LogInformation($"Message received: {context.Message.Test} {Thread.CurrentPrincipal.Identity.AuthenticationType}");
            try
            {
                // Use request client if you want to be able to track the job
                //var response = await _requestClient.GetResponse<JobSubmissionAccepted>(new JobMessage
                //{
                //    FreezeDuration = TimeSpan.FromMinutes(1)
                //});
                await _bus.Publish(new JobMessage { FreezeDuration = TimeSpan.FromMinutes(1) });
                _logger.LogInformation("Request created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request creation failed.");
            }
        }
    }
}
