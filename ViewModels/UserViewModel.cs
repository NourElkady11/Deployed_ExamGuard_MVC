using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
	public class UserViewModel
	{
        public string? Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Password { get; set; }

      
        public IFormFile? Image { get; set; }

        public string? ImageName { get; set; }

        public IEnumerable<string>?Roles { get; set; }
}
}
