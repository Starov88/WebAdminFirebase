using System.ComponentModel.DataAnnotations;

namespace BlazorCore.Models.Requests
{
    class RefreshTokenRequestModel : BaseAuthRequestModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Token { get; set; }
    }
}
