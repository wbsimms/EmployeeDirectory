using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace EmployeeDirectory.Lib.Model
{
	[ElasticType(Name = "employeeRecord",IdProperty = "Id")]
	public class EmployeeRecord
	{
		public int Id { get; set; }
		[ElasticProperty(Name = "firstName")]
		public string FirstName { get; set; }
		[ElasticProperty(Name = "lastName")]
		public string LastName { get; set; }
		[ElasticProperty(Name = "address")]
		public string Address { get; set; }
		[ElasticProperty(Name = "jobTitle")]
		public string JobTitle { get; set; }
		[ElasticProperty(Name = "phone")]
		public string Phone { get; set; }
		[ElasticProperty(Name = "email")]
		public string Email { get; set; }

		public EmployeeRecord Clone() // There's no ICloneable<T> in .NET ... fun discussion on why.
		{
			// Ask me why I don't use a propery mapping utility here (like AutoMapper).
			return new EmployeeRecord()
			{
				Address = this.Address,
				Email = this.Email,
				FirstName = this.FirstName,
				Id = this.Id,
				JobTitle = this.JobTitle,
				LastName = this.LastName,
				Phone = this.Phone
			};
		}
	}
}
