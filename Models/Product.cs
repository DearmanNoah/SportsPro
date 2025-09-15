using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SportsPro.Models
{
	public class Product
	{
		public Product() => Customers = new HashSet<Customer>();

		public int ProductID { get; set; }

		[Required(ErrorMessage = "Please enter a product code.")]
		public string ProductCode { get; set; } = string.Empty;
		[Required(ErrorMessage = "Please enter a name")]
		public string Name { get; set; } = string.Empty;
		[Column(TypeName = "decimal(8,2)")]
		[Required(ErrorMessage = "Please enter a yearly price.")]
		[Range(0, double.MaxValue, ErrorMessage = "Yearly price must be above 0.")]
		public decimal YearlyPrice { get; set; }
		[Required(ErrorMessage = "Please enter a date.")]
		public DateTime ReleaseDate { get; set; } = DateTime.Now;

		public ICollection<Customer> Customers { get; set;}
	}
}
