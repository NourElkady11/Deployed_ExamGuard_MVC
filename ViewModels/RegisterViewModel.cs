using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
	//StudentViewModel
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^(?=.*[a-zA-Z].*[a-zA-Z].*[a-zA-Z]).+$", ErrorMessage = "First name must contain at least 3 letters")]
        public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^(?=.*[a-zA-Z].*[a-zA-Z].*[a-zA-Z]).+$", ErrorMessage = "Last name must contain at least 3 letters")]
        public string LastName { get; set; }

		[Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^(?=.*[a-zA-Z].*[a-zA-Z].*[a-zA-Z]).+$", ErrorMessage = "Username name must contain at least 3 letters")]
        public string Username { get; set; }

		[EmailAddress]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Password Dosent match the Confirm Password")]
		public string ConfirmPasword { get; set; }

		[Required(ErrorMessage = "phone is required")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "address is required")]
		public string? address { get; set; }

        public bool Isagree { get; set; }

		[Required]
        public IFormFile? Image { get; set; }

        public string? ImageName { get; set; }
    }
}

