using Shop.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models.Accaunts
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        //[ValidEmailDomain(allowedDomain: ".com", ErrorMessage = "Email must end with .com")]
        public string Email { get; set; }
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password does not match")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
