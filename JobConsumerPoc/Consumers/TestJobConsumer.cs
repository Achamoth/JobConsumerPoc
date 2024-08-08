using MassTransit.JobService;

namespace JobConsumerPoc.Consumers
{
    public class TestJobConsumer : IJobConsumer<JobMessage>
    {
        public async Task Run(JobContext<JobMessage> context)
        {
            Console.WriteLine("Inside JobConsumer");
            await Task.Delay(context.Job.FreezeDuration);
            Console.WriteLine("Finished JobConsumer");
        }
    }

    public class JobMessage
    {
        public TimeSpan FreezeDuration { get; set; }
    }
}
