using JobConsumerPoc.Controllers;
using MassTransit;
using MassTransit.Contracts.JobService;
using System.Security.Claims;

namespace JobConsumerPoc
{
    public class MassTransitConsumeObserver : IConsumeObserver
    {
        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            if (context.Message is TestMessage || context.Message is StartJob)
            {
                Console.WriteLine($"Inside consume observer {context.GetType()}");
                var identity = new ClaimsIdentity($"{context.GetType()}", $"{context.GetType()}", "Role");
                Thread.CurrentPrincipal = new ClaimsPrincipal(identity);
            }
            return Task.CompletedTask;
        }
    }
}
