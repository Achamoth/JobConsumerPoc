using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using JobConsumerPoc.Consumers;
using JobConsumerPoc.Mappings;
using MassTransit;
using MassTransit.Definition;
using MassTransit.JobService.Components.StateMachines;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace JobConsumerPoc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var defaultBusConnectionString = builder.Configuration.GetConnectionString("DefaultBusConnectionString");

            ConfigureNHibernate(builder.Services, builder.Configuration);

            // Add services to the container.
            builder.Services.AddMassTransit(configure =>
            {
                configure.AddSagaRepository<JobSaga>().NHibernateRepository();
                configure.AddSagaRepository<JobTypeSaga>().NHibernateRepository();
                configure.AddSagaRepository<JobAttemptSaga>().NHibernateRepository();

                configure.AddDelayedMessageScheduler();
                configure.AddConsumer<TestMessageConsumer>();
                configure.AddConsumer<TestJobConsumer>();
                configure.AddRequestClient<JobMessage>();

                if (defaultBusConnectionString.StartsWith("Endpoint"))
                {
                    configure.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.UseDelayedMessageScheduler();
                        cfg.Host(defaultBusConnectionString);

                        cfg.ServiceInstance(instance =>
                        {
                            instance.ConfigureJobServiceEndpoints(jfc =>
                            {
                                jfc.ConfigureSagaRepositories(context);
                            });
                            instance.ConfigureEndpoints(context, f =>
                            {
                                f.Include<TestJobConsumer>();
                            });
                        });

                        cfg.ConfigureEndpoints(context, new DefaultEndpointNameFormatter("JobConsumerPoc", false));
                    });
                }

                else if (defaultBusConnectionString.StartsWith("amqp://"))
                {
                    configure.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.UseDelayedMessageScheduler();
                        cfg.Host(defaultBusConnectionString);

                        cfg.ServiceInstance(instance =>
                        {
                            instance.ConfigureJobServiceEndpoints(jfc =>
                            {
                                jfc.ConfigureSagaRepositories(context);
                            });
                            instance.ConfigureEndpoints(context, f =>
                            {
                                f.Include<TestJobConsumer>();
                            });
                        });

                        cfg.ConfigureEndpoints(context, new DefaultEndpointNameFormatter("JobConsumerPoc", false));
                    });
                }
            });
            builder.Services.AddMassTransitHostedService();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        public static void ConfigureNHibernate(IServiceCollection services, IConfiguration configuration)
        {
            //var mapper = new ModelMapper();
            //mapper.AddMappings([typeof(JobSagaMap), typeof(JobAttemptSagaMap), typeof(JobTypeSagaMap)]);
            //var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            //var dbConfiguration = new Configuration();
            //dbConfiguration.DataBaseIntegration(c =>
            //{
            //    c.Dialect<MsSql2012Dialect>();
            //    c.Driver<MicrosoftDataSqlClientDriver>();
            //    c.ConnectionString = configuration.GetConnectionString("DatabaseConnectionString");
            //    c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            //    c.SchemaAction = SchemaAutoAction.Validate;
            //});
            //dbConfiguration.AddMapping(domainMapping);

            var sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .ConnectionString(configuration.GetConnectionString("DatabaseConnectionString"))
                    .Driver<MicrosoftDataSqlClientDriver>()
                    .Dialect<MsSql2012Dialect>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<JobSagaMap>())
                .BuildSessionFactory();
            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());
        }
    }
}
