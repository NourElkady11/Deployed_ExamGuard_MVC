using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
	public class ResetPasswordViewModel
	{

		[DataType(DataType.Password),Required]
		public string Password { get; set; }

		[Compare(nameof(Password), ErrorMessage = "Password Dosen't match the Confirm Password"),Required]
		public string ConfirmPasword { get; set; }

        public string? email { get; set; }

        public string? token { get; set; }
    }
}
