using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;

namespace HowToGet.IoC
{
    public class DependencyContainer
    {
		public static IContainer Container { get; private set; }

		public static void RegisterTypes(Assembly assembly)
		{
			var builder = new ContainerBuilder();

			builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly()); 
			
			// Register the Web API controllers.
			builder.RegisterApiControllers(assembly);
			builder.RegisterHubs(assembly);
			
			//builder.RegisterAssemblyTypes(assembly).Where(t =>!t.IsAbstract && typeof(ApiController).IsAssignableFrom(t))
			//	.InstancePerMatchingLifetimeScope(AutofacWebApiDependencyResolver.ApiRequestTag);
			
			Container = builder.Build();
		}

		public static T ResolveType<T>()
		{
			return Container.Resolve<T>();
		}
    }
}
