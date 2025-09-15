using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ValidationController : Controller
    {
        private Repository<Customer> data { get; set; }

        public ValidationController(SportsProContext ctx) => data = new Repository<Customer>(ctx);

        public JsonResult CheckEmail(string email)
        {
            if (ViewBag.Action == "Add")
            {
                var options = new QueryOptions<Customer>
                {
                    Where = t => t.Email == email
                };

                Customer customer = data.Get(options)!;

                if (customer == null)
                {
                    return Json(true);
                }
                else
                {
                    return Json($"The email {email} is already in the database.");
                }
            }
            else
            {
                return Json(true);
            }
        }
    }
}
