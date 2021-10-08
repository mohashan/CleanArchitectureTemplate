using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.User
{
    public class SignInDto
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} Is Required")]
        [MaxLength(50, ErrorMessage = "Max length for {0} is {1} char")]
        public string UserName { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} Is Required")]
        [MaxLength(50, ErrorMessage = "Max length for {0} is {1} char")]
        public string Password { get; set; }
    }
}