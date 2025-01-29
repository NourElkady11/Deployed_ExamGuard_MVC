using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage ="First name is required")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; }

		[EmailAddress]
		public string Email { get; set; }

		[DataType(DataType.Password)]

		public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Password Dosent match the Confirm Password")]
		public string ConfirmPasword { get; set; }

		public bool Isagree { get; set; }
	}
}

