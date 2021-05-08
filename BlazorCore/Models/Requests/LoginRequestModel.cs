using System.ComponentModel.DataAnnotations;

namespace BlazorCore.Models.Requests
{
    public class LoginRequestModel : BaseAuthRequestModel
    {
        [MaxLength(128)]
        [EmailAddress]
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ConfirmationCode { get; set; }
    }
}
