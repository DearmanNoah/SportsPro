using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
	{
		private Repository<Customer> customers { get; set; }
		private Repository<Country> countries { get; set; }
		public CustomerController(SportsProContext ctx)
		{
			customers = new Repository<Customer>(ctx);
			countries = new Repository<Country>(ctx);
		}
		[HttpGet]
        [Route("customers")]
        public IActionResult List()
		{
			var options = new QueryOptions<Customer>
			{
				OrderBy = n => n.LastName
			};
			var customerList = customers.List(options);
			return View("List", customerList);
		}

		[HttpGet]
		public IActionResult Add()
		{
			var options = new QueryOptions<Country>();
			var countryList = countries.List(options);
			ViewData["Countries"] = countryList;
			ViewBag.Action = "Add";
			return View("EditCustomer", new Customer());
		}
		[HttpGet]
		public IActionResult Edit(int id)
		{
			var options = new QueryOptions<Country>();
			var countryList = countries.List(options);
			ViewData["Countries"] = countryList;
			ViewBag.Action = "Edit";
			var customer = customers.Get(id);
			return View("EditCustomer", customer);
		}
		[HttpPost]
		public IActionResult Edit(Customer customer)
		{
			var emailCheck = customers.Get(new QueryOptions<Customer>
			{
				Where = t => t.Email == customer.Email
			});
			if (emailCheck != null && ViewBag.Action == "Add")
			{
				ModelState.AddModelError("Email",
					"Email address already in use.");
			}
			emailCheck = null!;
			if (customer.CountryID == "select a country...")
			{
				ModelState.AddModelError("Country", "Please select a country");
			}
			if (ModelState.IsValid)
			{
				if (customer.CustomerID == 0)
				{
					customers.Insert(customer);
				}
				else
				{
					var existingCustomer = customers.Get(new QueryOptions<Customer>
					{
						Where = c => c.CustomerID == customer.CustomerID
					});
					if (existingCustomer != null)
					{
						customers.Entry(existingCustomer).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
					}
					customers.Update(customer);
				}
				customers.Save();
				return RedirectToAction("List", "Customer");
			}
			else
			{
				ViewBag.Action = (customer.CustomerID == 0) ? "Add" : "Edit"; return View("EditCustomer", customer);
			}
		}
		[HttpGet]
		public IActionResult Delete(int id)
		{
			var customer = customers.Get(id);
			var model = new DeleteConfirmModel
			{
				Id = customer.CustomerID,
				Name = customer.FullName
			};
			
			return View("DeleteCustomer", model);
		}
		[HttpPost]
		public IActionResult Delete(DeleteConfirmModel model)
		{
            var customer = customers.Get(model.Id);
            customers.Delete(customer);
			customers.Save();
			return RedirectToAction("List", "Customer");
		}
	}
}