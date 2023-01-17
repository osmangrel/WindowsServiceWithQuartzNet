using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Job;

namespace DenemeService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // Grab the Scheduler instance from the Factory 
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                // and start it off
                scheduler.Start();

                WriteLogFile("Scheduler is started");
                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<MailSendMarketPrice>()
                    .WithIdentity("MAIL_SEND_MARKETPRICE_JOB", "group1")
                    .Build();

                IJobDetail job2 = JobBuilder.Create<DifferentJob>()
                   .WithIdentity("DIFFERENT_JOB", "group1")
                   .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                //0 30 15 * * ? her gün 15:30 da çalış
                //https://www.freeformatter.com/cron-expression-generator-quartz.html


                //1.işi 5 saniyede bir yap. 
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithCronSchedule("*/5 * * ? * * *")
                    .ForJob(job)
                    .Build();

                //2.işi 10 saniyede bir yap. 
                ITrigger trigger2 = TriggerBuilder.Create()
                 .WithIdentity("trigger2", "group1")
                 .StartNow()
                 .WithCronSchedule("*/10 * * ? * * *")
                 .ForJob(job2)
                 .Build();

                // Tell Quartz to schedule the job using our trigger

                scheduler.ScheduleJob(job, trigger);
                scheduler.ScheduleJob(job2, trigger2);


                var nextFireTime = trigger.GetNextFireTimeUtc();
                if (nextFireTime != null)
                    WriteLogFile("First Job Next Fire Time: " + nextFireTime.Value.LocalDateTime.ToString());
                var nextFireTime2 = trigger2.GetNextFireTimeUtc();
                if (nextFireTime2 != null)
                    WriteLogFile("Second Job Next Fire Time: " + nextFireTime2.Value.LocalDateTime.ToString());


            }
            catch (SchedulerException se)
            {
                WriteLogFile(se.Message);
            }

            WriteLogFile("Program is going to quit");
        }
        public class MailSendMarketPrice : IJob
        {
            public async void Execute(IJobExecutionContext context)
            {
                await Task.Run(() => runJob());
            }
            private static async Task runJob()
            {
                //httpgetset url
                WriteLogFile("MailSendMarketPrice working. I am First Job.");
            }
        }
        public class DifferentJob : IJob
        {
            public async void Execute(IJobExecutionContext context)
            {
                await runJob();
            }
            private static async Task runJob()
            {
                //httpgetset url
                WriteLogFile("DifferentJob working. I am Second Job.");
            }
        }

        protected override void OnStop()
        {
            WriteLogFile("Stopping windows service");
        }
        public static void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\OsmanCronLogFile.txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }
    }
}
