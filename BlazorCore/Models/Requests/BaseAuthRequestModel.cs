using BlazorCore.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorCore.Models.Requests
{
    public class BaseAuthRequestModel
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string DeviceId { get; set; }
        [Required]
        public ClientType ClientType { get; set; }
        [Range(-64800, 64800)]
        public int? TimezoneSeconds { get; set; }
    }
}
