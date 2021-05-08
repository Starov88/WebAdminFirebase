using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorCore.Models.Requests
{
    public class RegistrationRequestModel : BaseAuthRequestModel
    {
        [MaxLength(128)]
        [EmailAddress]
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ConfirmationCode { get; set; }

        [MaxLength(32)]
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [MaxLength(32)]
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [MaxLength(128)]
        public string CompanyName { get; set; }
    }
}
