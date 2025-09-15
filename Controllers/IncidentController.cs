using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using SportsPro.Models;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
	{
		private Repository<Product> products;
		private Repository<Technician> technicians;
		private Repository<Customer> customers;
		private Repository<Incident> incidents;
		public IncidentController(SportsProContext ctx)
		{
			products = new Repository<Product>(ctx);
			technicians = new Repository<Technician>(ctx);
			customers = new Repository<Customer>(ctx);
			incidents = new Repository<Incident>(ctx);
		}
		[HttpGet]
		[Route("incidents")]
		public IActionResult List(string id = "All")
		{
			var incidentsOptions = new QueryOptions<Incident> {
				Includes = "Customer, Product",
				OrderBy = m => m.Title
			};

			if (id == "Unassigned")
			{
				incidentsOptions.Where = c => c.CustomerID == -1;
				incidentsOptions.OrderBy = m => m.Title;
			}
			if (id == "Open")
			{
				incidentsOptions.Where = i => i.DateClosed == null;
				incidentsOptions.OrderBy = m => m.Title;
			}

			var incidentList = incidents.List(incidentsOptions).ToList();
			var model = new IncidentsViewModel
			{
				Incidents = incidentList,
				SelectedIncidentStatus = id
			};
			return View("List", model);
		}

		public ViewResult ListByTech()
		{
			var incidentsOptions = new QueryOptions<Incident>
			{
				Includes = "Customer, Product",
				OrderBy = m => m.Title
			};
			var incidentList = incidents.List(incidentsOptions).ToList();


			var technicianOptions = new QueryOptions<Technician>
			{
				OrderBy = t => t.Name
			};
			var technicianList = technicians.List(technicianOptions).ToList();

			var session = new SportsProSession(HttpContext.Session);
			var model = new IncidentsViewModel
			{
				Technicians = technicianList,
				Incidents = incidentList,
				TechName = session.GetName(),
				CIncident = new Incident()
			};
			return View("GetTech", model);
		}
		[HttpPost]
		public RedirectToActionResult ListByTech(IncidentsViewModel model)
		{
			TempData.Clear();
			if (model.CIncident.TechnicianID > 0)
			{
				var techniciansOptions = technicians.Get(model.CIncident.TechnicianID);
				var session = new SportsProSession(HttpContext.Session);
				model.TechName = techniciansOptions.Name ?? "No name found";
				session.SetName(model.TechName);

				return RedirectToAction("TechList", "Incident", model.CIncident.TechnicianID);
			}
			else
				TempData["error"] = "Please select a technician";
			return RedirectToAction("ListByTech", "Incident");
		}

		public ViewResult TechList()
		{

			var session = new SportsProSession(HttpContext.Session);
			string nameId = session.GetName();

			var technicianOptions = new QueryOptions<Technician>
			{
				OrderBy = t => t.Name
			};
			var technicianList = technicians.List(technicianOptions).ToList();

			var incidentsOptions = new QueryOptions<Incident>
			{
				Includes = "Customer, Product",
				Where = i => i.Technician.Name == nameId && i.DateClosed == null, 
				OrderBy = m => m.Title
			};
			var incidentList = incidents.List(incidentsOptions).ToList();

			var model = new IncidentsViewModel
			{
				Incidents = incidentList,
				Technicians = technicianList,
				TechName = session.GetName()
			};

			return View("TechList", model);
		}

		[HttpGet]
		public IActionResult Add()
		{
			var technicianOptions = new QueryOptions<Technician>
			{
				OrderBy = t => t.Name
			};
			var technicianList = technicians.List(technicianOptions).ToList();

			var customerOptions = new QueryOptions<Customer>
			{
				OrderBy = n => n.LastName
			};
			var customerList = customers.List(customerOptions).ToList();

			var productOptions = new QueryOptions<Product>
			{
				OrderBy = d => d.Name
			};
			var productList = products.List(productOptions).ToList();


			ViewData["Technicians"] = technicianList;
			ViewData["Customers"] = customerList;
			ViewData["Products"] = productList;

			var model = new IncidentsViewModel
			{
				CIncident = new Incident(),
				Action = "Add"
			};
			return View("EditIncident", model);
		}
		[HttpGet]
		public IActionResult Edit(int id)
		{
			var newIncident = incidents.Get(id) ?? new Incident();

			var technicianOptions = new QueryOptions<Technician>
			{
				OrderBy = t => t.Name
			};
			var technicianList = technicians.List(technicianOptions).ToList();

			var customerOptions = new QueryOptions<Customer>
			{
				OrderBy = n => n.LastName
			};
			var customerList = customers.List(customerOptions).ToList();

			var productOptions = new QueryOptions<Product>
			{
				OrderBy = d => d.Name
			};
			var productList = products.List(productOptions).ToList();


            ViewData["Technicians"] = technicianList;
            ViewData["Customers"] = customerList;
            ViewData["Products"] = productList;

            var model = new IncidentsViewModel
			{
				CIncident = newIncident,
				Action = "Edit"
			};
			return View("EditIncident", model);
		}
		[HttpPost]
		public IActionResult Edit(IncidentsViewModel incident)
		{
			ModelState.Clear();
			if (ModelState.IsValid)
			{
				if (incident.CIncident.ProductID == 0)
					incidents.Insert(incident.CIncident);
				else
					incidents.Update(incident.CIncident);
				incidents.Save();
				return RedirectToAction("List", "Incident");
			}
			else
			{
				incident.Action = (incident.CIncident.IncidentID == 0) ? "Add" : "Edit"; return View("EditIncident", incident.CIncident);
			}
		}
		[HttpGet]
		public IActionResult Delete(int id)
		{
			var incident = incidents.Get(id);
			var model = new DeleteConfirmModel
			{
				Id = incident.IncidentID,
				Name = incident.Title
			};

			return View("DeleteIncident", model);
		}
		[HttpPost]
		public IActionResult Delete(DeleteConfirmModel model)
		{
			var incident = incidents.Get(model.Id);
			incidents.Delete(incident);
			incidents.Save();
			return RedirectToAction("List", "Incident");
		}
		[HttpGet]
		public IActionResult TechEdit(int id)
		{
			var newIncident = incidents.Get(id) ?? new Incident();

			var technicianOptions = new QueryOptions<Technician>
			{
				OrderBy = t => t.Name
			};
			var technicianList = technicians.List(technicianOptions).ToList();

			var customerOptions = new QueryOptions<Customer>
			{
				OrderBy = n => n.LastName
			};
			var customerList = customers.List(customerOptions).ToList();

			var productOptions = new QueryOptions<Product>
			{
				OrderBy = d => d.Name
			};
			var productList = products.List(productOptions).ToList();

			var model = new IncidentsViewModel
			{
				Customers = customerList,
				Products = productList,
				Technicians = technicianList,
				CIncident = newIncident,
				Action = "Edit"
			};
			return View("TechEditIncident", model);
		}
		[HttpPost]
		public IActionResult TechEdit(IncidentsViewModel incident)
		{
			ModelState.Clear();
			if (ModelState.IsValid)
			{
				incidents.Update(incident.CIncident);
				incidents.Save();
				return RedirectToAction("TechList", "Incident");
			}
			else
			{
				incident.Action = (incident.CIncident.IncidentID == 0) ? "Add" : "Edit"; 
				return View("TechEditIncident", incident.CIncident);
			}
		}
	}
}