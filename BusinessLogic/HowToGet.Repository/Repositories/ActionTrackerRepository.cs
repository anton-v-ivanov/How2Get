using System;
using HowToGet.Models.Analytics;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using NLog;

namespace HowToGet.Repository.Repositories
{
	public class ActionTrackerRepository : IActionTrackerRepository
	{
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public void RecordAction(ActionBase action)
		{
			try
			{
				var collection = MongoHelper.Database.GetCollection<ActionBase>("actions");
				collection.Insert(action);
			}
			catch (Exception exception)
			{
				_logger.ErrorException("Insert tracking failed", exception);
			}
		}
	}
}