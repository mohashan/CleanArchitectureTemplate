using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.User
{
    public class SignInDto
    {
        [Display(Name = "grant_type")]
        [Required(ErrorMessage = "{0} Is Required")]
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }

        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}