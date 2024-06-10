create table JobTypeSaga (
	CorrelationId uniqueidentifier primary key not null,
	CurrentState int not null,
	ActiveJobCount int not null,
	ConcurrentJobLimit int not null,
	OverrideJobLimit int null,
	OverrideLimitExpiration datetime null,
	ActiveJobs nvarchar(max) null,
	Instances nvarchar(max) null
)

create table JobSaga (
	CorrelationId uniqueidentifier primary key not null,
	CurrentState int not null,
	Submitted datetime null,
	ServiceAddress varchar(500) null,
	JobTimeout bigint null,
	Job nvarchar(max),
	JobTypeId uniqueidentifier not null,
	AttemptId uniqueidentifier not null,
	RetryAttempt int not null,
	[Started] datetime null,
	Completed datetime null,
	Duration bigint null,
	Faulted datetime null,
	Reason varchar(255) null,
	JobSlotWaitToken uniqueidentifier null,
	JobRetryDelayToken uniqueidentifier null
)

create table JobAttemptSaga (
	CorrelationId uniqueidentifier primary key not null,
	CurrentState int not null,
	JobId uniqueidentifier not null,
	RetryAttempt int not null,
	ServiceAddress varchar(500) not null,
	InstanceAddress varchar(500) not null,
	[Started] datetime null,
	Faulted datetime null,
	StatusCheckTokenId uniqueidentifier null
)