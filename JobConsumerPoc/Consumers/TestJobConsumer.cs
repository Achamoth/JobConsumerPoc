using MassTransit.JobService;

namespace JobConsumerPoc.Consumers
{
    public class TestJobConsumer : IJobConsumer<JobMessage>
    {
        public async Task Run(JobContext<JobMessage> context)
        {
            await Task.Delay(context.Job.FreezeDuration);
        }
    }

    public class JobMessage
    {
        public TimeSpan FreezeDuration { get; set; }
    }
}
