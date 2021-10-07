using System.ComponentModel.DataAnnotations;

namespace Application.Common.Api
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "Operation successfully Done")]
        Success = 0,

        [Display(Name = "An error occurred on the server")]
        ServerError = 1,

        [Display(Name = "Submitted parameters are not valid")]
        BadRequest = 2,

        [Display(Name = "Not found")]
        NotFound = 3,

        [Display(Name = "List is empty")]
        ListEmpty = 4
    }
}