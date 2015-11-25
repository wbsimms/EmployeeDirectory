using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EmployeeDirectory.Lib;
using EmployeeDirectory.Lib.Model;
using EmployeeDirectory.Lib.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nest;

namespace EmployeeDirectory.Lib.Test.Repository
{
	[TestClass]
	public class EmployeeRecordRepositoryTest
	{
		[TestMethod]
		public void ConstuctorTest()
		{
			EmployeeRecordRepository repository = new EmployeeRecordRepository(null);
			Assert.IsNotNull(repository);
		}

		[TestMethod]
		public void GetAllTest()
		{
			Mock<IElasticClient> client = new Mock<IElasticClient>();
			client.Setup(x => x.Search<EmployeeRecord>(It.IsAny<Func<SearchDescriptor<EmployeeRecord>,SearchDescriptor<EmployeeRecord>>>()).Documents).Returns(
				() =>
				{
					return new List<EmployeeRecord>()
					{
						new EmployeeRecord() { },
						new EmployeeRecord() { },
						new EmployeeRecord() { }
					};
				});

			EmployeeRecordRepository repository = new EmployeeRecordRepository(client.Object);
			Assert.IsNotNull(repository);
			var results = repository.GetAll();
			Assert.IsNotNull(results);
			Assert.AreEqual(3,results.Count());
		}

		[TestMethod]
		public void AddTest()
		{
			Mock<IElasticClient> client = new Mock<IElasticClient>();
			client.Setup(x => x.Index<EmployeeRecord>(It.IsAny<EmployeeRecord>(),null).Id).Returns("asdasdf");

			EmployeeRecordRepository repository = new EmployeeRecordRepository(client.Object);
			Assert.IsNotNull(repository);
			var results = repository.Add(new EmployeeRecord()
			{
				Address = "Neverland",
				FirstName = "Peter",
				LastName = "Pan",
				Email = "shouldhave@slept.in",
				JobTitle = "Lord of the Flies",
				Phone = "555-555-5555"
			});
			Assert.IsNotNull(results);
			Assert.AreEqual("asdasdf", results);
		}

		[TestMethod]
		public void DeleteByIdTest()
		{
			Mock<IElasticClient> client = new Mock<IElasticClient>();
			client.Setup(x => x.Delete<EmployeeRecord>(It.IsAny<Func<DeleteDescriptor<EmployeeRecord>,DeleteDescriptor<EmployeeRecord>>>()).Found).Returns(true);

			EmployeeRecordRepository repository = new EmployeeRecordRepository(client.Object);
			Assert.IsNotNull(repository);
			var results = repository.Delete(new EmployeeRecord()
			{
				Id = 9999,
				Email = "shouldhave@slept.in",
			});
			Assert.IsNotNull(results);
			Assert.AreEqual(true, results);
			client.Verify(x => x.Delete<EmployeeRecord>(It.IsAny<Func<DeleteDescriptor<EmployeeRecord>, DeleteDescriptor<EmployeeRecord>>>()).Found, Times.Exactly(1));
		}

		[TestMethod]
		public void DeleteWithTest()
		{
			Mock<IElasticClient> client = new Mock<IElasticClient>();
			client.Setup(x => x.Delete<EmployeeRecord>(It.IsAny<Func<DeleteDescriptor<EmployeeRecord>, DeleteDescriptor<EmployeeRecord>>>()).Found).Returns(true);

			EmployeeRecordRepository repository = new EmployeeRecordRepository(client.Object);
			Assert.IsNotNull(repository);
			var results = repository.Delete(9999);
			Assert.IsNotNull(results);
			Assert.AreEqual(true, results);
			client.Verify(x => x.Delete<EmployeeRecord>(It.IsAny<Func<DeleteDescriptor<EmployeeRecord>, DeleteDescriptor<EmployeeRecord>>>()).Found, Times.Exactly(1));
		}


		[TestMethod]
		public void DeleteByEmailTest()
		{
			Mock<IElasticClient> client = new Mock<IElasticClient>();
			client.Setup(
				x =>
					x.Delete<EmployeeRecord>(It.IsAny<Func<DeleteDescriptor<EmployeeRecord>, DeleteDescriptor<EmployeeRecord>>>())
						.Found).Returns(true);
			client.Setup(
				x =>
					x.Search<EmployeeRecord>(It.IsAny<Func<SearchDescriptor<EmployeeRecord>, SearchDescriptor<EmployeeRecord>>>())
						.Documents).Returns(
							new List<EmployeeRecord>()
							{
								new EmployeeRecord() {Id = 9999}
							});

			EmployeeRecordRepository repository = new EmployeeRecordRepository(client.Object);
			Assert.IsNotNull(repository);
			var results = repository.Delete(new EmployeeRecord()
			{
				Email = "shouldhave@slept.in",
			});
			Assert.IsNotNull(results);
			Assert.AreEqual(true, results);
			client.Verify(x => x.Delete<EmployeeRecord>(It.IsAny<Func<DeleteDescriptor<EmployeeRecord>, DeleteDescriptor<EmployeeRecord>>>()).Found, Times.Exactly(1));
			client.Verify(x =>
				x.Search<EmployeeRecord>(It.IsAny<Func<SearchDescriptor<EmployeeRecord>, SearchDescriptor<EmployeeRecord>>>())
					.Documents, Times.Exactly(1));
		}

		[TestMethod]
		public void UpdateTest()
		{
			var response = new Mock<IUpdateResponse>();
			response.SetupGet(x => x.IsValid).Returns(true);
            Mock<IElasticClient> client = new Mock<IElasticClient>();
			client.Setup(
				x =>
					x.Update<EmployeeRecord>(It.IsAny<Func<UpdateDescriptor<EmployeeRecord, EmployeeRecord>, UpdateDescriptor<EmployeeRecord, EmployeeRecord>>>())
						).Returns(response.Object );

			EmployeeRecordRepository repository = new EmployeeRecordRepository(client.Object);
			Assert.IsNotNull(repository);
			repository.Update(new EmployeeRecord()
			{
				Id = 9999
			});

			client.Verify(x =>
					x.Update<EmployeeRecord>(It.IsAny<Func<UpdateDescriptor<EmployeeRecord, EmployeeRecord>, UpdateDescriptor<EmployeeRecord, EmployeeRecord>>>())
						, Times.Exactly(1));
		}
	}
}
