using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[EmailAddress]
		public string Email { get; set; }
	}
}
