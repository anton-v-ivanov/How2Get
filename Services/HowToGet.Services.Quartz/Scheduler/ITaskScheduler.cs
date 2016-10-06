namespace HowToGet.Services.Quartz.Scheduler
{
	public interface ITaskScheduler
	{
		string Name { get; }
		void Run();
		void Stop(); 
	}
}