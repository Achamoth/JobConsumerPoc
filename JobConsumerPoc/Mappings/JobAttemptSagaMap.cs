using FluentNHibernate.Mapping;
using MassTransit.JobService.Components.StateMachines;

namespace JobConsumerPoc.Mappings
{
    public class JobAttemptSagaMap : ClassMap<JobAttemptSaga>
    {
        public JobAttemptSagaMap()
        {
            Schema("dbo");
            Table("JobAttemptSaga");
            Not.LazyLoad();
            Id(x => x.CorrelationId);
            Map(x => x.CurrentState).Not.Nullable();
            Map(x => x.JobId).Not.Nullable();
            Map(x => x.RetryAttempt).Not.Nullable();
            Map(x => x.ServiceAddress).Not.Nullable().CustomType<NHibernate.Type.UriType>();
            Map(x => x.InstanceAddress).Not.Nullable().CustomType<NHibernate.Type.UriType>();
            Map(x => x.Started).Nullable();
            Map(x => x.Faulted).Nullable();
            Map(x => x.StatusCheckTokenId).Nullable();
        }
    }
}
