using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
	{
		private Repository<Product> products { get; set; }
		public ProductController(SportsProContext ctx) => products = new Repository<Product>(ctx);

		[HttpGet]
        [Route("products")]
        public ViewResult List()
		{
			var options = new QueryOptions<Product>
			{
				OrderBy = d => d.Name
			};
			var productList = products.List(options);
			return View("List", productList);
		}

		[HttpGet]
		public ViewResult Add()
		{
			ViewBag.Action = "Add";
			return View("EditProduct", new Product());
		}
		[HttpGet]
		public ViewResult Edit(int id)
		{
			ViewBag.Action = "Edit";
			var product = products.Get(id);
			return View("EditProduct", product);
		}
		[HttpPost]
		public RedirectToActionResult Edit(Product product)
		{
			if (ModelState.IsValid)
			{
				if (product.ProductID == 0)
				{
					products.Insert(product);
					TempData["message"] = product.Name + " added";
				}
				else
				{
					products.Update(product);
					TempData["message"] = product.Name + " edited";
				}
				products.Save();
				return RedirectToAction("List", "Product");
			}
			else
			{
				ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit"; return RedirectToAction("EditProduct", "Product", product);
			}
		}
		[HttpGet]
		public ViewResult Delete(int id)
		{
			var product = products.Get(id);
            var model = new DeleteConfirmModel
            {
                Id = product.ProductID,
                Name = product.Name
            };

            return View("DeleteProduct", model);
		}
		[HttpPost]
		public RedirectToActionResult Delete(DeleteConfirmModel model)
		{
            TempData["message"] = model.Name + " deleted";
            var product = products.Get(model.Id);
            products.Delete(product);
			products.Save();
            return RedirectToAction("List", "Product");
		}
	}
}