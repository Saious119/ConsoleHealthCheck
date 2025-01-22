using Quartz;

namespace ConsoleHealthCheck.Jobs
{
    public class ScheduleJob : IJob
    {
        public string consoleName { get; set; }
        public Task Execute(IJobExecutionContext context)
        {
            var consoleName = context.MergedJobDataMap.GetString("consoleName");
            // Job logic here using consoleName
            Console.WriteLine($"User-defined job executed for console: {consoleName}");
            return Task.CompletedTask;
        }
        public ScheduleJob(string consoleName)
        {
            this.consoleName = consoleName;
        }
    }
}