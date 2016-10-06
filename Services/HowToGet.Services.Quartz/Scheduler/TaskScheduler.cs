using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using Quartz;
using Quartz.Impl;

namespace HowToGet.Services.Quartz.Scheduler
{
	public class TaskScheduler : ITaskScheduler
	{
		private IScheduler _scheduler;

		public string Name
		{
			get { return GetType().Name; }
		}

		public void Run()
		{
			var properties = new NameValueCollection();

			properties["quartz.scheduler.instanceName"] = "HowToGet.Services.Quartz.Scheduler";

			properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";

			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			properties["quartz.plugin.xml.fileNames"] = path + @"\jobs.xml";

			ISchedulerFactory schedulerFactory = new StdSchedulerFactory(properties);
			_scheduler = schedulerFactory.GetScheduler();

			_scheduler.Start();
		}

		public void Stop()
		{
			_scheduler.Shutdown();
		}
	}
}