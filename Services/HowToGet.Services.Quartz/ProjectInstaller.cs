using System.ComponentModel;
using System.Configuration.Install;

namespace HowToGet.Services.Quartz
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
		}
	}
}
