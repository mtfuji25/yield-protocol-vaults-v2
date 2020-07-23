using Quartz;
using Quartz.Impl;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DataManager.Models
{
    public class JobScheduler
    {
        #region Methods
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();
            IJobDetail optimize = JobBuilder.Create<OptimizeDatabase>().Build();
            await scheduler.ScheduleJob(optimize, TriggerBuilder.Create()
                .WithIdentity("Optimize Database")
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(1, 0))
                  )
                .Build());

            IJobDetail rebuild = JobBuilder.Create<RebuildSearchIndex>().Build();
            await scheduler.ScheduleJob(rebuild, TriggerBuilder.Create()
                .WithIdentity("Rebuild Search Index")
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(2, 0))
                  )
                .Build());
        }

        public class OptimizeDatabase : IJob
        {
            #region Methods
            public async Task Execute(IJobExecutionContext context)
            {
                await Task.Run(() =>
                {
                    Stopwatch stopWatch = new Stopwatch();
                    using (DataContext dataContext = new DataContext())
                    {
                        stopWatch.Start();
                        string result = "Ok";
                        try
                        {
                            // Rebuild Indexes
                            dataContext.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable \"ALTER INDEX ALL ON ? REBUILD WITH (ONLINE=OFF)\"");
                            // Update Statistics
                            dataContext.Database.ExecuteSqlCommand("EXEC sp_updatestats;");
                            // Clean up old scheduled task logs
                            dataContext.Database.ExecuteSqlCommand("Delete from tblTaskLogs where Date < {0}", new object[] { DateTime.Now.AddDays(-30) });
                        }
                        catch (Exception ex)
                        {
                            result = ex.Message + Environment.NewLine + ex.StackTrace;
                        }
                        stopWatch.Stop();
                        //dataContext.TaskLogs.Add(new TaskLog
                        //{
                        //    Date = DateTime.Now,
                        //    ElapsedTicks = stopWatch.ElapsedMilliseconds,
                        //    Result = result,
                        //    Task = "Optimize Database"
                        //});
                        dataContext.SaveChanges();
                    }
                });
            }
            #endregion
        }

        public class RebuildSearchIndex : IJob
        {
            #region Methods
            public async Task Execute(IJobExecutionContext context)
            {
                await Task.Run(() =>
                {
                    //Search.Index.All();
                });
            }
            #endregion
        }
        #endregion
    }

    [Table("TaskLogs")]
    public class TaskLog
    {
        #region Properties
        public DateTime Date { get; set; }

        public long ElapsedTicks { get; set; }

        public string Result { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int TaskLogId { get; set; }

        public string Task { get; set; }

        [NotMapped]
        public TimeSpan TimeSpan
        {
            get
            {
                return TimeSpan.FromTicks(ElapsedTicks);
            }
            set
            {
                ElapsedTicks = value.Ticks;
            }
        }
        #endregion
    }
}