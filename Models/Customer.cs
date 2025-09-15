using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
	public class Customer
	{
		public Customer() => Products = new HashSet<Product>();

		public int CustomerID { get; set; }
		[Required(ErrorMessage = "Please enter a first name.")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 50 characters.")]
		public string FirstName { get; set; } = string.Empty;
		[Required(ErrorMessage = "Please enter a last name")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 50 characters.")]
		public string LastName { get; set; } = string.Empty;
		[Required(ErrorMessage = "Please enter an address")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Address must be between 1 and 50 characters.")]
		public string Address { get; set; } = string.Empty;
		[Required(ErrorMessage = "Please enter a city")]
		public string City { get; set; } = string.Empty;
		[Required(ErrorMessage = "Please enter a state")]
		public string State { get; set; } = string.Empty;
		[Required(ErrorMessage = "Please enter a postal code")]
		[StringLength(20, MinimumLength = 1, ErrorMessage = "Postal Code must be between 1 and 20 characters.")]
		public string PostalCode { get; set; } = string.Empty;
		[RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$", ErrorMessage = "Please enter a valid phone number Ex: (999) 999-9999")]
		public string? Phone { get; set; }
		[DataType(DataType.EmailAddress)]
		[Remote("CheckEmail","Validation")]
		public string? Email { get; set; }
		[StringLength(2, ErrorMessage = "You must enter a country from the list.")]
        public string CountryID { get; set; } = string.Empty; // foreign key property
		[ValidateNever]
        public Country Country { get; set; } = null!;           // navigation property

        public string FullName => FirstName + " " + LastName;   // read-only property

		public ICollection<Product> Products { get; set; }
	}
}