using BlazorCore.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlazorCore.Models.Requests
{
    public class ConfirmationCodeRequestModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Entity { get; set; }
        [Required]
        public NotificationEntityType EntityType { get; set; }
    }
}
