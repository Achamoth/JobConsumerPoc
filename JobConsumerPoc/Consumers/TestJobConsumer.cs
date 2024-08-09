using MassTransit.JobService;

namespace JobConsumerPoc.Consumers
{
    public class TestJobConsumer : IJobConsumer<JobMessage>
    {
        public async Task Run(JobContext<JobMessage> context)
        {
            var principal = Thread.CurrentPrincipal;
            Console.WriteLine($"Inside JobConsumer {Thread.CurrentPrincipal.Identity.AuthenticationType}");
            //await Task.Delay(context.Job.FreezeDuration);
            Console.WriteLine("Finished JobConsumer");
        }
    }

    public class JobMessage
    {
        public TimeSpan FreezeDuration { get; set; }
    }
}
