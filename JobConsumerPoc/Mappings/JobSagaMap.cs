using FluentNHibernate.Mapping;
using MassTransit.JobService.Components.StateMachines;
using NHibernate.Linq;

namespace JobConsumerPoc.Mappings
{
    public class JobSagaMap : ClassMap<JobSaga>
    {
        public JobSagaMap()
        {
            Schema("dbo");
            Table("JobSaga");
            Id(x => x.CorrelationId);
            Map(x => x.CurrentState).Not.Nullable();
            Map(x => x.Submitted).Nullable();
            Map(x => x.ServiceAddress).Nullable();
            Map(x => x.JobTimeout).MappedAs(new NHibernate.Type.TimeSpanType());
            Map(x => x.Job).CustomType<JsonDictionary<string, object>>().Nullable();
            Map(x => x.JobTypeId).Not.Nullable();
            Map(x => x.AttemptId).Not.Nullable();
            Map(x => x.RetryAttempt).Not.Nullable();
            Map(x => x.Started).Nullable();
            Map(x => x.Completed).Nullable();
            Map(x => x.Duration).Nullable();
            Map(x => x.Faulted).Nullable();
            Map(x => x.Reason).Nullable();
            Map(x => x.JobSlotWaitToken).Nullable();
            Map(x => x.JobRetryDelayToken).Nullable();
        }
    }
}
