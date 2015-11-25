using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Nest;

namespace EmployeeDirectory
{
	public class EmployeeDirectoryResolver
	{
		private IUnityContainer container;
		private static EmployeeDirectoryResolver INSTANCE = new EmployeeDirectoryResolver();

		private EmployeeDirectoryResolver()
		{
			Init();
		}

		private void Init()
		{
			this.container = new UnityContainer();
			Register(container);
		}

		public IUnityContainer Register(IUnityContainer unityContainer)
		{
			unityContainer.RegisterInstance(typeof (IElasticClient),
				new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200"), "employeedirectory")));
			return unityContainer;
		}

		public static EmployeeDirectoryResolver Instance {
			get { return INSTANCE; }
		}
	}
}
