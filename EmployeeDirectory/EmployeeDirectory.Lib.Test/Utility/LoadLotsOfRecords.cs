using System;
using System.Linq;
using EmployeeDirectory.Lib.Model;
using EmployeeDirectory.Lib.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nest;

namespace EmployeeDirectory.Lib.Test.Utility
{
	[TestClass]
	public class LoadLotsOfRecords
	{
		[TestMethod,TestCategory("Utility"),Ignore]
		public void LoadEmUp()
		{
			var bulkDescriptor = new BulkDescriptor();
			foreach (int i in Enumerable.Range(1,30000))
			{
				var record = new EmployeeRecord()
				{
					Address = "asdfdsfa",
					Email = "adfdfa@dsfsdf.com",
					FirstName = "firstName"+i,
					LastName = "lastName"+i,
					JobTitle = "sfadsdaf",
					Phone = "555-555-5555",
					Id = i
				};
				bulkDescriptor.Index<EmployeeRecord>(r => r.Document(record));
			}
			var client = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200"), "employeedirectory"));
			client.Bulk(bulkDescriptor);
		}

		[TestMethod,TestCategory("Utility"),Ignore]
		public void LoadAll()
		{
			EmployeeRecordRepository repo = new EmployeeRecordRepository(new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200"), "employeedirectory")));
			var all = repo.GetAll();
		}
	}
}
