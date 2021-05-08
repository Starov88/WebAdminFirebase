using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorCore.Models.Users
{
    public class UserModel : UserBriefModel
    {
        [Required]
        public List<TeamBriefModel> Teams { get; set; }
    }
}
