using System.ComponentModel.DataAnnotations;

namespace MyStore.DTO
{
	public class ResetPasswordDto
	{
		[Required(ErrorMessage = "The Confirm Password field is required")]
		public string CurrentPassword { get; set; } = "";

		[Required(ErrorMessage = "The Confirm Password field is required")]
		public string Password { get; set; } = "";

		[Required(ErrorMessage = "The Confirm Password field is required")]
		[Compare("Password", ErrorMessage = "Confirm Password and Password do not match")]
		public string ConfirmPassword { get; set; } = "";
	}
}
