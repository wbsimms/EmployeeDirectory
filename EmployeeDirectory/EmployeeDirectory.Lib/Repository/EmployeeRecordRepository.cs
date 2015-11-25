using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Lib.Model;
using Nest;

namespace EmployeeDirectory.Lib.Repository
{
	public interface IEmployeeRecordRepository
	{
		IEnumerable<EmployeeRecord> GetAll();
		string Add(EmployeeRecord employeeRecord);
		bool Delete(EmployeeRecord employeeRecord);
		bool Update(EmployeeRecord employeeRecord);
	}

	public class EmployeeRecordRepository : IEmployeeRecordRepository
	{
		private IElasticClient client;

		public EmployeeRecordRepository(IElasticClient client)
		{
			this.client = client;
		}

		public IEnumerable<EmployeeRecord> GetAll()
		{
			return client.Search<EmployeeRecord>(s => s.Query(q => q.MatchAll()).Size(300000).Sort(x => x.OnField(f => f.Id))).Documents; // See https://gist.github.com/wbsimms/0657c47390a56b3cc5f6 for a scan/scroll example
		}

		public string Add(EmployeeRecord employeeRecord)
		{
			return client.Index<EmployeeRecord>(employeeRecord).Id;
		}

		public bool Delete(int id)
		{
			if (id > 0)
				return client.Delete<EmployeeRecord>(x => x.Id(id)).Found;
			throw new ApplicationException("Unable to find record to delete. Data sync error.\r\nId:\r\n" + Newtonsoft.Json.JsonConvert.SerializeObject(id));
		}


		public bool Delete(EmployeeRecord employeeRecord)
		{
			if (employeeRecord.Id > 0) return Delete(employeeRecord.Id);
			if (!string.IsNullOrEmpty(employeeRecord.Email))
			{
				var fromES = client.Search<EmployeeRecord>(x => x.Query(q => q.Term(t => t.OnField(f => f.Email).Value(employeeRecord.Email)))).Documents.FirstOrDefault();
				if (fromES != null)
				{
					return Delete(fromES.Id);
				}
			}
			throw new ApplicationException("Unable to find record to delete. Data sync error.\r\nRecord:\r\n"+Newtonsoft.Json.JsonConvert.SerializeObject(employeeRecord));
		}

		public bool Update(EmployeeRecord employeeRecord)
		{
			if (employeeRecord.Id == 0)
				throw new ApplicationException("No Id provided");
			var result = client.Update<EmployeeRecord>(x => x.Id(employeeRecord.Id).Doc(employeeRecord));
            return result.IsValid;
		}
	}
}
