using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
	{
		private Repository<Technician> technicians { get; set; }
		public TechnicianController(SportsProContext ctx) => technicians = new Repository<Technician>(ctx);

		[HttpGet]
        [Route("technicians")]
        public IActionResult List()
		{
			var options = new QueryOptions<Technician>
			{
				OrderBy = t => t.Name
			};
			var technicianList = technicians.List(options);
			return View("List", technicianList);
		}

		[HttpGet]
		public IActionResult Add()
		{
			ViewBag.Action = "Add";
			return View("EditTech", new Technician());
		}
		[HttpGet]
		public IActionResult Edit(int id)
		{
			ViewBag.Action = "Edit";
			var technician = technicians.Get(id);
			return View("EditTech", technician);
		}
		[HttpPost]
		public IActionResult Edit(Technician technician)
		{
			if (ModelState.IsValid)
			{
				if (technician.TechnicianID == 0) 
					technicians.Insert(technician);
				else
					technicians.Update(technician); 
				technicians.Save();
				return RedirectToAction("List", "Technician");
			}
			else
			{
				ViewBag.Action = (technician.TechnicianID == 0) ? "Add" : "Edit"; return View("EditTech", technician);
			}
		}
		[HttpGet]
		public IActionResult Delete(int id)
		{
			var technician = technicians.Get(id);
			var model = new DeleteConfirmModel
			{
				Id = technician.TechnicianID,
				Name = technician.Name
			};

			return View("DeleteTech", model);
		}
		[HttpPost]
		public IActionResult Delete(DeleteConfirmModel model)
		{
			var technician = technicians.Get(model.Id);
			technicians.Delete(technician);
			technicians.Save();
			return RedirectToAction("List", "Technician");
		}
	}
}
