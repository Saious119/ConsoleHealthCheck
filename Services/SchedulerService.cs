using ConsoleHealthCheck.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace ConsoleHealthCheck.Services
{
    public class SchedulerService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private IScheduler _scheduler;

        public SchedulerService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        public async Task StartAsync()
        {
            _scheduler = await _schedulerFactory.GetScheduler();
            _scheduler.JobFactory = _jobFactory;
            await _scheduler.Start();
        }

        public async Task ScheduleJob(string consoleName, string cronExpression)
        {
            var jobDataMap = new JobDataMap();
            jobDataMap.Put("consoleName", consoleName);

            var job = JobBuilder.Create<ScheduleJob>()
                .WithIdentity("UserDefinedJob")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("UserDefinedJobTrigger")
                .WithCronSchedule(cronExpression)
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }
    }
}