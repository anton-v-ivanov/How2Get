using Autofac;
using HowToGet.Notifications.Configuration;
using HowToGet.Notifications.Interfaces;
using HowToGet.Notifications.Providers;
using HowToGet.Repository.Interfaces;
using HowToGet.Repository.Repositories;

namespace HowToGet.IoC.Modules
{
	public class NotificationModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<NotificationRepository>().As<INotificationRepository>().SingleInstance();
			
			builder.RegisterType<NotificationProvider>().As<INotificationProvider>().SingleInstance();

			builder.RegisterType<EmailNotificationProvider>().As<IEmailNotificationProvider>()
					.WithParameter("notificationPath", NotificationsConfig.Instance.NotificationsPath)
					.SingleInstance();
			
			base.Load(builder);
		}
	}
}