using FluentNHibernate.Mapping;
using MassTransit.JobService.Components.StateMachines;

namespace JobConsumerPoc.Mappings
{
    public class JobTypeSagaMap : ClassMap<JobTypeSaga>
    {
        public JobTypeSagaMap()
        {
            Schema("dbo");
            Table("JobTypeSaga");
            Not.LazyLoad();
            Id(x => x.CorrelationId);
            Map(x => x.CurrentState).Not.Nullable();
            Map(x => x.ActiveJobCount).Not.Nullable();
            Map(x => x.ConcurrentJobLimit).Not.Nullable();
            Map(x => x.OverrideJobLimit).Nullable();
            Map(x => x.OverrideLimitExpiration).Nullable();
            Map(x => x.ActiveJobs).CustomType<JsonList<ActiveJob>>();
            Map(x => x.Instances).CustomType<JsonDictionary<Uri, JobTypeInstance>>();
        }
    }
}
