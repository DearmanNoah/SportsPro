using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class RegistrationController : Controller
	{
		private Repository<Customer> customers { get; set; }
		private Repository<Product> products { get; set; }
		public RegistrationController(SportsProContext ctx)
		{
			customers = new Repository<Customer>(ctx);
			products = new Repository<Product>(ctx);
		}
		public IActionResult Index()
		{

			var session = new SportsProSession(HttpContext.Session);
			string nameId = session.GetName();

			var options = new QueryOptions<Customer>
			{
				Includes = "Products",
				OrderBy = n => n.LastName,
				Where = m => m.FirstName == nameId
			};
			var customerList = customers.List(options).ToList();
			var customerFullName = customerList[0].FullName ?? customerList[0].FirstName ?? "No customer found";

			var productOptions = new QueryOptions<Product>
			{
				OrderBy = d => d.Name
			};
			var productList = products.List(productOptions).ToList();

			var model = new CustomersViewModel
			{
				Customers = customerList,
				CustomerName = customerFullName,
				Products = productList
			};

			return View("Index", model);
		}
		[HttpGet]
		public ViewResult ListByCustomer()
		{
			var session = new SportsProSession(HttpContext.Session);

			var options = new QueryOptions<Customer>
			{
				OrderBy = n => n.LastName
			};
			var customerList = customers.List(options).ToList();

			var model = new CustomersViewModel
			{
				CustomerName = session.GetName(),
				CCustomer = new Customer(),
				Customers = customerList

			};
			return View("GetCustomer", model);
		}

		[HttpPost]
		public IActionResult ListByCustomer(CustomersViewModel model)
		{
			TempData.Clear();
			if (model.CCustomer.CustomerID > 0)
			{
				var fCustomers = customers.Get(model.CCustomer.CustomerID);
				var session = new SportsProSession(HttpContext.Session);
				model.CustomerName = fCustomers.FirstName ?? "No name found";
				session.SetName(model.CustomerName);

				return RedirectToAction("Index", "Registration");
			}
			else
			{
				TempData["error"] = "Please select a customer";
				return RedirectToAction("ListByCustomer", "Registration");
			}
		}

		[HttpPost]
		public RedirectToActionResult RegisterProduct(CustomersViewModel model)
		{
			TempData.Clear();

			var customerOptions = new QueryOptions<Customer>
			{
				Includes = "Products",
				Where = w => w.CustomerID == model.CCustomer.CustomerID,
				OrderBy = n => n.LastName
			};
			var customer = customers.Get(customerOptions);

			if (customer != null)
			{
				var productOptions = new QueryOptions<Product>
				{
					Where = m => m.ProductID == model.CProduct.ProductID,
					OrderBy = d => d.Name
				};
				var product = products.Get(productOptions);

				if (product != null)
				{
					if (!customer.Products.Contains(product))
					{
						customer.Products.Add(product);
						customers.Update(customer);
						customers.Save();
					}
					else
					{
						TempData["error"] = "This product is already registered";
					}
				}
			}

			return RedirectToAction("Index", "Registration");
		}

		[HttpPost]
		public IActionResult Delete(int id, int id2)
		{
			var customerOptions = new QueryOptions<Customer>
			{
				Includes = "Products",
				Where = w => w.CustomerID == id,
				OrderBy = n => n.LastName
			};
			var customer = customers.Get(customerOptions);

			if (customer != null)
			{
				var product = customer.Products.FirstOrDefault(p => p.ProductID == id2);
				if (product != null)
				{
					customer.Products.Remove(product);
					customers.Save();
				}
			}

			return RedirectToAction("Index", "Registration");
		}
	}
}
