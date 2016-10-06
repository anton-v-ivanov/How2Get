using System.ServiceProcess;
using Common.Logging;
using HowToGet.Services.Quartz.Scheduler;

namespace HowToGet.Services.Quartz
{
	public partial class QuartzService : ServiceBase
	{
		private static readonly ILog CurrentLogger = LogManager.GetCurrentClassLogger();
		
		private ITaskScheduler _scheduler;

		public QuartzService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			CurrentLogger.Info("Service started");

			_scheduler = new TaskScheduler();
			_scheduler.Run();
		}

		protected override void OnStop()
		{
			if (_scheduler != null)
				_scheduler.Stop();

			CurrentLogger.Info("Service stopped");
		}

		public void StartConsole(string[] args)
		{
			OnStart(args);
		}

		public void StopConsole()
		{
			OnStop();
		}
	}
}
