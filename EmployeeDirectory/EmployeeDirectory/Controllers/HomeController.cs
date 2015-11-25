using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EmployeeDirectory.Lib.Model;
using EmployeeDirectory.Lib.Repository;
using KendoCRUDService.Common;

namespace EmployeeDirectory.Controllers
{
	public class HomeController : Controller
	{
		private IEmployeeRecordRepository repository;
		private IEnumerable<EmployeeRecord> allRecords; // this is bad. but it's just for demo purposes.


		public HomeController(IEmployeeRecordRepository repository)
		{
			this.repository = repository;
			this.allRecords = this.repository.GetAll();
		}

		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public JsonResult GetAll()
		{
			var result = new JsonResult()
			{
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = Int32.MaxValue, // need scan/scroll... but this is just for the demo
				Data = allRecords
			};
			return result;
		}

		[HttpPost]
		public ActionResult Create(EmployeeRecord record)
		{
			repository.Add(record);
			return Json(record);
		}

		[HttpPost]
		public ActionResult Delete(EmployeeRecord record)
		{
			repository.Delete(record);
			return Json(record);
		}

		[HttpPost]
		public ActionResult Update(EmployeeRecord record)
		{
			repository.Update(record);
			return Json(record);
		}
	}
}