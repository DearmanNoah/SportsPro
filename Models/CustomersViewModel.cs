namespace SportsPro.Models
{
	public class CustomersViewModel
	{
		public string CustomerName = string.Empty;
		public List<Customer> Customers { get; set; } = null!;
		public Customer CCustomer { get; set; } = null!;
		public List<Product> Products { get; set; } = null!;
		public Product CProduct { get; set; } = null!;
	}
}
