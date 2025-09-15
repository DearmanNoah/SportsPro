using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SportsPro.Models
{
	public class Technician
	{

		public int TechnicianID { get; set; }

		[Required(ErrorMessage = "Please enter a name.")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter an email.")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter a phone number.")]
		public string Phone { get; set; } = string.Empty;
	}
}