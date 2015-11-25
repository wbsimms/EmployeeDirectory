using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Lib.Repository;
using Microsoft.Practices.Unity;

namespace EmployeeDirectory.Lib
{
	public class EmployeeDirectoryLibResolver
	{
		private IUnityContainer container;
		private static EmployeeDirectoryLibResolver INSTANCE = new EmployeeDirectoryLibResolver();

		private EmployeeDirectoryLibResolver()
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
			unityContainer.RegisterType<IEmployeeRecordRepository, EmployeeRecordRepository>();
			return unityContainer;
		}

		public static EmployeeDirectoryLibResolver Instance {
			get { return INSTANCE; }
		}
	}
}
